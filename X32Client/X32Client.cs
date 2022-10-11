using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Suhock.Osc;
using Suhock.X32.Nodes;

namespace Suhock.X32;

public sealed class X32Client : IX32Client
{
    public const int DefaultPort = 10023;

    private readonly IOscQueryClient _queryClient;

    public event EventHandler<OscMessage>? MessageReceived;

    public event EventHandler<OscMessage>? MessageSent;
    
    public ILogger<X32Client>? Logger { get; init; }

    public X32Client(string address, int port = DefaultPort) : this(
        new OscClient(new UdpClientConnection(address, port)))
    {
    }

    public X32Client(IOscClient oscClient) : this(new OscQueryClient(oscClient))
    {
    }

    public X32Client(IOscQueryClient queryClient)
    {
        _queryClient = queryClient;
        
        _queryClient.MessageSent += (_, msg) =>
        {
            LogSend(msg);
            MessageSent?.Invoke(this, msg);
        };
            
        _queryClient.MessageReceived += (_, msg) =>
        {
            LogReceive(msg);
            MessageReceived?.Invoke(this, msg);
        };
        
        Root = new RootNode(queryClient, new OscMessageFactory());
    }

    public RootNode Root { get; }
    
    public void Run()
    {
        _queryClient.Start();
    }

    public void Send(OscMessage msg)
    {
        LogSend(msg);
        _queryClient.Send(msg);
    }

    public Task SendAsync(OscMessage msg) => SendAsync(msg, CancellationToken.None);

    public async Task SendAsync(OscMessage msg, CancellationToken cancellationToken)
    {
        LogSend(msg);
        await _queryClient.SendAsync(msg, cancellationToken).ConfigureAwait(false);
    }

    public Task Subscribe(CancellationToken cancellationToken)
    {
        async Task MessageLoop()
        {
            while (true)
            {
                await Root.XRemote().ConfigureAwait(false);
                await Task.Delay(2000, cancellationToken).ConfigureAwait(false);

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        return Task.Run(MessageLoop, cancellationToken);
    }

    public OscMessage Query(OscMessage msg) => Query(msg, 1000);

    public OscMessage Query(OscMessage msg, int timeout) => Query(msg, msg.Address, timeout);

    public OscMessage Query(OscMessage msg, string responseAddress, int timeout)
    {
        ThrowIfQueryClientNotListening();

        return _queryClient.Query(msg, responseAddress, timeout);
    }

    public Task<OscMessage> QueryAsync(OscMessage msg, CancellationToken cancellationToken) =>
        QueryAsync(msg, msg.Address, cancellationToken);

    private void LogSend(OscMessage msg)
    {
        Logger?.LogInformation("Sending: {msg}", msg);
    }

    private void LogReceive(OscMessage msg)
    {
        Logger?.LogInformation("Receive: {msg}", msg);
    }

    public async Task<OscMessage> QueryAsync(OscMessage msg, string responseAddress,
        CancellationToken cancellationToken)
    {
        ThrowIfQueryClientNotListening();

        return await _queryClient.QueryAsync(msg, responseAddress, cancellationToken);
    }

    private void ThrowIfQueryClientNotListening()
    {
        if (!_queryClient.IsRunning)
        {
            throw new InvalidOperationException("Not listening");
        }
    }
}