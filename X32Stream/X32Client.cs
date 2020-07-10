using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Suhock.X32Stream
{
    public class X32Client
    {
        private readonly int port;
        private readonly string address;

        private UdpClient Client;
        private IPEndPoint EndPoint;

        public delegate void HandleMessage(X32Message message);

        public X32Client(string address, int port)
        {
            this.address = address;
            this.port = port;
        }

        private void Reset()
        {
            if (Client == null)
            {
                Client = new UdpClient();
            }

            if (EndPoint == null)
            {
                EndPoint = new IPEndPoint(IPAddress.Parse(address), port);
            }
        }

        public async void Subscribe(HandleMessage callback)
        {
            await Task.Run(() =>
            {
                Reset();
                XInfoMessageLoop();

                while (Client != null)
                {
                    try
                    {
                        byte[] buffer = Client.Receive(ref EndPoint);
                        callback(X32Message.Decode(buffer));
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Client.Close();
                        Client = null;
                    }
                }
            });
        }

        public async void Send(X32Message msg)
        {
            byte[] encodedMsg = msg.Encode();
            Reset();

            try
            {
                await Client.SendAsync(encodedMsg, encodedMsg.Length, EndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async void XInfoMessageLoop()
        {
            byte[] xinfo = new X32Message("/xremote").Encode();

            while (Client != null)
            {
                try
                {
                    await Client.SendAsync(xinfo, xinfo.Length, EndPoint);
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Client.Close();
                    Client = null;
                }

                await Task.Delay(2000);
            }
        }
    }
}
