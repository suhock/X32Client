using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Suhock.Osc;

public sealed class OscQueryClient : IOscQueryClient
{
    private readonly IOscClient _client;

    private readonly OscMessageDispatcher _dispatcher;

    private readonly ConcurrentDictionary<string, ConcurrentQueue<MessageResultHandle>> _waitQueues = new();

    public event EventHandler<OscMessage>? MessageReceived;

    public event EventHandler<OscMessage>? MessageSent;

    public OscQueryClient(IOscClient client)
    {
        _client = client;

        _dispatcher = new OscMessageDispatcher(_client);

        _dispatcher.MessageReceived += (_, msg) =>
        {
            if (_waitQueues.ContainsKey(msg.Address) &&
                _waitQueues[msg.Address].TryDequeue(out var mhe))
            {
                mhe.Result = msg;
                mhe.Wait.Set();
            }

            MessageReceived?.Invoke(this, msg);
        };

        _dispatcher.MessageSent += (_, msg) => MessageSent?.Invoke(this, msg);
    }

    public bool IsRunning => _dispatcher.IsRunning;

    public void Start()
    {
        _dispatcher.Start();
    }

    public void Send(OscMessage msg)
    {
        _dispatcher.Send(msg);
    }

    public Task SendAsync(OscMessage msg) => SendAsync(msg, CancellationToken.None);

    public async Task SendAsync(OscMessage msg, CancellationToken cancellationToken)
    {
        await _dispatcher.SendAsync(msg, cancellationToken).ConfigureAwait(false);
    }

    public OscMessage Query(OscMessage msg) => Query(msg, 1000);

    public OscMessage Query(OscMessage msg, int timeout) => Query(msg, msg.Address, timeout);

    public OscMessage Query(OscMessage msg, string responseAddress, int timeout)
    {
        if (!_dispatcher.IsRunning)
        {
            throw new InvalidOperationException("Not listening");
        }

        using var messageEvent = AddResponseHandler(responseAddress);
        _client.Send(msg);
        WaitForResponseHandler(messageEvent, timeout);

        if (messageEvent.Result is null)
        {
            throw new Exception($"Missing result for {msg}");
        }

        return messageEvent.Result;
    }

    public Task<OscMessage> QueryAsync(OscMessage msg) => QueryAsync(msg, msg.Address, CancellationToken.None);

    public Task<OscMessage> QueryAsync(OscMessage msg, CancellationToken cancellationToken) =>
        QueryAsync(msg, msg.Address, cancellationToken);

    public Task<OscMessage> QueryAsync(OscMessage msg, string responseAddress) =>
        QueryAsync(msg, responseAddress, CancellationToken.None);

    public Task<OscMessage> QueryAsync(OscMessage msg, string responseAddress, CancellationToken cancellationToken)
    {
        if (!_dispatcher.IsRunning)
        {
            throw new InvalidOperationException("Not listening");
        }

        using var messageEvent = AddResponseHandler(responseAddress);
        _ = _client.SendAsync(msg, cancellationToken);

        return WaitForResponseHandlerAsync(messageEvent, 1000, cancellationToken);
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

        var messageEvent = new MessageResultHandle(address);

        lock (_waitQueues)
        {
            lock (_waitQueues[address])
            {
                _waitQueues[address].Enqueue(messageEvent);
            }
        }

        return messageEvent;
    }

    private static void WaitForResponseHandler(MessageResultHandle messageEvent, int timeout)
    {
        if (!messageEvent.Wait.WaitOne(timeout))
        {
            throw new TimeoutException("No response for " + messageEvent.Address);
        }
    }

    private static async Task<OscMessage> WaitForResponseHandlerAsync(MessageResultHandle messageEvent, int timeout,
        CancellationToken cancellationToken)
    {
        var tcSource = new TaskCompletionSource<OscMessage>();
        ThreadPool.RegisterWaitForSingleObject(
            messageEvent.Wait,
            (state, timedOut) =>
            {
                if (timedOut)
                {
                    throw new TimeoutException($"No response for {messageEvent.Address}");
                }

                if (messageEvent.Result is null)
                {
                    throw new Exception($"Missing result for {messageEvent.Address}");
                }

                (state as TaskCompletionSource<OscMessage>)?.SetResult(messageEvent.Result);
            },
            tcSource,
            timeout,
            true);

        await using (cancellationToken.Register(() => tcSource.TrySetCanceled()))
        {
            return await tcSource.Task.ConfigureAwait(false);
        }
    }

    private class MessageResultHandle : IDisposable
    {
        public string Address { get; }

        public OscMessage? Result { get; set; }

        public readonly EventWaitHandle Wait = new(false, EventResetMode.ManualReset);

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