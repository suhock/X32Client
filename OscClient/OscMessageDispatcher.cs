using System;
using System.Threading.Tasks;

namespace Suhock.Osc
{
    public class OscMessageDispatcher
    {
        private Task _task = null;
        
        public OscMessageDispatcher(OscClient client)
        {
            Client = client;
        }

        public event EventHandler<OscMessage> MessageSent;

        public event EventHandler<OscMessage> MessageReceived;

        public OscClient Client { get; }

        public bool IsRunning { get => _task != null && !_task.IsCompleted; }

        public void Run()
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("Already running");
            }

            _task = new Task(() =>
            {
                using (_task)
                {
                    while (true)
                    {
                        OscMessage msg = Client.Receive();
                        MessageReceived?.Invoke(this, msg);
                    }
                }
            }, TaskCreationOptions.LongRunning);

            _task.Start();
        }

        public void Send(OscMessage msg)
        {
            if (!IsRunning)
            {
                throw new InvalidOperationException("Not running");
            }

            MessageSent?.Invoke(this, msg);
            Client.Send(msg);
        }

        public Task SendAsync(OscMessage msg)
        {
            if (!IsRunning)
            {
                throw new InvalidOperationException("Not running");
            }

            MessageSent?.Invoke(this, msg);
            return Client.SendAsync(msg);
        }
    }
}
