using System;
using System.Threading;
using System.Threading.Tasks;

namespace Suhock.Osc;

public interface IOscQueryClient
{
    event EventHandler<OscMessage>? MessageReceived;

    event EventHandler<OscMessage>? MessageSent;

    bool IsRunning { get; }

    void Start();

    void Send(OscMessage msg);

    Task SendAsync(OscMessage msg) => SendAsync(msg, CancellationToken.None);

    Task SendAsync(OscMessage msg, CancellationToken cancellationToken);

    OscMessage Query(OscMessage msg) => Query(msg, msg.Address, 500);

    OscMessage Query(OscMessage msg, int timeout) => Query(msg, msg.Address, 500);

    OscMessage Query(OscMessage msg, string responseAddress, int timeout);

    Task<OscMessage> QueryAsync(OscMessage msg) => QueryAsync(msg, msg.Address, CancellationToken.None);

    Task<OscMessage> QueryAsync(OscMessage msg, CancellationToken cancellationToken) =>
        QueryAsync(msg, msg.Address, cancellationToken);

    Task<OscMessage> QueryAsync(OscMessage msg, string responseAddress) =>
        QueryAsync(msg, responseAddress, CancellationToken.None);

    Task<OscMessage> QueryAsync(OscMessage msg, string responseAddress, CancellationToken cancellationToken);
}