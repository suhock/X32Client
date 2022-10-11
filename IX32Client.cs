using System;
using System.Threading;
using System.Threading.Tasks;
using Suhock.Osc;
using Suhock.X32.Nodes;

namespace Suhock.X32;

public interface IX32Client
{
    public event EventHandler<OscMessage>? MessageReceived;

    public event EventHandler<OscMessage>? MessageSent;

    public RootNode Root { get; }

    public void Run();

    public void Send(OscMessage msg);

    public Task SendAsync(OscMessage msg) => SendAsync(msg, CancellationToken.None);

    public Task SendAsync(OscMessage msg, CancellationToken cancellationToken);

    public Task Subscribe() => Subscribe(CancellationToken.None);
    
    public Task Subscribe(CancellationToken token);
    
    public OscMessage Query(OscMessage msg);
    
    public OscMessage Query(OscMessage msg, int timeout);
    
    public OscMessage Query(OscMessage msg, string responseAddress, int timeout);

    public Task<OscMessage> QueryAsync(OscMessage msg) => QueryAsync(msg, CancellationToken.None);

    public Task<OscMessage> QueryAsync(OscMessage msg, CancellationToken cancellationToken);

    public Task<OscMessage> QueryAsync(OscMessage msg, string responseAddress) =>
        QueryAsync(msg, responseAddress, CancellationToken.None);

    public Task<OscMessage> QueryAsync(OscMessage msg, string responseAddress, CancellationToken cancellationToken);
}