using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Suhock.X32.Client;
using Suhock.X32.Client.Message;
using Suhock.X32.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Suhock.X32.Routing
{
    class X32Routing
    {
        const string DefaultConfigFilename = "x32routing.json";

        public class X32RoutingConfig
        {
            public string Address { get; set; }
            public int Port { get; set; } = X32Client.DefaultPort;
            public string CredentialsFilename { get; set; } = "credentials.json";
            public string SpreadsheetId { get; set; }
            public string SpreadsheetRange { get; set; } = "Channels!A1:F";
            public string ConsoleName { get; set; }
            public bool DryRun { get; set; } = false;

            public class Group
            {
                public string Name { get; set; }
                public string Type { get; set; }
                public string Id { get; set; }
            }

            public List<Group> Groups { get; set; } = new List<Group>();
        }

        class ChannelInfo
        {
            public string Id;
            public string Name;
            public string Group;
            public string Source;
        }

        class Headamp
        {
            public int Index;
            public float Gain;
            public int Phantom;
        }

        public X32RoutingConfig Config { get; }

        public X32Routing(X32RoutingConfig config)
        {
            Config = config;
        }

        public void Run()
        {
            // list of all channels in spreadsheet
            IList<ChannelInfo> channels = ReadSheet();

            // mapping of channel id to original headamp configuration
            Dictionary<string, Headamp> headamps = new Dictionary<string, Headamp>(32);

            X32Client client = new X32Client(Config.Address, Config.Port)
            {
                OnConnect = (X32Client client) =>
                {
                    X32ConsoleLogger.WriteLine(ConsoleColor.White, "Connected to " + client.Address);
                    X32ConsoleLogger.WriteLine();
                },
                OnDisconnect = (X32Client client) =>
                {
                    X32ConsoleLogger.WriteLine(ConsoleColor.White, "\nDisconnected from " + client.Address);
                }
            };

            client.Connect();

            foreach (ChannelInfo channel in channels)
            {
                string channelId = channel.Id.PadLeft(2, '0');

                Send(client,
                    new X32Message("/-ha/" + channelId + "/index"),
                    (X32Client client, X32Message msg) =>
                        headamps[channelId] = new Headamp()
                        {
                            Index = ((X32IntParameter)msg.Parameters[0]).Value,
                            Gain = -1.0f,
                            Phantom = -1
                        });
            }

            foreach (var (_, headamp) in headamps)
            {
                if (headamp.Index > -1)
                {
                    Send(client,
                        new X32Message("/headamp/" + headamp.Index.ToString().PadLeft(3, '0') + "/gain"),
                        (X32Client client, X32Message msg) =>
                            headamp.Gain = ((X32FloatParameter)msg.Parameters[0]).Value);

                    Send(client,
                        new X32Message("/headamp/" + headamp.Index.ToString().PadLeft(3, '0') + "/phantom"),
                        (X32Client client, X32Message msg) =>
                            headamp.Phantom = ((X32IntParameter)msg.Parameters[0]).Value);
                }
            }

            foreach (var channel in channels)
            {
                int userInIndex = X32Util.ConvertStringToUserInIndex(channel.Source);
                int headampIndex = X32Util.ConvertUserInIndexToHeadampIndex(userInIndex);
                Headamp oldHeadamp = headamps[channel.Id];
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
                            X32Util.ConvertUserInIndexToString(userInIndex)
                        };

                if (oldHeadamp.Index >= 0 && headampIndex >= 0)
                {
                    logParts.AddRange(new object[]
                        {
                                    ConsoleColor.DarkGray,
                                    " Gain=",
                                    ConsoleColor.White,
                                    string.Format("{0:+0.0;-0.0}dB", X32Util.ConvertFloatToHeadampGain(oldHeadamp.Gain)),
                                    ConsoleColor.DarkGray,
                                    " 48V=",
                                    ConsoleColor.White,
                                    oldHeadamp.Phantom > 0 ? "On" : "Off"
                        });
                }

                X32ConsoleLogger.WriteLine(logParts.ToArray());

                string channelAddress = "/ch/" + channel.Id;

                Send(client, new X32Message("/config/userrout/in/" + channel.Id, userInIndex));
                Send(client, new X32Message(channelAddress + "/config/name", channel.Name));

                foreach (var group in Config.Groups)
                {
                    if (group.Type == "Bus")
                    {
                        // all channel should be assigned to grouping buses with type GRP set and active only if channel belongs to group
                        Send(client, new X32Message(channelAddress + "/mix/" + group.Id.PadLeft(2, '0') + "/type", 5));
                        Send(client, new X32Message(channelAddress + "/mix/" + group.Id.PadLeft(2, '0') + "/on", channel.Group == group.Name ? 1 : 0));
                    }


                    if (channel.Group == group.Name)
                    {
                        int st = 1;
                        int dca = 0;

                        switch (group.Type)
                        {
                            case "Bus":
                            case "Off":
                                // channels assigned to grouping bus should not be in main mix
                                st = 0;
                                dca = 0;
                                break;

                            case "DCA":
                                st = 1;
                                dca = 1 << (int.Parse(group.Id) - 1);
                                break;

                            case "":
                                break;

                            default:
                                X32ConsoleLogger.WriteLine(ConsoleColor.DarkYellow, "Warning: Unknown group type \"{0}\"", group.Type);
                                break;
                        }

                        Send(client, new X32Message(channelAddress + "/mix/st", st));
                        Send(client, new X32Message(channelAddress + "/grp/dca", dca));
                    }
                }

                if (oldHeadamp.Index >= 0 && headampIndex >= 0)
                {
                    Send(client, new X32Message("/headamp/" + headampIndex.ToString().PadLeft(3, '0') + "/gain", oldHeadamp.Gain));
                    Send(client, new X32Message("/headamp/" + headampIndex.ToString().PadLeft(3, '0') + "/phantom", oldHeadamp.Phantom));
                }
            }

            client.Disconnect();
        }

        private IList<ChannelInfo> ReadSheet()
        {
            UserCredential credential;

            X32ConsoleLogger.WriteLine("Reading Google Spreadsheet " + Config.SpreadsheetId + " " + Config.SpreadsheetRange);

            using (var stream = new FileStream(Config.CredentialsFilename, FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new string[] { SheetsService.Scope.SpreadsheetsReadonly },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "X32 Routing Sync",
            });

            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(Config.SpreadsheetId, Config.SpreadsheetRange);

            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;
            List<ChannelInfo> result = new List<ChannelInfo>();

            if (values != null && values.Count > 1)
            {
                int consoleIndex, channelIndex, nameIndex, groupIndex, sourceIndex;
                IList<object> headerRow = values.First();

                if ((consoleIndex = headerRow.IndexOf("Console")) < 0)
                {
                    throw new Exception("Missing column named Console");
                }

                if ((channelIndex = headerRow.IndexOf("Channel")) < 0)
                {
                    throw new Exception("Missing column named Channel");
                }

                if ((nameIndex = headerRow.IndexOf("Name")) < 0)
                {
                    throw new Exception("Missing column named Name");
                }

                if ((groupIndex = headerRow.IndexOf("Group")) < 0)
                {
                    throw new Exception("Missing column named Group");
                }

                if ((sourceIndex = headerRow.IndexOf("Source")) < 0)
                {
                    throw new Exception("Missing column named Source");
                }

                foreach (IList<object> row in values.Skip(1))
                {
                    if (row.Count > consoleIndex && row[consoleIndex].ToString() == Config.ConsoleName && row.Count > channelIndex)
                    {
                        string strId = row[channelIndex].ToString();

                        if (int.TryParse(strId, out int intId) && intId >= 1 && intId <= 32)
                        {
                            result.Add(new ChannelInfo()
                            {
                                Id = strId.PadLeft(2, '0'),
                                Name = row.Count > nameIndex ? row[nameIndex].ToString() : "",
                                Group = row.Count > groupIndex ? row[groupIndex].ToString() : "",
                                Source = row.Count > sourceIndex ? row[sourceIndex].ToString() : ""
                            });
                        }
                        else
                        {
                            X32ConsoleLogger.WriteLine(ConsoleColor.DarkYellow, "Unsupported channel ID \"{0}\"", strId);
                        }
                    }
                }
            }

            return result;
        }

        private void Send(X32Client client, X32Message msg, X32Client.MessageHandler handler)
        {
            bool dryRun = Config.DryRun && msg.Parameters.Count() > 0;

            if (dryRun)
            {
                X32ConsoleLogger.Write("Would ");
            }

            X32ConsoleLogger.WriteSend(client, msg);

            if (handler != null)
            {
                client.Send(msg, (X32Client client, X32Message msg) =>
                    {
                        X32ConsoleLogger.WriteReceive(client, msg);
                        handler?.Invoke(client, msg);
                    });
            } else
            {
                client.Send(msg);
            }
        }

        private void Send(X32Client client, X32Message msg)
        {
            Send(client, msg, null);
        }

        static async Task Main(string[] args)
        {
            string configFilename = args.Length > 0 ? args[0] : DefaultConfigFilename;
            X32RoutingConfig config;

            using (FileStream fs = File.OpenRead(configFilename))
            {
                config = await JsonSerializer.DeserializeAsync<X32RoutingConfig>(fs);
            }

            X32Routing x32Routing = new X32Routing(config);
            x32Routing.Run();
        }
    }
}
