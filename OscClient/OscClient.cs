using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Suhock.Osc;

public sealed class OscClient : IOscClient
{
    private readonly IOscConnection _connection;

    private readonly IOscMessageParser _messageParser;

    /// <summary>
    /// Creates an OSC client instance with a default OscMessageFactory implementation, accepting the default
    /// argument types specified in the OSC specification.
    /// </summary>
    /// <param name="connection">an OSC connection implementation</param>
    public OscClient(IOscConnection connection) : this(connection, new OscMessageParser())
    {
    }

    /// <summary>
    /// Creates an OSC client instance.
    /// </summary>
    /// <param name="connection">an OSC connection implementation</param>
    /// <param name="messageParser">a </param>
    public OscClient(IOscConnection connection, IOscMessageParser messageParser)
    {
        _connection = connection;
        _messageParser = messageParser;
    }

    private void VerifyState()
    {
    }

    public OscMessage Receive()
    {
        VerifyState();

        return _messageParser.ParseBytes(_connection.Receive(), out _);
    }

    public async Task<OscMessage> ReceiveAsync(CancellationToken cancellationToken)
    {
        VerifyState();

        return _messageParser.ParseBytes(await _connection.ReceiveAsync(cancellationToken), out _);
    }

    public void Send(OscMessage msg)
    {
        VerifyState();
        _connection.Send(msg.GetPacketBytes());
    }

    public async Task SendAsync(OscMessage msg, CancellationToken cancellationToken)
    {
        VerifyState();
        await _connection.SendAsync(msg.GetPacketBytes(), cancellationToken);
    }
}