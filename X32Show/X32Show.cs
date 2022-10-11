using Suhock.Osc;
using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;
using Suhock.X32.Util;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Suhock.X32.Show
{
    public sealed class X32Show : IDisposable
    {
        private readonly X32Client _client;
        private readonly OscMessageFactory _messageFactory;


        public X32Show(X32ShowConfig config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            _client = new X32Client(Config.Address, Config.Port);
            _messageFactory = new OscMessageFactory();
        }

        public X32ShowConfig Config { get; }

        public void Dispose()
        {
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
            _ = _client.Subscribe(CancellationToken.None);

            for (var i = 0; i < 16; i++)
            {
                await SendAsync("/bus/" + (i + 1).ToString().PadLeft(2, '0') + "/mix/on", 0).ConfigureAwait(false);
            }

            for (var i = 0; i < 32; i += 2)
            {
                await SendAsync($"/config/chlink/{(i + 1)}-{(i + 2)}", 0).ConfigureAwait(false);
            }

            await _client.Root.Main.Stereo.Mix.On(false).ConfigureAwait(false);
            await _client.Root.Dca(1).Fader(FaderFineLevel.MinValue).ConfigureAwait(false);
            await _client.Root.Dca(2).Fader(FaderFineLevel.MinValue).ConfigureAwait(false);

            var interval = 50;
            var meter = 0.0f;
            var colorOffset = 0;
            var colors = new[]
            {
                StripColor.Red,
                StripColor.Yellow,
                StripColor.Green,
                StripColor.Cyan,
                StripColor.Blue,
                StripColor.Magenta,
                StripColor.White
            };

            var levels = new float[16];

            while (true)
            {
                if (freqFader != null && ampFader != null)
                {
                    meter += 0.05f + freqFader.GetArgumentValue<float>(0) * 0.1f;

                    colorOffset++;

                    var width = (float)((Math.Cos(meter * Math.PI) + 1) / 2);
                    var minMute = (int)(8.5 * (1 - width));
                    var maxMute = 15 - minMute;

                    for (var i = 15; i >= 1; i--)
                    {
                        levels[i] = levels[i - 1];
                    }

                    levels[0] = (float)(ampFader.GetArgumentValue<float>(0) * Math.Sin(meter * 2 * Math.PI) + 1) / 2;

                    for (var i = 0; i < 32; i++)
                    {
                        await _client.Root.Channel(i + 1).Mix.Fader(FaderFineLevel.FromEncodedValue(levels[i % 16]))
                            .ConfigureAwait(false);
                    }

                    for (var i = 0; i < 32; i++)
                    {
                        await _client.Root.Channel(i + 1).Mix.On((i % 16) >= minMute && (i % 16) <= maxMute)
                            .ConfigureAwait(false);
                        await _client.Root.Channel(i + 1).Config
                            .Color(colors[(colorOffset * interval / 1000 + i) % colors.Length]).ConfigureAwait(false);
                    }
                }

                await Task.Delay(50).ConfigureAwait(true);
            }
        }

        private async Task SendAsync(string address, params object[] values)
        {
            //X32ConsoleLogger.WriteSend(_client, msg);
            await _client.SendAsync(_messageFactory.Create(address, values)).ConfigureAwait(false);
        }
    }
}