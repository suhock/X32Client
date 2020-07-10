using Suhock.X32Stream;
using System;
using System.Threading.Tasks;

namespace Suhock.X32Show
{
    class Program
    {
        static async Task Main(string[] args)
        {
            X32Client clientDst = new X32Client("172.20.2.1", 10023);
            X32Client clientSrc = new X32Client("172.20.2.1", 10023);
            X32Message mixFader = null;

            _ = clientSrc.Subscribe((X32Message msg) =>
              {
                  if (msg.Address == "/main/st/mix/fader")
                  {
                      mixFader = msg;
                  }
              });

            for (int i = 0; i < 16; i++)
            {
                X32Message muteBus = new X32Message("/bus/" + (i + 1).ToString().PadLeft(2, '0') + "/mix/on", new X32Message.IX32Parameter[]
                {
                    new X32Message.X32IntParameter()
                    {
                        Value = 0
                    }
                });

                Console.WriteLine("Send: " + muteBus);
                clientDst.Send(muteBus);
            }

            for (int i = 0; i < 32; i += 2)
            {
                X32Message unlink = new X32Message("/config/chlink/" + (i + 1).ToString() + "-" + (i + 2).ToString(), new X32Message.IX32Parameter[]
                {
                    new X32Message.X32IntParameter()
                    {
                        Value = 0
                    }
                });

                Console.WriteLine("Send: " + unlink);
                clientDst.Send(unlink);
            }

            X32Message muteMain = new X32Message("/main/st/mix/on", new X32Message.IX32Parameter[]
            {
                new X32Message.X32IntParameter()
                {
                    Value = 0
                }
            });

            Console.WriteLine("Send: " + muteMain);
            clientDst.Send(muteMain);

            int interval = 33;
            float meter = 0.0f;
            int colorOffset = 0;
            int[] colors = new int[] {
                1, 3, 2, 6, 4, 5, 7
            };

            while (true)
            {
                float level = 0.0f;

                meter += (float)interval / 1000;
                meter -= (int)(meter / 2) * 2;

                colorOffset++;

                float width = (float)(Math.Cos(meter * Math.PI) + 1) / 2;
                int minMute = (int)(8.5 * (1 - width));
                int maxMute = 15 - minMute;

                if (mixFader != null)
                {
                    Console.WriteLine("Recv: " + mixFader);
                    level = ((X32Message.X32FloatParameter)mixFader.Parameters[0]).Value;
                    mixFader = null;

                    for (int i = 0; i < 32; i++)
                    {
                        X32Message chFader = new X32Message("/ch/" + (i + 1).ToString().PadLeft(2, '0') + "/mix/fader", new X32Message.IX32Parameter[]
                        {
                            new X32Message.X32FloatParameter()
                            {
                                Value = (float) (Math.Sin((i / 16.0 + level) * 2 * Math.PI) + 1) / 2
                            }
                        });

                        Console.WriteLine("Send: " + chFader);
                        clientDst.Send(chFader);
                    }
                }

                for (int i = 0; i < 32; i++)
                {
                    X32Message chMute = new X32Message("/ch/" + (i + 1).ToString().PadLeft(2, '0') + "/mix/on", new X32Message.IX32Parameter[]
                    {
                            new X32Message.X32IntParameter()
                            {
                                Value = (i % 16) >= minMute && (i % 16) <= maxMute ? 0 : 1
                            }
                    });

                    Console.WriteLine("Send: " + chMute);
                    clientDst.Send(chMute);

                    X32Message chColor = new X32Message("/ch/" + (i + 1).ToString().PadLeft(2, '0') + "/config/color", new X32Message.IX32Parameter[]
                    {
                            new X32Message.X32IntParameter()
                            {
                                Value = colors[(colorOffset * interval / 1000 + i) % colors.Length]
                            }
                    });

                    Console.WriteLine("Send: " + chColor);
                    clientDst.Send(chColor);
                }

                await Task.Delay(interval);
            }
        }
    }
}
