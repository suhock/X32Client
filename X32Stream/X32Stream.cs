using Suhock.Osc;
using Suhock.X32.Client;
using Suhock.X32.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Suhock.X32.Stream
{
    internal class X32Stream
    {
        private const string DefaultConfigFilename = "x32stream.json";

        private static X32StreamConfig config;

        private static X32Client clientDst;

        private static X32Client clientSrc;

        private static async Task Main(string[] args)
        {
            string configFilename = args.Length > 0 ? args[0] : DefaultConfigFilename;

            using (FileStream fs = File.OpenRead(configFilename))
            {
                config = await JsonSerializer.DeserializeAsync<X32StreamConfig>(fs);
            }

            clientDst = new X32Client(config.Destination.Address, config.Destination.Port)
            {
                OnConnect = (X32Client client) =>
                {
                    Console.WriteLine("Connected to " + client.Address);
                },
                OnDisconnect = (X32Client client) =>
                {
                    Console.WriteLine("Disconnected from " + client.Address);
                }
            };

            clientSrc = new X32Client(config.Source.Address, config.Source.Port)
            {
                OnConnect = async (X32Client client) =>
                {
                    Console.WriteLine("Connected to " + client.Address);
                    _ = RunInit();
                    await client.Subscribe().ConfigureAwait(false);
                },
                OnDisconnect = (X32Client client) =>
                {
                    Console.WriteLine("Disconnected from " + client.Address);
                },
                OnMessage = (X32Client client, OscMessage msg) =>
                {
                    foreach (string pattern in config.Patterns)
                    {
                        if (Regex.IsMatch(msg.Address, pattern))
                        {
                            Send(clientDst, msg);
                            break;
                        }
                    }
                }
            };

            await Task.WhenAny(clientSrc.Connect(), clientDst.Connect()).ConfigureAwait(true);
        }

        private static Task RunInit()
        {
            return Task.Run(() =>
            {
                foreach (string initString in config.Init)
                {
                    bool inList = false;
                    bool inSeq = false;
                    StringBuilder token = null;
                    List<StringBuilder> commands = new List<StringBuilder>();
                    List<string> tokens = null;
                    string seqLow = null;
                    string seqHigh = null;

                    foreach (char c in initString)
                    {
                        bool used = false;

                        switch (c)
                        {
                            case '{':
                                inList = true;
                                token = new StringBuilder();
                                tokens = new List<string>();
                                used = true;
                                break;

                            case ',':
                                if (inList)
                                {
                                    tokens.Add(token.ToString());
                                    token = new StringBuilder();
                                    used = true;
                                }

                                break;

                            case '}':
                                if (inList)
                                {
                                    tokens.Add(token.ToString());
                                    List<StringBuilder> newList = new List<StringBuilder>(commands.Count * tokens.Count);

                                    foreach (StringBuilder command in commands)
                                    {
                                        foreach (string t in tokens)
                                        {
                                            newList.Add(new StringBuilder(command.ToString()).Append(t));
                                        }
                                    }

                                    commands = newList;
                                    inList = false;
                                    token = null;
                                    used = true;
                                }

                                break;

                            case '[':
                                inSeq = true;
                                token = new StringBuilder();
                                used = true;
                                break;

                            case ']':
                                if (inSeq)
                                {
                                    seqHigh = token.ToString();
                                    inSeq = false;
                                    token = null;
                                    used = true;

                                    int paddedLength = seqLow.Length;
                                    int low = Int32.Parse(seqLow);
                                    int high = Int32.Parse(seqHigh);

                                    List<StringBuilder> newList = new List<StringBuilder>(commands.Count * (high - low + 1));

                                    for (int i = low; i <= high; i++)
                                    {
                                        string value = i.ToString();

                                        if (seqLow.Length > value.Length)
                                        {
                                            value = value.PadLeft(seqLow.Length, '0');
                                        }

                                        foreach (StringBuilder command in commands)
                                        {
                                            newList.Add(new StringBuilder(command.ToString()).Append(value));
                                        }
                                    }

                                    commands = newList;
                                    seqLow = null;
                                    seqHigh = null;
                                }

                                break;

                            case '-':
                                if (inSeq)
                                {
                                    seqLow = token.ToString();
                                    token = new StringBuilder();
                                    used = true;
                                    break;
                                }

                                break;
                        }

                        if (!used)
                        {
                            if (token != null)
                            {
                                token.Append(c);
                            }
                            else
                            {
                                if (commands.Count == 0)
                                {
                                    commands.Add(new StringBuilder());
                                }

                                foreach (StringBuilder command in commands)
                                {
                                    command.Append(c);
                                }
                            }
                        }
                    }

                    foreach (StringBuilder command in commands)
                    {
                        Send(clientSrc, new OscMessage(command.ToString()));
                    }
                }
            });
        }

        private static void Send(X32Client client, OscMessage msg)
        {
            X32ConsoleLogger.WriteSend(client, msg);
            client.Send(msg);
        }
    }
}
