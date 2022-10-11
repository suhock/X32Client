using Suhock.Osc;
using System;
using System.Collections.Generic;

namespace Suhock.X32.Util;

public static class X32ConsoleLogger
{
    private static readonly object _lock = new();

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
        lock (_lock)
        {
            var originalColor = Console.ForegroundColor;
            var startIndex = 0;
            var currentIndex = 0;

            void EmptyQueue()
            {
                if (currentIndex > startIndex)
                {
                    var args = new object[currentIndex - startIndex - 1];
                    Array.Copy(parts, startIndex + 1, args, 0, currentIndex - startIndex - 1);
                    Console.Write((string)parts[startIndex], args);
                }

                startIndex = currentIndex + 1;
            }

            for (currentIndex = 0; currentIndex < parts.Length; currentIndex++)
            {
                if (parts[currentIndex].GetType() == typeof(ConsoleColor))
                {
                    EmptyQueue();
                    Console.ForegroundColor = (ConsoleColor)parts[currentIndex];
                }
                else if (currentIndex == startIndex && parts[currentIndex].GetType() != typeof(string))
                {
                    Console.Write(parts[startIndex]);
                    startIndex = currentIndex + 1;
                }
            }

            EmptyQueue();

            if (newLine)
            {
                Console.WriteLine();
            }

            Console.ForegroundColor = originalColor;
        }
    }

    public static void WriteSend(IX32Client client, OscMessage msg)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        if (msg == null)
        {
            throw new ArgumentNullException(nameof(msg));
        }

        var parts = new List<object>
        {
            ConsoleColor.DarkGray,
            "Send",
            //ConsoleColor.Gray,
            //client.Address,
            ConsoleColor.DarkGray,
            ": "
        };

        WriteMessage(parts, msg, ConsoleColor.Red);
        WriteParts(parts.ToArray(), true);
    }

    public static void WriteReceive(X32Client client, OscMessage msg)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        if (msg == null)
        {
            throw new ArgumentNullException(nameof(msg));
        }

        var parts = new List<object>
        {
            ConsoleColor.DarkGray,
            "Recv ",
            ConsoleColor.DarkGray,
            ": "
        };

        WriteMessage(parts, msg, ConsoleColor.White);
        WriteParts(parts.ToArray(), true);
    }

    private static void WriteMessage(List<object> parts, OscMessage msg, ConsoleColor addressColor)
    {
        parts.Add(addressColor);
        parts.Add(msg.Address);
        parts.Add(ConsoleColor.DarkGray);
        parts.Add(' ' + msg.GetTypeTagString());
        parts.Add(ConsoleColor.White);

        foreach (var arg in msg.Arguments)
        {
            parts.Add(' ' + arg.ToString());
        }
    }
}