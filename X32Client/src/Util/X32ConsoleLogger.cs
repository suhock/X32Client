using Suhock.X32.Client;
using Suhock.X32.Client.Message;
using System;
using System.Collections.Generic;

namespace Suhock.X32.Util
{
    public class X32ConsoleLogger
    {
        private static readonly object Lock = new object();

        public static void Write(params object[] parts)
        {
            WriteParts(parts, false);
        }

        public static void WriteLine(params object[] parts)
        {
            WriteParts(parts, true);
        }

        private static void WriteParts(object[] parts, bool newLine)
        {
            lock (Lock)
            {
                ConsoleColor originalColor = Console.ForegroundColor;
                int startIndex = 0;
                int currentIndex = 0;

                void emptyQueue()
                {
                    if (currentIndex > startIndex)
                    {
                        object[] args = new object[currentIndex - startIndex - 1];
                        Array.Copy(parts, startIndex + 1, args, 0, currentIndex - startIndex - 1);
                        Console.Write((string)parts[startIndex], args);
                    }

                    startIndex = currentIndex + 1;
                }

                for (currentIndex = 0; currentIndex < parts.Length; currentIndex++)
                {
                    if (parts[currentIndex].GetType() == typeof(ConsoleColor))
                    {
                        emptyQueue();
                        Console.ForegroundColor = (ConsoleColor)parts[currentIndex];
                    }
                    else if (currentIndex == startIndex && parts[currentIndex].GetType() != typeof(string))
                    {
                        Console.Write(parts[startIndex]);
                        startIndex = currentIndex + 1;
                    }
                }

                emptyQueue();

                if (newLine)
                {
                    Console.WriteLine();
                }

                Console.ForegroundColor = originalColor;
            }
        }

        public static void WriteSend(X32Client client, X32Message msg)
        {
            List<object> parts = new List<object>
            {
                ConsoleColor.DarkGray,
                "Send to ",
                ConsoleColor.White,
                client.Address,
                ConsoleColor.DarkGray,
                ": "
            };

            WriteMessage(parts, msg, ConsoleColor.Red);

            WriteParts(parts.ToArray(), true);
        }

        public static void WriteReceive(X32Client client, X32Message msg)
        {
            List<object> parts = new List<object>
                {
                    ConsoleColor.DarkGray,
                    "Recv from ",
                    ConsoleColor.White,
                    client.Address,
                    ConsoleColor.DarkGray,
                    ": "
                };

            WriteMessage(parts, msg, ConsoleColor.Green);

            WriteParts(parts.ToArray(), true);
        }

        private static void WriteMessage(List<object> parts, X32Message msg, ConsoleColor addressColor)
        {
            string[] strs = msg.ToString().Split(' ');

            parts.Add(addressColor);
            parts.Add(strs[0]);

            if (strs.Length > 1)
            {
                parts.Add(ConsoleColor.DarkGray);
                parts.Add(' ' + strs[1]);
                parts.Add(ConsoleColor.White);
                parts.Add(' ' + string.Join(' ', strs, 2, strs.Length - 2));
            }
        }
    }
}
