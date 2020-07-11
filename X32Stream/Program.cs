using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
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

            Task mainLoop = clientSrc.Subscribe((X32Message msg) =>
            {
                foreach (string pattern in config.Patterns)
                {
                    if (Regex.IsMatch(msg.Address, pattern))
                    {
                        Send(clientDst, msg);
                        break;
                    }
                }
            });

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
                        case '(':
                            inList = true;
                            token = new StringBuilder();
                            tokens = new List<string>();
                            used = true;
                            break;

                        case '|':
                            if (inList)
                            {
                                tokens.Add(token.ToString());
                                token = new StringBuilder();
                                used = true;
                            }

                            break;

                        case ')':
                            if (inList)
                            {
                                tokens.Add(token.ToString());
                                List<StringBuilder> newList = new List<StringBuilder>(commands.Count() * tokens.Count());

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

                                bool pad = seqLow[0] == '0';
                                int low = Int32.Parse(seqLow);
                                int high = Int32.Parse(seqHigh);

                                List<StringBuilder> newList = new List<StringBuilder>(commands.Count() * (high - low + 1));

                                for (int i = low; i <= high; i++)
                                {
                                    string value = i.ToString();

                                    if (pad)
                                    {
                                        value = value.PadLeft(2, '0');
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
                        } else
                        {
                            if (commands.Count() == 0)
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
                    Send(clientSrc, new X32Message(command.ToString()));
                }
            }

            await mainLoop;
        }

        private static object ConsoleLock = new object();

        private static void Send(X32Client client, X32Message msg)
        {
            lock (ConsoleLock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Send to ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(client.Address);
                Console.ForegroundColor = ConsoleColor.DarkGray;
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
            }

            client.Send(msg);
        }
    }
}
