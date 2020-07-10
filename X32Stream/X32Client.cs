using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Suhock.X32Stream
{
    public class X32Client
    {
        public int Port { get; }
        public string Address { get; }

        private UdpClient Client;
        private IPEndPoint EndPoint;

        public delegate void HandleMessage(X32Message message);

        public X32Client(string address, int port)
        {
            Address = address;
            Port = port;
        }

        private void Reset()
        {
            if (Client == null)
            {
                Client = new UdpClient();
                EndPoint = new IPEndPoint(IPAddress.Parse(Address), Port);
            }
        }

        public async Task<bool> Subscribe(HandleMessage callback)
        {
            return await Task.Run(async () =>
            {
                Reset();
                XInfoMessageLoop();

                while (true)
                {
                    if (Client != null)
                    {
                        try
                        {
                            byte[] buffer = Client.Receive(ref EndPoint);

                            try
                            {
                                callback(X32Message.Decode(buffer));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            await Task.Delay(2000);
                        }
                    }
                }

                return true;
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

            while (true)
            {
                try
                {
                    await Client.SendAsync(xinfo, xinfo.Length, EndPoint);
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine(e);
                    Client = null;
                    Reset();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                await Task.Delay(2000);
            }
        }
    }
}
