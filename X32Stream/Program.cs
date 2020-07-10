using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Suhock.X32Stream
{
    class Program
    {
        static void Main(string[] args)
        {
            X32Client clientDst = new X32Client("192.168.1.30", 10023);
            X32Client clientSrc = new X32Client("192.168.1.30", 10023);

            clientSrc.Subscribe((X32Message msg) =>
            {
                Console.WriteLine("Recv: " + msg);

                if (msg.Address.StartsWith("/ch/"))
                {
                    int ch = System.Int32.Parse(msg.Address.Substring(4, 2)) + 1;

                    if (ch <= 32)
                    {
                        string newCh = ch.ToString().PadLeft(2, '0');
                        char[] address = msg.Address.ToCharArray();
                        address[4] = newCh[0];
                        address[5] = newCh[1];

                        msg.Address = new string(address);
                        Console.WriteLine("Send: " + BytesToString(msg.Encode()));
                        clientDst.Send(msg);
                    }
                }
                else if (Regex.IsMatch(msg.Address, @"^/((auxin|ch)/\d\d/(config|dyn|eq|gate|grp/dca|mix/(fader|on)|preamp/(hp|invert)))|dca"))
                {
                    Console.WriteLine("Send: " + BytesToString(msg.Encode()));
                }
            });

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
