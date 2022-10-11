using System;
using System.Threading.Tasks;
using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes;

public sealed class ChannelAutoMixNode : AbstractBaseNode
{
    internal ChannelAutoMixNode(ChannelNode parent) : base(parent, "automix")
    {
        if (parent.Id > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(parent), parent.Id,
                "Only available for channels 1 to 8");
        }
    }

    public async Task Group(AutomixGroup value) => await SetValue("group", value).ConfigureAwait(false);

    public async Task<AutomixGroup> Group() => (AutomixGroup)await GetValue<int>("group").ConfigureAwait(false);

    public async Task Weight(AutoMixWeight value) => await SetValue("weight", value).ConfigureAwait(false);

    public async Task<AutoMixWeight> Weight() =>
        AutoMixWeight.FromEncodedValue(await GetValue<float>("weight").ConfigureAwait(false));
}