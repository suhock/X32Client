using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Suhock.Osc;

public sealed class UdpClientConnection : IOscConnection
{
    private readonly UdpClient _client;

    public UdpClientConnection(string address, int port) : this(new UdpClient(address, port))
    {
    }
    
    public UdpClientConnection(UdpClient client)
    {
        _client = client;
    }

    public byte[] Receive()
    {
        IPEndPoint? ep = null;

        return _client.Receive(ref ep);
    }

    public async Task<byte[]> ReceiveAsync(CancellationToken cancellationToken)
    {
        return (await _client.ReceiveAsync(cancellationToken)).Buffer;
    }

    public void Send(ReadOnlySpan<byte> data)
    {
        _client.Send(data);
    }
    
    public async Task SendAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken)
    {
        await _client.SendAsync(data, cancellationToken);
    }
}