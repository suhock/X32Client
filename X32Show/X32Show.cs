using Suhock.X32.Client;
using Suhock.X32.Client.Message;
using Suhock.X32.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Suhock.X32.Show
{
    class X32Show
    {
        const string DefaultConfigFilename = "x32show.json";

        public class X32ShowConfig
        {
            public string Address { get; set; }
            public int Port { get; set; } = X32Client.DefaultPort;
        }

        private static X32ShowConfig config;

        static async Task Main(string[] args)
        {
            string configFilename = args.Length > 0 ? args[0] : DefaultConfigFilename;

            using (FileStream fs = File.OpenRead(configFilename))
            {
                config = await JsonSerializer.DeserializeAsync<X32ShowConfig>(fs);
            }

            X32Message freqFader = null;
            X32Message ampFader = null;

            X32Client client = new X32Client(config.Address, config.Port)
            {
                OnMessage = (X32Client client, X32Message msg) =>
                {
                    if (msg.Address == "/dca/1/fader")
                    {
                        freqFader = msg;
                        _ = X32ConsoleLogger.WriteReceive(client, msg);
                    }
                    else if (msg.Address == "/dca/2/fader")
                    {
                        ampFader = msg;
                        _ = X32ConsoleLogger.WriteReceive(client, msg);
                    }
                }
            };

            _ = client.Connect();
            _ = client.Subscribe();

            for (int i = 0; i < 16; i++)
            {
                Send(client, new X32Message("/bus/" + (i + 1).ToString().PadLeft(2, '0') + "/mix/on", 0));
            }

            for (int i = 0; i < 32; i += 2)
            {
                Send(client, new X32Message("/config/chlink/" + (i + 1).ToString() + "-" + (i + 2).ToString(), 0));
            }

            Send(client, new X32Message("/main/st/mix/on", 0));
            Send(client, new X32Message("/dca/1/fader"));
            Send(client, new X32Message("/dca/2/fader"));

            int interval = 50;
            float meter = 0.0f;
            int colorOffset = 0;
            int[] colors = new int[] {
                1, 3, 2, 6, 4, 5, 7
            };

            float[] levels = new float[16];

            while (true)
            {
                List<Task> tasks = new List<Task>(3 * 32);

                if (freqFader != null && ampFader != null)
                {
                    meter += 0.05f + ((X32FloatParameter)freqFader.Parameters[0]).Value * 0.1f;

                    colorOffset++;

                    float width = (float)((Math.Cos(meter * Math.PI) + 1) / 2);
                    int minMute = (int)(8.5 * (1 - width));
                    int maxMute = 15 - minMute;

                    for (int i = 15; i >= 1; i--)
                    {
                        levels[i] = levels[i - 1];
                    }

                    levels[0] = (float)(((X32FloatParameter)ampFader.Parameters[0]).Value * Math.Sin(meter * 2 * Math.PI) + 1) / 2;

                    for (int i = 0; i < 32; i++)
                    {
                        Send(client, new X32Message("/ch/" + (i + 1).ToString().PadLeft(2, '0') + "/mix/fader", levels[i % 16]));
                    }

                    for (int i = 0; i < 32; i++)
                    {
                        Send(client, new X32Message("/ch/" + (i + 1).ToString().PadLeft(2, '0') + "/mix/on", (i % 16) >= minMute && (i % 16) <= maxMute ? 0 : 1));
                        Send(client, new X32Message("/ch/" + (i + 1).ToString().PadLeft(2, '0') + "/config/color", colors[(colorOffset * interval / 1000 + i) % colors.Length]));
                    }
                }

                await Task.Delay(50);
            }
        }

        private static async void Send(X32Client client, X32Message msg)
        {
            //_ = X32ConsoleLogger.WriteSend(client, msg);
            await client.Send(msg);
        }
    }
}
