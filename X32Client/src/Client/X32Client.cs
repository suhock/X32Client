using Suhock.X32.Client.Message;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Suhock.X32.Client
{
    public class X32Client
    {
        public const int DefaultPort = 10023;

        public int Port { get; }
        public string Address { get; }

        private UdpClient Client;

        private DateTime LastMessageTime = DateTime.MinValue;

        public bool IsConnected { get; private set; } = false;

        public delegate void ConnectionHandler(X32Client client);
        public delegate void MessageHandler(X32Client client, X32Message message);

        public X32Client(string address, int port)
        {
            Address = address;
            Port = port;
        }

        public X32Client(string address) : this(address, DefaultPort) { }

        public ConnectionHandler OnConnect { get; set; }
        public ConnectionHandler OnDisconnect { get; set; }
        public MessageHandler OnMessage { get; set; }

        class MessageHandlerEvent
        {
            public MessageHandler Handler;
            public EventWaitHandle Wait = new EventWaitHandle(false, EventResetMode.ManualReset);

            public MessageHandlerEvent(MessageHandler handler)
            {
                Handler = handler;
            }
        }

        private ConcurrentDictionary<string, ConcurrentQueue<MessageHandlerEvent>> ewhQueues;

        private void Init()
        {
            Client = new UdpClient(Address, Port);
            Client.Client.ReceiveTimeout = 100;
            ewhQueues = new ConcurrentDictionary<string, ConcurrentQueue<MessageHandlerEvent>>();
        }

        private X32Message Receive()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0); ;
            byte[] bytes = Client.Receive(ref ep);

            return new X32Message(bytes);
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

                            X32Message msg = null;

                            try
                            {
                                msg = Receive();
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
                                    throw e;
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
                    } catch (SocketException e)
                    {
                        SetDisconnectState();

                        if (e.SocketErrorCode != SocketError.Interrupted)
                        {
                            throw e;
                        }
                    }
                    catch (Exception e)
                    {
                        SetDisconnectState();

                        throw e;
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
                    Client = null;
                    IsConnected = false;
                    OnDisconnect?.Invoke(this);
                }
            }
        }

        readonly X32Message pingMsg = new X32Message("/xinfo");

        private void SendPingMessage(bool waitForResponse, bool connectionRequired)
        {
            SendMessage(pingMsg, waitForResponse ? (X32Client, X32Message) => { } : (MessageHandler)null, connectionRequired);
        }

        private MessageHandlerEvent RegisterSendResponseHandler(X32Message msg, MessageHandler responseHandler)
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
            } else
            {
                return null;
            }
        }

        private void WaitForResponseHandler(X32Message msg, MessageHandlerEvent mhe)
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

        private void SendMessage(X32Message msg, MessageHandler responseHandler, bool requireConnected)
        {
            if (requireConnected && !IsConnected)
            {
                throw new InvalidOperationException("Not connected");
            }

            MessageHandlerEvent mhe = RegisterSendResponseHandler(msg, responseHandler);
            byte[] bytes = msg.ToBytes();
            Client.Send(bytes, bytes.Length);
            WaitForResponseHandler(msg, mhe);
        }

        public void Send(X32Message msg)
        {
            SendMessage(msg, null, true);
        }

        public void Send(X32Message msg, MessageHandler responseHandler)
        {
            SendMessage(msg, responseHandler, true);
        }

        private async Task SendMessageAsync(X32Message msg, MessageHandler responseHandler, bool requireConnected)
        {
            if (requireConnected && !IsConnected)
            {
                throw new InvalidOperationException("Not connected");
            }

            MessageHandlerEvent mhe = RegisterSendResponseHandler(msg, responseHandler);
            byte[] bytes = msg.ToBytes();
            await Client.SendAsync(bytes, bytes.Length);
            WaitForResponseHandler(msg, mhe);
        }

        public async Task SendAsync(X32Message msg)
        {
            await SendMessageAsync(msg, null, true);
        }

        public async Task SendAsync(X32Message msg, MessageHandler responseHandler)
        {
            await SendMessageAsync(msg, responseHandler, true);
        }

        public async Task Subscribe()
        {
            X32Message msg = new X32Message("/xremote");

            while (true)
            {
                try
                {
                    await SendAsync(msg);
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine(e);
                    Client = null;
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
