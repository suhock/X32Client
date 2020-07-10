using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Suhock.X32Stream
{
    class Program
    {
        const string DefaultConfigFilename = "x32stream.json";

        static async Task Main(string[] args)
        {
            string configFilename = args.Length > 0 ? args[0] : DefaultConfigFilename;
            X32StreamConfig config;

            using (FileStream fs = File.OpenRead(configFilename))
            {
                config = await JsonSerializer.DeserializeAsync<X32StreamConfig>(fs);
            }

            X32Client clientDst = new X32Client(config.Destination.Address, config.Destination.Port);
            X32Client clientSrc = new X32Client(config.Source.Address, config.Source.Port);

            await clientSrc.Subscribe((X32Message msg) =>
            {
                foreach (string pattern in config.Patterns)
                {
                    if (Regex.IsMatch(msg.Address, pattern))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("Forward to ");
                        Console.Write(clientDst.Address);
                        Console.Write(": ");

                        string[] strs = msg.ToString().Split(' ');

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(strs[0]);

                        if (strs.Length > 1)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write(" ");
                            Console.Write(strs[1]);

                            Console.ForegroundColor = ConsoleColor.White;

                            for (int i = 2; i < strs.Length; i++)
                            {
                                Console.Write(" ");
                                Console.Write(strs[i]);
                            }
                        }

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();

                        clientDst.Send(msg);

                        break;
                    }
                }
            });
        }
    }
}
