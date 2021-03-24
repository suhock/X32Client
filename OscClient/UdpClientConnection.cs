using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Suhock.Osc
{
    public class UdpClientConnection : IOscConnection
    {
        public UdpClient Client { get; }

        public UdpClientConnection(UdpClient client)
        {
            Client = client;
        }

        public byte[] Receive()
        {
            IPEndPoint ep = null;

            return Client.Receive(ref ep);
        }

        public Task<byte[]> ReceiveAsync()
        {
            return Task.Factory.FromAsync(Client.ReceiveAsync(),
                (result) => ((Task<UdpReceiveResult>)result).Result.Buffer);
        }

        public void Send(byte[] data, int bytes)
        {
            Client.Send(data, bytes);
        }

        public Task SendAsync(byte[] data, int bytes)
        {
            return Client.SendAsync(data, bytes);
        }
    }
}
