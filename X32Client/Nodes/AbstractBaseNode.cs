using Suhock.X32.Types.Sets;
using Suhock.X32.Types.Floats;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Suhock.Osc;

namespace Suhock.X32.Nodes;

public abstract class AbstractBaseNode
{
    private readonly string _addressPrefix;

    private readonly IOscQueryClient _client;

    private readonly IOscMessageFactory _messageFactory;

    private readonly ConcurrentDictionary<Type, object> _containers = new();

    internal AbstractBaseNode(IOscQueryClient oscClient, IOscMessageFactory messageFactory, string addressPrefix)
    {
        _client = oscClient;
        _messageFactory = messageFactory;
        _addressPrefix = addressPrefix;
    }

    internal AbstractBaseNode(AbstractBaseNode parent, string addressPath) : this(parent._client,
        parent._messageFactory,
        $"{parent._addressPrefix}{addressPath}/")
    {
    }

    internal TNode GetNode<TNode>(Func<TNode> factory) where TNode : AbstractBaseNode
    {
        var type = typeof(TNode);
        
        return (TNode)_containers.GetOrAdd(type, _ => factory());
    }

    internal TNode GetGroupNode<TNode>(int length, int index, Func<TNode> factory) where TNode : AbstractBaseNode
    {
        var groupType = typeof(TNode[]);
        var list = (TNode?[])_containers.GetOrAdd(groupType, _ => new TNode?[length]);
        
        return list[index] ??= factory();
    }

    internal async Task Send(string path)
    {
        await _client.SendAsync(_messageFactory.Create(_addressPrefix + path)).ConfigureAwait(false);
    }

    internal async Task SetValue(string path, object value)
    {
        value = value switch
        {
            AbstractSteppedDecimal f => f.EncodedValue,
            BitSet set => set.Bits,
            bool b => b ? 1 : 0,
            Enum e => e.ToString(),
            _ => value
        };

        await _client.SendAsync(_messageFactory.Create(_addressPrefix + path, value)).ConfigureAwait(false);
    }

    internal async Task<T> GetValue<T>(string path)
    {
        var result = await _client.QueryAsync(_messageFactory.Create(_addressPrefix + path))
            .ConfigureAwait(false);

        return result.GetArgumentValue<T>(0);
    }

    internal async Task<bool> GetBoolValue(string path)
    {
        var result = await GetValue<int>(path).ConfigureAwait(false);

        return result != 0;
    }
}