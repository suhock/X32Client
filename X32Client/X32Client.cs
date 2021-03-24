using Suhock.Osc;
using Suhock.X32.Nodes;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Suhock.X32
{
    public class X32Client : IDisposable
    {
        public const int DefaultPort = 10023;

        private readonly UdpClient _udpClient;

        private readonly OscClient _client;

        private readonly OscMessageDispatcher _dispatcher;

        private readonly ConcurrentDictionary<string, ConcurrentQueue<MessageResultHandle>> _waitQueues =
            new ConcurrentDictionary<string, ConcurrentQueue<MessageResultHandle>>();

        public X32Client(string address, int port)
        {
            MessageFactory = new X32MessageFactory();
            Address = address;
            Port = port;

            _udpClient = new UdpClient(address, port);
            _client = new OscClient(new UdpClientConnection(_udpClient), MessageFactory);
            _dispatcher = new OscMessageDispatcher(_client);

            _dispatcher.MessageReceived += (_, msg) =>
            {
                if (_waitQueues.ContainsKey(msg.Address) &&
                    _waitQueues[msg.Address].TryDequeue(out MessageResultHandle mhe))
                {
                    mhe.Result = msg;
                    mhe.Wait.Set();
                }

                MessageReceived?.Invoke(this, msg);
            };

            _dispatcher.MessageSent += (_, msg) => MessageSent?.Invoke(this, msg);
        }

        public X32Client(string address) : this(address, DefaultPort) { }

        ~X32Client()
        {
            Dispose(false);
        }

        public event EventHandler<OscMessage> MessageReceived;

        public event EventHandler<OscMessage> MessageSent;

        public string Address { get; }

        public int Port { get; }

        public X32MessageFactory MessageFactory { get; }

        public RootClient Root => new RootClient(this);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool IsDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                _udpClient.Dispose();
            }

            IsDisposed = true;
        }

        public void Run()
        {
            _dispatcher.Run();
        }

        public void Send(OscMessage msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            _dispatcher.Send(msg);
        }

        public Task SendAsync(OscMessage msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            return _dispatcher.SendAsync(msg);
        }

        public Task Subscribe()
        {
            return Task.Run(async () =>
            {
                OscMessage msg = MessageFactory.Create("/xremote");

                while (true)
                {
                    await SendAsync(msg).ConfigureAwait(false);
                    await Task.Delay(2000).ConfigureAwait(false);
                }
            });
        }

        public OscMessage Query(OscMessage msg)
        {
            return Query(msg, 1000);
        }

        public OscMessage Query(OscMessage msg, int timeout)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            return Query(msg, msg.Address, timeout);
        }

        public OscMessage Query(OscMessage msg, string responseAddress, int timeout)
        {
            if (!_dispatcher.IsRunning)
            {
                throw new InvalidOperationException("Not listening");
            }

            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            using MessageResultHandle messageEvent = AddResponseHandler(responseAddress);
            _client.Send(msg);
            WaitForResponseHandler(messageEvent, timeout);

            return messageEvent.Result;
        }

        public Task<OscMessage> QueryAsync(OscMessage msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            return QueryAsync(msg, msg.Address);
        }

        public Task<OscMessage> QueryAsync(OscMessage msg, string responseAddress)
        {
            if (!_dispatcher.IsRunning)
            {
                throw new InvalidOperationException("Not listening");
            }

            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            using MessageResultHandle messageEvent = AddResponseHandler(responseAddress);
            _ = _client.SendAsync(msg);

            return WaitForResponseHandlerAsync(messageEvent, 1000);
        }

        public T GetValue<T>(string address)
        {
            return Query(MessageFactory.Create(address)).GetValue<T>(0);
        }

        public void SetValue<T>(string address, T value)
        {
            Send(MessageFactory.Create(address, value));
        }

        public bool GetBoolValue(string address)
        {
            return GetValue<int>(address) != 0;
        }

        private MessageResultHandle AddResponseHandler(string address)
        {
            lock (_waitQueues)
            {
                if (!_waitQueues.ContainsKey(address))
                {
                    _waitQueues[address] = new ConcurrentQueue<MessageResultHandle>();
                }
            }

            MessageResultHandle messageEvent = new MessageResultHandle(address);
            _waitQueues[address].Enqueue(messageEvent);

            return messageEvent;
        }

        private static void WaitForResponseHandler(MessageResultHandle messageEvent, int timeout)
        {
            if (!messageEvent.Wait.WaitOne(timeout))
            {
                throw new TimeoutException("No response for " + messageEvent.Address);
            }
        }

        private static Task<OscMessage> WaitForResponseHandlerAsync(MessageResultHandle messageEvent, int timeout)
        {
            TaskCompletionSource<OscMessage> tcSource = new TaskCompletionSource<OscMessage>();
            RegisteredWaitHandle rwHandle = ThreadPool.RegisterWaitForSingleObject(
                messageEvent.Wait,
                (state, timedOut) => {
                    if (timedOut)
                    {
                        throw new TimeoutException("No response for " + messageEvent.Address);
                    }

                    ((TaskCompletionSource<OscMessage>)state).SetResult(messageEvent.Result);
                },
                tcSource,
                timeout,
                true);

            return tcSource.Task;
        }

        private class MessageResultHandle : IDisposable
        {
            public string Address { get; }

            public OscMessage Result { get; set; } = null;

            public readonly EventWaitHandle Wait = new EventWaitHandle(false, EventResetMode.ManualReset);

            public MessageResultHandle(string address)
            {
                Address = address;
            }

            public void Dispose()
            {
                Wait.Dispose();
            }
        }
    }
}
