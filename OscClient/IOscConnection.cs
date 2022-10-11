using System;
using System.Threading;
using System.Threading.Tasks;

namespace Suhock.Osc;

public interface IOscConnection
{
    public byte[] Receive();

    public Task<byte[]> ReceiveAsync(CancellationToken cancellationToken);

    public void Send(ReadOnlySpan<byte> data);

    public Task SendAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken);

}