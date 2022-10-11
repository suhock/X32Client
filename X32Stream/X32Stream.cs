using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Suhock.Osc;

namespace Suhock.X32.Stream
{
    public sealed class X32Stream : IDisposable
    {
        private readonly X32StreamConfig _config;

        private readonly IX32Client _clientDst;

        private readonly IX32Client _clientSrc;

        private readonly IOscMessageFactory _messageFactory;

        public X32Stream(X32StreamConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _messageFactory = new OscMessageFactory();
            _clientDst = new X32Client(config.Destination.Address, config.Destination.Port);
            _clientSrc = new X32Client(config.Source.Address, config.Source.Port);

            _clientSrc.MessageReceived += (_, msg) =>
            {
                if (config.Patterns.Any(pattern => Regex.IsMatch(msg.Address, pattern)))
                {
                    Send(_clientDst, msg.Address, msg.Arguments);
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

        private bool _isDisposed;

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                //_clientSrc.Dispose();
                //_clientDst.Dispose();
            }

            _isDisposed = true;
        }

        public Task Run()
        {
            _clientSrc.Run();
            _clientDst.Run();
            var subscribeLoop = _clientSrc.Subscribe();
            RunInit();

            return subscribeLoop;
        }

        private void RunInit()
        {
            foreach (var initString in _config.Init)
            {
                ParseInitString(initString);
            }
        }

        private void ParseInitString(string initString)
        {
            var inList = false;
            var inSeq = false;
            StringBuilder token = null;
            var commands = new List<StringBuilder>();
            List<string> tokens = null;
            string seqLow = null;

            foreach (var c in initString)
            {
                var used = false;

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
                            var newList = new List<StringBuilder>(commands.Count * tokens.Count);

                            foreach (var command in commands)
                            {
                                foreach (var t in tokens)
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
                            var seqHigh = token.ToString();
                            inSeq = false;
                            token = null;
                            used = true;

                            var low = int.Parse(seqLow);
                            var high = int.Parse(seqHigh);

                            var newList = new List<StringBuilder>(commands.Count * (high - low + 1));

                            for (var i = low; i <= high; i++)
                            {
                                var value = i.ToString();

                                if (seqLow.Length > value.Length)
                                {
                                    value = value.PadLeft(seqLow.Length, '0');
                                }

                                newList.AddRange(
                                    commands.Select(command =>
                                            new StringBuilder(command.ToString()).Append(value)));
                            }

                            commands = newList;
                            seqLow = null;
                        }

                        break;

                    case '-':
                        if (inSeq)
                        {
                            seqLow = token.ToString();
                            token = new StringBuilder();
                            used = true;
                        }

                        break;
                }

                if (used)
                {
                    continue;
                }
                
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

                    foreach (var command in commands)
                    {
                        command.Append(c);
                    }
                }
            }

            foreach (var command in commands)
            {
                Send(_clientSrc, command.ToString());
            }
        }

        private async void Send(IX32Client client, string address, params object[] values)
        {
            //X32ConsoleLogger.WriteSend(client, msg);
            await client.SendAsync(_messageFactory.Create(address, values)).ConfigureAwait(false);
        }
    }
}
