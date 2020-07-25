using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Suhock.Osc
{
    public class OscClient : IDisposable
    {
        private bool IsDisposed = false;

        public UdpClient Client { get; private set; }

        public OscClient(string address, int port) : this(new UdpClient(address, port))
        {
        }

        public OscClient(UdpClient client)
        {
            Client = client;
        }

        ~OscClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                Client.Dispose();
            }

            Client = null;
            IsDisposed = true;
        }

        public void Close()
        {
            Dispose(true);
        }

        public int Send(OscMessage msg)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            byte[] bytes = msg.GetBytes();
            return Client.Send(bytes, bytes.Length);
        }

        public Task<int> SendAsync(OscMessage msg)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            byte[] bytes = msg.GetBytes();
            return Task<int>.Factory.FromAsync(Client.SendAsync(bytes, bytes.Length), (result) => ((Task<int>)result).Result);
        }

        public IAsyncResult BeginSend(OscMessage msg, AsyncCallback requestCallback)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            byte[] bytes = msg.GetBytes();
            return Client.BeginSend(bytes, bytes.Length, requestCallback, msg);
        }

        public int EndSend(IAsyncResult asyncResult)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return Client.EndSend(asyncResult);
        }

        public OscMessage Receive()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            byte[] bytes = Client.Receive(ref ep);

            return new OscMessage(bytes);
        }

        public IAsyncResult BeginReceive(AsyncCallback requestCallback, object state)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return Client.BeginReceive(requestCallback, state);
        }

        public Task<OscMessage> ReceiveAsync()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return Task<OscMessage>.Factory.FromAsync(Client.ReceiveAsync(), (result) =>
                new OscMessage(((Task<UdpReceiveResult>)result).Result.Buffer));
        }
    }
}
