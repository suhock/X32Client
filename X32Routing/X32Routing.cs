using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Suhock.Osc;
using Suhock.X32.Types.Sets;
using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;
using Suhock.X32.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Suhock.X32.Routing
{
    public sealed class X32Routing : IDisposable
    {
        private readonly X32Client _client;

        public X32Routing(X32RoutingConfig config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            _client = new X32Client(Config.Address, Config.Port);

            _client.MessageSent += (source, msg) =>
            {
                X32ConsoleLogger.WriteSend(_client, msg);
                _client.Send(msg);
            };
        }

        public X32RoutingConfig Config { get; }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task Run()
        {
            // list of all channels in spreadsheet
            IList<ChannelState> channels = ReadSheet();

            _client.Run();

            Dictionary<int, HeadampState> headamps = new Dictionary<int, HeadampState>(channels.Count);

            foreach (ChannelState channel in channels)
            {
                OscMessage haMsg = await QueryAsync("/-ha/" + (channel.Id - 1).ToString().PadLeft(2, '0') + "/index").ConfigureAwait(true);
                int haIndex = haMsg.GetValue<int>(0);

                if (haIndex > -1)
                {
                    if (!headamps.ContainsKey(haIndex))
                    {
                        headamps[haIndex] = new HeadampState()
                        {
                            Index = haMsg.GetValue<int>(0),
                            Gain = (await QueryAsync("/headamp/" + haIndex.ToString().PadLeft(3, '0') + "/gain").ConfigureAwait(true)).GetValue<float>(0),
                            Phantom = (await QueryAsync("/headamp/" + haIndex.ToString().PadLeft(3, '0') + "/phantom").ConfigureAwait(true)).GetValue<int>(0)
                        };
                    }

                    channel.OriginalHeadamp = headamps[haIndex];
                }
                else
                {
                    channel.OriginalHeadamp = null;
                }
            }

            foreach (var channel in channels)
            {
                int uiIndex = X32Util.ConvertStringToUserInIndex(channel.Source);
                int haIndex = X32Util.ConvertUserInIndexToHeadampIndex(uiIndex);
                List<object> logParts = new List<object>
                {
                    "\n",
                    ConsoleColor.DarkGray,
                    "Channel ",
                    ConsoleColor.White,
                    channel.Id,
                    ConsoleColor.DarkGray,
                    " Source=",
                    ConsoleColor.White,
                    X32Util.ConvertUserInIndexToString(uiIndex)
                };

                if (channel.OriginalHeadamp != null && haIndex > -1)
                {
                    logParts.AddRange(new object[]
                    {
                        ConsoleColor.DarkGray,
                        " Gain=",
                        ConsoleColor.White,
                        string.Format("{0:+0.0;-0.0}dB", X32Util.ConvertFloatToHeadampGain(channel.OriginalHeadamp.Gain)),
                        ConsoleColor.DarkGray,
                        " 48V=",
                        ConsoleColor.White,
                        channel.OriginalHeadamp.Phantom > 0 ? "On" : "Off"
                    });
                }

                X32ConsoleLogger.WriteLine(logParts.ToArray());

                var channelClient = _client.Root.Channel(channel.Id);

                Send("/config/userrout/in/" + channel.Id.ToString().PadLeft(2, '0'), uiIndex);
                channelClient.Config.SetName(channel.Name);

                if (!channel.On)
                {
                    channelClient.Config.SetColor(StripColor.Black);
                    channelClient.Group.SetMuteGroups(new MuteGroupSet());
                    channelClient.Mix.SetOn(false);
                    channelClient.Mix.SetFader(FaderFineLevel.MinValue);
                }

                foreach (var group in Config.Groups)
                {
                    if (group.Type == "Bus")
                    {
                        // all channels should be assigned to grouping buses with type GRP set and active only if channel belongs to group
                        channelClient.Mix.Send(group.Id).SetTapType(InputTap.Group);
                        channelClient.Mix.Send(group.Id).SetOn(channel.Group == group.Name);
                    }

                    if (channel.Group == group.Name)
                    {
                        bool st;
                        DcaGroupSet dca = new DcaGroupSet();

                        switch (group.Type)
                        {
                            case "Bus":
                            case "Off":
                                st = false; // channels assigned to grouping bus should not be in main mix
                                break;

                            case "DCA":
                                st = true; // channels assigned to DCA groups should be in the main mix
                                dca.Add(group.Id);
                                break;

                            default:
                                X32ConsoleLogger.WriteLine(ConsoleColor.Yellow, "Warning: Unknown group type \"{0}\"", group.Type);
                                goto case "";

                            case "":
                                st = true;
                                break;
                        }

                        channelClient.Mix.SetStereoSendOn(st);
                        channelClient.Group.SetDcaGroups(dca);

                        if (channel.On)
                        {
                            channelClient.Config.SetColor(group.Color);
                            channelClient.Group.SetMuteGroups(group.MuteGroups);
                        }
                    }
                }

                if (channel.OriginalHeadamp != null && haIndex > -1)
                {
                    Send("/headamp/" + haIndex.ToString().PadLeft(3, '0') + "/gain", channel.OriginalHeadamp.Gain);
                    Send("/headamp/" + haIndex.ToString().PadLeft(3, '0') + "/phantom", channel.OriginalHeadamp.Phantom);
                }
            }
        }

        private IList<ChannelState> ReadSheet()
        {
            UserCredential credential;

            X32ConsoleLogger.WriteLine(
                ConsoleColor.DarkGray, "Reading ",
                ConsoleColor.DarkGreen, "Google Spreadsheet ",
                ConsoleColor.White, Config.SpreadsheetId);
            X32ConsoleLogger.WriteLine(
                ConsoleColor.DarkGray, "Range ",
                ConsoleColor.White, Config.SpreadsheetRange);

            using (var stream = new FileStream(Config.CredentialsFilename, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new string[] { SheetsService.Scope.SpreadsheetsReadonly },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("token.json", true)).Result;
            }

            IList<IList<object>> values;

            using (var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "X32 Routing Sync",
            }))
            {
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(Config.SpreadsheetId, Config.SpreadsheetRange);

                ValueRange response = request.Execute();
                values = response.Values;
            }

            List<ChannelState> result = new List<ChannelState>();

            if (values != null && values.Count > 1)
            {
                IList<object> headerRow = values.First();

                int ColumnIndex(string header)
                {
                    int index;

                    if ((index = headerRow.IndexOf(header)) < 0)
                    {
                        throw new Exception("Missing column named " + header);
                    }

                    X32ConsoleLogger.WriteLine(
                        ConsoleColor.DarkGray, "Found ",
                        ConsoleColor.White, header,
                        ConsoleColor.DarkGray, " in column ",
                        ConsoleColor.White, (char)('A' + index));

                    return index;
                }

                int consoleIndex = ColumnIndex("Console");
                int channelIndex = ColumnIndex("Channel");
                int nameIndex = ColumnIndex("Name");
                int onIndex = ColumnIndex("On");
                int groupIndex = ColumnIndex("Group");
                int sourceIndex = ColumnIndex("Source");

                foreach (IList<object> row in values.Skip(1))
                {
                    if (row.Count > consoleIndex &&
                        row[consoleIndex].ToString() == Config.ConsoleName &&
                        row.Count > channelIndex)
                    {
                        string strId = row[channelIndex].ToString();

                        if (int.TryParse(strId, out int intId) && intId >= 1 && intId <= 32)
                        {
                            result.Add(new ChannelState()
                            {
                                Id = intId,
                                Name = row.Count > nameIndex ? row[nameIndex].ToString() : "",
                                On = row.Count > onIndex && (bool)row[onIndex].Equals("TRUE"),
                                Group = row.Count > groupIndex ? row[groupIndex].ToString() : "",
                                Source = row.Count > sourceIndex ? row[sourceIndex].ToString() : ""
                            });
                        }
                        else
                        {
                            X32ConsoleLogger.WriteLine(ConsoleColor.Yellow, "Warning: Unsupported channel string \"{0}\"", strId);
                        }
                    }
                }
            }

            return result;
        }

        private async Task<OscMessage> QueryAsync(string address, params object[] args)
        {
            var msg = _client.MessageFactory.Create(address, args);

            OscMessage response = await _client.QueryAsync(msg).ConfigureAwait(true);
            X32ConsoleLogger.WriteReceive(_client, response);

            return response;
        }

        private void Send(string address, params object[] args)
        {
            var msg = _client.MessageFactory.Create(address, args);

            if (Config.DryRun && args.Length > 0)
            {
                X32ConsoleLogger.Write("Would ");
                X32ConsoleLogger.WriteSend(_client, msg);

                return;
            }

            X32ConsoleLogger.WriteSend(_client, msg);
            _client.Send(msg);
        }

        private class ChannelState
        {
            public int Id;
            public string Name;
            public string Group;
            public bool On;
            public string Source;

            public HeadampState OriginalHeadamp = null;
        }

        private class HeadampState
        {
            public int Index;
            public float Gain;
            public int Phantom;
        }
    }
}
