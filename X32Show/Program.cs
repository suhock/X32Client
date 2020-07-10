using Suhock.X32Stream;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Suhock.X32Show
{
    class Program
    {
        static async Task Main(string[] args)
        {
            X32Client clientDst = new X32Client("192.168.1.30", 10023);
            X32Client clientSrc = new X32Client("192.168.1.30", 10023);
            X32Message mixFader = null;

            clientSrc.Subscribe((X32Message msg) =>
            {
                if (msg.Address == "/main/st/mix/fader")
                {
                    mixFader = msg;
                }
            });

            float meter = 0.0f;
            int colorOffset = 0;

            while (true)
            {
                float level = 0.0f;

                meter += 0.1f;

                if (meter >= 2.0f)
                {
                    meter = 0.0f;
                }

                colorOffset++;

                float width = (float)(Math.Cos(meter * Math.PI) + 1) / 2;
                int minMute = (int)(7 * (1 - width));
                int maxMute = 15 - minMute;

                if (mixFader != null)
                {
                    Console.WriteLine("Recv: " + mixFader);
                    level = ((X32Message.X32FloatParameter)mixFader.Parameters[0]).Value;
                    mixFader = null;

                    for (int i = 0; i < 16; i++)
                    {
                        X32Message chFader = new X32Message("/ch/" + (i + 1).ToString().PadLeft(2, '0') + "/mix/fader", new X32Message.IX32Parameter[]
                        {
                            new X32Message.X32FloatParameter()
                            {
                                Value = (float) (Math.Sin((i / 16.0 + level) * 2 * Math.PI) + 1) / 2
                            }
                        });

                        Console.WriteLine("Send: " + BytesToString(chFader.Encode()));
                        clientDst.Send(chFader);
                    }
                }

                for (int i = 0; i < 16; i++)
                {
                    X32Message chMute = new X32Message("/ch/" + (i + 1).ToString().PadLeft(2, '0') + "/mix/on", new X32Message.IX32Parameter[]
                    {
                            new X32Message.X32IntParameter()
                            {
                                Value = i >= minMute && i <= maxMute ? 0 : 1
                            }
                    });

                    Console.WriteLine("Send: " + BytesToString(chMute.Encode()));
                    clientDst.Send(chMute);

                    X32Message chColor = new X32Message("/ch/" + (i + 1).ToString().PadLeft(2, '0') + "/config/color", new X32Message.IX32Parameter[]
                    {
                            new X32Message.X32IntParameter()
                            {
                                Value = (colorOffset + i) % 16
                            }
                    });

                    Console.WriteLine("Send: " + BytesToString(chColor.Encode()));
                    clientDst.Send(chColor);
                }

                await Task.Delay(100);
            }

            Console.ReadKey();
        }

        static string BytesToString(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0)
                {
                    bytes[i] = (byte)'~';
                }
            }

            return Encoding.ASCII.GetString(bytes);
        }
    }
}
