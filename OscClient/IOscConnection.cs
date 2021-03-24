using System.Threading.Tasks;

namespace Suhock.Osc
{
    public interface IOscConnection
    {
        public byte[] Receive();

        public Task<byte[]> ReceiveAsync();

        public void Send(byte[] data) => Send(data, data.Length);

        public void Send(byte[] data, int bytes);

        public Task SendAsync(byte[] data) => SendAsync(data, data.Length);

        public Task SendAsync(byte[] data, int bytes);
    }
}
