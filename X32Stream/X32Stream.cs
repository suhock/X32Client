using Suhock.Osc;
using Suhock.X32.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Suhock.X32.Stream
{
    public class X32Stream : IDisposable
    {
        private readonly X32StreamConfig Config;

        private readonly X32Client ClientDst;

        private readonly X32Client ClientSrc;

        public X32Stream(X32StreamConfig config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));

            ClientDst = new X32Client(config.Destination.Address, config.Destination.Port);
            ClientSrc = new X32Client(config.Source.Address, config.Source.Port);

            ClientSrc.MessageReceived += (client, msg) =>
            {
                foreach (string pattern in config.Patterns)
                {
                    if (Regex.IsMatch(msg.Address, pattern))
                    {
                        Send(ClientDst, msg);
                        break;
                    }
                }
            };
        }

        ~X32Stream()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool IsDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                ClientSrc.Dispose();
                ClientDst.Dispose();
            }

            IsDisposed = true;
        }

        public Task Run()
        {
            ClientSrc.Run();
            ClientDst.Run();
            Task subscribeLoop = ClientSrc.Subscribe();
            RunInit();

            return subscribeLoop;
        }

        private void RunInit()
        {
            foreach (string initString in Config.Init)
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
                                int low = int.Parse(seqLow);
                                int high = int.Parse(seqHigh);

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
                    Send(ClientSrc, ClientSrc.MessageFactory.Create(command.ToString()));
                }
            }
        }

        private static async void Send(X32Client client, OscMessage msg)
        {
            X32ConsoleLogger.WriteSend(client, msg);
            await client.SendAsync(msg).ConfigureAwait(false);
        }
    }
}
