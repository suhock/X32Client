using System.Threading.Tasks;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes;

public sealed class DelayNode : AbstractBaseNode
{
    internal DelayNode(AbstractBaseNode parent) : base(parent, "delay")
    {
    }

    public async Task On(bool on) => await SetValue("on", on).ConfigureAwait(false);

    public async Task<bool> On() => await GetBoolValue("on").ConfigureAwait(false);

    public async Task Time(DelayTime value) => await SetValue("time", value).ConfigureAwait(false);

    public async Task<DelayTime> Time() =>
        DelayTime.FromEncodedValue(await GetValue<float>("time").ConfigureAwait(false));
}