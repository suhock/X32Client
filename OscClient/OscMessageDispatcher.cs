using System;
using System.Threading;
using System.Threading.Tasks;

namespace Suhock.Osc;

public sealed class OscMessageDispatcher
{
    private readonly IOscClient _client;

    private Task? _task;

    public event EventHandler<OscMessage>? MessageSent;

    public event EventHandler<OscMessage>? MessageReceived;

    public OscMessageDispatcher(IOscClient client)
    {
        _client = client;
    }

    public bool IsRunning => _task is { IsCompleted: false };

    public void Start() => Start(CancellationToken.None);
    
    public void Start(CancellationToken cancellationToken)
    {
        ThrowIfRunning();

        _task = new Task(ReceiveLoop, TaskCreationOptions.LongRunning);
        _task.Start();

        void ReceiveLoop()
        {
            using (_task)
            {
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var msg = _client.Receive();
                    MessageReceived?.Invoke(this, msg);
                }
            }
        }
    }

    public void Send(OscMessage msg)
    {
        ThrowIfNotRunning();
        MessageSent?.Invoke(this, msg);
        _client.Send(msg);
    }

    public Task SendAsync(OscMessage msg) => SendAsync(msg, CancellationToken.None);

    public Task SendAsync(OscMessage msg, CancellationToken cancellationToken)
    {
        ThrowIfNotRunning();
        MessageSent?.Invoke(this, msg);
        
        return _client.SendAsync(msg, cancellationToken);
    }

    private void ThrowIfRunning()
    {
        if (IsRunning)
        {
            throw new InvalidOperationException("Already running");
        }
    }

    private void ThrowIfNotRunning()
    {
        if (!IsRunning)
        {
            throw new InvalidOperationException("Not running");
        }
    }
}