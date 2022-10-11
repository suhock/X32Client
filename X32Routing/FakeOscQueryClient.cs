using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Suhock.Osc;
using Suhock.Osc.Arguments;

namespace Suhock.X32.Routing;

internal sealed class FakeOscQueryClient : IOscQueryClient
{
    public event EventHandler<OscMessage>? MessageReceived;
    
    public event EventHandler<OscMessage>? MessageSent;
    
    public bool IsRunning => true;
    
    public void Start()
    {
    }

    public void Send(OscMessage msg)
    {
        MessageSent?.Invoke(this, msg);
    }

    public Task SendAsync(OscMessage msg, CancellationToken cancellationToken)
    {
        MessageSent?.Invoke(this, msg);
        return Task.Run(() => { }, cancellationToken);
    }

    public OscMessage Query(OscMessage msg, string responseAddress, int timeout)
    {
        return QueryAsync(msg, responseAddress, CancellationToken.None).Result;
    }

    public Task<OscMessage> QueryAsync(OscMessage msg, string responseAddress, CancellationToken cancellationToken)
    {
        MessageSent?.Invoke(this, msg);

        var result = msg.Address switch
        {
            var x when new Regex("/-ha/.*/index").IsMatch(x) => new[]
            {
                new OscIntArgument(0)
            },
            var x when new Regex("/headamp/.*/phantom").IsMatch(x) => new[]
            {
                new OscIntArgument(0)
            },
            var x when new Regex("/headamp/.*/gain").IsMatch(x) => new[]
            {
                new OscFloatArgument(0)
            },
            _ => Array.Empty<IOscArgument>()
        };

        var response = new OscMessage(msg.Address, result);
        MessageReceived?.Invoke(this, response);

        return Task.Run(() => response, cancellationToken);
    }
}