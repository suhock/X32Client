using Suhock.Osc;
using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;
using Suhock.X32.Util;
using System;
using System.Threading.Tasks;

namespace Suhock.X32.Show
{
    public sealed class X32Show : IDisposable
    {
        private readonly X32Client _client;


        public X32Show(X32ShowConfig config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            _client = new X32Client(Config.Address, Config.Port);
        }

        public X32ShowConfig Config { get; }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task Run()
        {
            OscMessage freqFader = null;
            OscMessage ampFader = null;

            _client.MessageReceived += (_, msg) =>
            {
                if (msg.Address == "/dca/1/fader")
                {
                    freqFader = msg;
                    X32ConsoleLogger.WriteReceive(_client, msg);
                }
                else if (msg.Address == "/dca/2/fader")
                {
                    ampFader = msg;
                    X32ConsoleLogger.WriteReceive(_client, msg);
                }
            };

            _client.Run();
            _ = _client.Subscribe();

            for (int i = 0; i < 16; i++)
            {
                Send(_client.MessageFactory.Create("/bus/" + (i + 1).ToString().PadLeft(2, '0') + "/mix/on", 0));
            }

            for (int i = 0; i < 32; i += 2)
            {
                Send(_client.MessageFactory.Create("/config/chlink/" + (i + 1).ToString() + "-" + (i + 2).ToString(), 0));
            }

            Send(_client.MessageFactory.Create("/main/st/mix/on", 0));
            Send(_client.MessageFactory.Create("/dca/1/fader"));
            Send(_client.MessageFactory.Create("/dca/2/fader"));

            int interval = 50;
            float meter = 0.0f;
            int colorOffset = 0;
            StripColor[] colors = new StripColor[] {
                StripColor.Red,
                StripColor.Yellow,
                StripColor.Green,
                StripColor.Cyan,
                StripColor.Blue,
                StripColor.Magenta,
                StripColor.White
            };

            float[] levels = new float[16];

            while (true)
            {
                if (freqFader != null && ampFader != null)
                {
                    meter += 0.05f + freqFader.GetValue<float>(0) * 0.1f;

                    colorOffset++;

                    float width = (float)((Math.Cos(meter * Math.PI) + 1) / 2);
                    int minMute = (int)(8.5 * (1 - width));
                    int maxMute = 15 - minMute;

                    for (int i = 15; i >= 1; i--)
                    {
                        levels[i] = levels[i - 1];
                    }

                    levels[0] = (float)(ampFader.GetValue<float>(0) * Math.Sin(meter * 2 * Math.PI) + 1) / 2;

                    for (int i = 0; i < 32; i++)
                    {
                        _client.Root.Channel(i + 1).Mix.SetFader(FaderFineLevel.FromEncodedValue(levels[i % 16]));
                    }

                    for (int i = 0; i < 32; i++)
                    {
                        _client.Root.Channel(i + 1).Mix.SetOn((i % 16) >= minMute && (i % 16) <= maxMute);
                        _client.Root.Channel(i + 1).Config.SetColor(colors[(colorOffset * interval / 1000 + i) % colors.Length]);
                    }
                }

                await Task.Delay(50).ConfigureAwait(true);
            }
        }

        private void Send(OscMessage msg)
        {
            //X32ConsoleLogger.WriteSend(_client, msg);
            _client.Send(msg);
        }
    }
}
