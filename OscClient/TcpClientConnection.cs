using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Suhock.Osc;

public sealed class TcpClientConnection : IOscConnection
{
    private readonly TcpClient _client;

    public TcpClientConnection(TcpClient client)
    {
        _client = client;
    }

    public byte[] Receive()
    {
        var stream = _client.GetStream();
        var sizeBytes = new byte[4];

        if (stream.Read(sizeBytes, 0, 4) < 4)
        {
            throw new Exception();
        }

        var packetLength = OscUtil.ReadInt(sizeBytes, out _);
        var bytesRead = 0;
        var buffer = new byte[packetLength];

        while (bytesRead < packetLength)
        {
            bytesRead += stream.Read(buffer, bytesRead, packetLength - bytesRead);
        }

        return buffer;
    }

    public async Task<byte[]> ReceiveAsync(CancellationToken cancellationToken)
    {
        var stream = _client.GetStream();
        var sizeBytes = new byte[4];

        var headerBytesRead = await stream.ReadAsync(sizeBytes.AsMemory(0, 4), cancellationToken);

        if (headerBytesRead < 4)
        {
            throw new Exception();
        }

        var packetLength = OscUtil.ReadInt(sizeBytes, out _);
        var bytesReadToBuffer = 0;
        var buffer = new byte[packetLength];

        while (bytesReadToBuffer < packetLength)
        {
            bytesReadToBuffer += await stream.ReadAsync(
                buffer.AsMemory(bytesReadToBuffer, packetLength - bytesReadToBuffer),
                cancellationToken);
        }

        return buffer;
    }

    public void Send(ReadOnlySpan<byte> data)
    {
        _client.GetStream().Write(data);
    }

    public async Task SendAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken)
    {
        await _client.GetStream().WriteAsync(data, cancellationToken);
    }
}