using Suhock.Osc;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Suhock.X32.Client
{
    public class X32Client : IDisposable
    {
        public const int DefaultPort = 10023;

        public int Port { get; }
        public string Address { get; }

        private OscClient Client;

        public bool IsConnected { get; private set; } = false;

        public delegate void ConnectionHandler(X32Client client);
        public delegate void MessageHandler(X32Client client, OscMessage message);

        public X32Client(string address, int port)
        {
            Address = address;
            Port = port;
        }

        public X32Client(string address) : this(address, DefaultPort) { }

        ~X32Client()
        {
            Dispose(false);
        }

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
                Client.Dispose();
                tsMessageLoop.Dispose();

                foreach ((_, var queue) in ewhQueues)
                {
                    while (queue.TryDequeue(out var mhe))
                    {
                        mhe.Dispose();
                    }
                }
            }

            IsDisposed = true;
        }

        public ConnectionHandler OnConnect { get; set; }
        public ConnectionHandler OnDisconnect { get; set; }
        public MessageHandler OnMessage { get; set; }

        private class MessageHandlerEvent : IDisposable
        {
            public MessageHandler Handler;
            public EventWaitHandle Wait = new EventWaitHandle(false, EventResetMode.ManualReset);

            public MessageHandlerEvent(MessageHandler handler)
            {
                Handler = handler;
            }

            public void Dispose()
            {
                Wait.Dispose();
            }
        }

        private ConcurrentDictionary<string, ConcurrentQueue<MessageHandlerEvent>> ewhQueues;

        private void Init()
        {
            Client = new OscClient(Address, Port);
            Client.Client.Client.ReceiveTimeout = 100;
            ewhQueues = new ConcurrentDictionary<string, ConcurrentQueue<MessageHandlerEvent>>();
        }

        private CancellationTokenSource tsMessageLoop = new CancellationTokenSource();

        public Task Connect()
        {
            Task messageLoop;

            lock (tsMessageLoop)
            {
                if (IsConnected)
                {
                    throw new InvalidOperationException("Already connected");
                }

                Init();

                messageLoop = Task.Run(() =>
                {
                    try
                    {
                        DateTime lastMessageTime = DateTime.Now;

                        while (true)
                        {
                            if (DateTime.Now - lastMessageTime > TimeSpan.FromMilliseconds(2000))
                            {
                                SendPingMessage(false, true);
                            }

                            OscMessage msg = null;

                            try
                            {
                                msg = Client.Receive();
                            }
                            catch (SocketException e)
                            {
                                if (e.SocketErrorCode == SocketError.TimedOut)
                                {
                                    if (DateTime.Now - lastMessageTime > TimeSpan.FromMilliseconds(5000))
                                    {
                                        throw new TimeoutException("Connection to " + Address + " timed out", e);
                                    }
                                }
                                else
                                {
                                    throw;
                                }
                            }

                            if (tsMessageLoop.Token.IsCancellationRequested)
                            {
                                return;
                            }

                            if (msg == null)
                            {
                                continue;
                            }

                            lastMessageTime = DateTime.Now;

                            if (!IsConnected)
                            {
                                IsConnected = true;
                                OnConnect?.Invoke(this);
                            }

                            if (ewhQueues.ContainsKey(msg.Address) && ewhQueues[msg.Address].TryDequeue(out MessageHandlerEvent mhe))
                            {
                                mhe.Handler?.Invoke(this, msg);
                                mhe.Wait.Set();
                            }
                            else
                            {
                                OnMessage?.Invoke(this, msg);
                            }
                        }
                    }
                    catch (SocketException e)
                    {
                        SetDisconnectState();

                        if (e.SocketErrorCode != SocketError.Interrupted)
                        {
                            throw;
                        }
                    }
                    catch (Exception)
                    {
                        SetDisconnectState();

                        throw;
                    }
                }, tsMessageLoop.Token);
            }

            SendPingMessage(true, false);

            return messageLoop;
        }

        public void Disconnect()
        {
            SetDisconnectState();
        }

        private void SetDisconnectState()
        {
            lock (tsMessageLoop)
            {
                if (IsConnected)
                {
                    tsMessageLoop.Cancel();
                    Client.Close();
                    IsConnected = false;
                    OnDisconnect?.Invoke(this);
                }
            }
        }

        private readonly OscMessage pingMsg = new OscMessage("/xinfo");

        private void SendPingMessage(bool waitForResponse, bool connectionRequired)
        {
            SendMessage(pingMsg, waitForResponse ? (X32Client, OscMessage) => { } : (MessageHandler)null, connectionRequired);
        }

        private MessageHandlerEvent RegisterSendResponseHandler(OscMessage msg, MessageHandler responseHandler)
        {
            if (responseHandler != null)
            {
                lock (ewhQueues)
                {
                    if (!ewhQueues.ContainsKey(msg.Address))
                    {
                        ewhQueues[msg.Address] = new ConcurrentQueue<MessageHandlerEvent>();
                    }
                }

                MessageHandlerEvent mhe = new MessageHandlerEvent(responseHandler);
                ewhQueues[msg.Address].Enqueue(mhe);

                return mhe;
            }
            else
            {
                return null;
            }
        }

        private void WaitForResponseHandler(OscMessage msg, MessageHandlerEvent mhe)
        {
            if (mhe != null)
            {
                mhe.Wait.WaitOne(5000);

                if (ewhQueues[msg.Address].Contains(mhe))
                {
                    throw new TimeoutException("Failed to receive response for " + msg.ToString());
                }
            }
        }

        private void SendMessage(OscMessage msg, MessageHandler responseHandler, bool requireConnected)
        {
            if (requireConnected && !IsConnected)
            {
                throw new InvalidOperationException("Not connected");
            }

            MessageHandlerEvent mhe = RegisterSendResponseHandler(msg, responseHandler);
            Client.Send(msg);
            WaitForResponseHandler(msg, mhe);

            if (mhe != null)
            {
                mhe.Dispose();
            }
        }

        public void Send(OscMessage msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            SendMessage(msg, null, true);
        }

        public void Send(OscMessage msg, MessageHandler responseHandler)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            SendMessage(msg, responseHandler, true);
        }

        private async Task SendMessageAsync(OscMessage msg, MessageHandler responseHandler, bool requireConnected)
        {
            if (requireConnected && !IsConnected)
            {
                throw new InvalidOperationException("Not connected");
            }

            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            MessageHandlerEvent mhe = RegisterSendResponseHandler(msg, responseHandler);
            await Client.SendAsync(msg).ConfigureAwait(false);
            WaitForResponseHandler(msg, mhe);
            mhe.Dispose();
        }

        public async Task SendAsync(OscMessage msg)
        {
            await SendMessageAsync(msg, null, true).ConfigureAwait(false);
        }

        public async Task SendAsync(OscMessage msg, MessageHandler responseHandler)
        {
            await SendMessageAsync(msg, responseHandler, true).ConfigureAwait(false);
        }

        public async Task Subscribe()
        {
            OscMessage msg = new OscMessage("/xremote");

            while (true)
            {
                try
                {
                    await SendAsync(msg).ConfigureAwait(false);
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine(e);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                await Task.Delay(2000).ConfigureAwait(false);
            }
        }
    }
}
