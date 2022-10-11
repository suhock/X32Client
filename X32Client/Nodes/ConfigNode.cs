using System;
using System.Threading.Tasks;

namespace Suhock.X32.Nodes;

public sealed class ConfigNode : AbstractBaseNode
{
    internal ConfigNode(AbstractBaseNode parent) : base(parent, "config")
    {
    }

    public async Task ChannelLink(int index, bool on)
    {
        await SetLink("chlink", index, 32, on).ConfigureAwait(false);
    }

    public Task<bool> ChannelLink(int index)
    {
        return IsLinked("chlink", index, 32);
    }

    public async Task AuxLink(int index, bool on)
    {
        await SetLink("auxlink", index, 8, on).ConfigureAwait(false);
    }

    public async Task<bool> AuxLink(int index)
    {
        return await IsLinked("auxlink", index, 8).ConfigureAwait(false);
    }

    public async Task FxLink(int index, bool on)
    {
        await SetLink("fxlink", index, 8, on).ConfigureAwait(false);
    }

    public async Task<bool> FxLink(int index)
    {
        return await IsLinked("fxlink", index, 8).ConfigureAwait(false);
    }

    public async Task MatrixLink(int index, bool on)
    {
        await SetLink("mtxlink", index, 6, on).ConfigureAwait(false);
    }

    public async Task<bool> MatrixLink(int index)
    {
        return await IsLinked("mtxlink", index, 6).ConfigureAwait(false);
    }

    private async Task SetLink(string path, int index, int maxIndex, bool on)
    {
        CheckRange(index, maxIndex);
        await SetValue($"{path}/{PairPath(index)}", on).ConfigureAwait(false);
    }

    private async Task<bool> IsLinked(string path, int index, int maxIndex)
    {
        CheckRange(index, maxIndex);

        return await GetBoolValue($"{path}/{PairPath(index)}").ConfigureAwait(false);
    }

    private static void CheckRange(int index, int maxIndex)
    {
        if (index < 1 || index > maxIndex || index % 2 == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, $"Must be between 1 and {maxIndex} and be odd");
        }
    }

    private static string PairPath(int index)
    {
        return $"{index}-{index + 1}";
    }

    public UserRoutingNode UserRouting => GetNode(() => new UserRoutingNode(this));
}