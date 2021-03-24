using System.Net.Sockets;
using System.Threading.Tasks;

namespace Suhock.Osc
{
    public class TcpClientConnection : IOscConnection
    {
        public TcpClient Client { get; }

        public TcpClientConnection(TcpClient client)
        {
            Client = client;
        }

        public byte[] Receive()
        {
            NetworkStream stream = Client.GetStream();
            byte[] sizeBytes = new byte[4];

            if (stream.Read(sizeBytes, 0, 4) < 4)
            {
                throw new System.Exception();
            }

            int packetLength = OscUtil.ReadInt(sizeBytes, out _);
            int bytesRead = 0;
            byte[] buffer = new byte[packetLength];

            while (bytesRead < packetLength)
            {
                bytesRead += stream.Read(buffer, bytesRead, packetLength - bytesRead);
            }

            return buffer;
        }

        public async Task<byte[]> ReceiveAsync()
        {
            NetworkStream stream = Client.GetStream();
            byte[] sizeBytes = new byte[4];

            if (await stream.ReadAsync(sizeBytes, 0, 4) < 4)
            {
                throw new System.Exception();
            }

            int packetLength = OscUtil.ReadInt(sizeBytes, out _);
            int bytesRead = 0;
            byte[] buffer = new byte[packetLength];

            while (bytesRead < packetLength)
            {
                bytesRead += await stream.ReadAsync(buffer, bytesRead, packetLength - bytesRead);
            }

            return buffer;
        }

        public void Send(byte[] data, int bytes)
        {
            Client.GetStream().Write(data, 0, bytes);
        }

        public Task SendAsync(byte[] data, int bytes)
        {
            return Client.GetStream().WriteAsync(data, 0, bytes);
        }
    }
}
