using System.Threading.Tasks;
using Suhock.X32.Types.Enums;

namespace Suhock.X32.Nodes;

public sealed class ChannelConfigNode : SlotConfigNode
{
    internal ChannelConfigNode(AbstractBaseNode parent) : base(parent)
    {
    }

    public async Task Source(Source value) => await SetValue("source", (int)value).ConfigureAwait(false);

    public async Task<Source> Source() => (Source)await GetValue<int>("source").ConfigureAwait(false);
}