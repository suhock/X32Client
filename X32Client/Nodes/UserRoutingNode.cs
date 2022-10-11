using System;
using System.Threading.Tasks;
using Suhock.X32.Types;

namespace Suhock.X32.Nodes;

public sealed class UserRoutingNode : AbstractBaseNode
{
    internal UserRoutingNode(AbstractBaseNode parent) : base(parent, "userrout")
    {
    }

    public async Task<UserRoutingInputSource> In(int index)
    {
        CheckRange(32, index);

        return UserRoutingInputSource.FromInt(await GetValue<int>($"in/{index:00}").ConfigureAwait(false));
    }

    public async Task In(int index, UserRoutingInputSource source)
    {
        CheckRange(32, index);
        await SetValue($"in/{index:00}", source.Value).ConfigureAwait(false);
    }

    public async Task<UserRoutingOutputSource> Out(int index)
    {
        CheckRange(48, index);

        return UserRoutingOutputSource.FromInt(await GetValue<int>("out/{index:00}").ConfigureAwait(false));
    }

    public async Task Out(int index, UserRoutingOutputSource source)
    {
        CheckRange(48, index);
        await SetValue($"out/{index:00}", source.Value).ConfigureAwait(false);
    }

    private static void CheckRange(int range, int index)
    {
        if (index < 1 || index > range)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, $"Must be between 1 and {range}");
        }
    }
}