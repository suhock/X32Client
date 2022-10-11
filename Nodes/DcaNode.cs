using System.Threading.Tasks;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes;

public sealed class DcaNode : IndexedSlotNode
{
    internal DcaNode(AbstractBaseNode parent, int id) : base(parent, "dca", 8, id)
    {
    }

    public async Task On(bool on) => await SetValue("on", on).ConfigureAwait(false);

    public async Task<bool> On() => await GetBoolValue("on").ConfigureAwait(false);

    public async Task Fader(FaderFineLevel level) => await SetValue("fader", level).ConfigureAwait(false);

    public async Task<FaderFineLevel> Fader() =>
        FaderFineLevel.FromEncodedValue(await GetValue<float>("fader").ConfigureAwait(false));

    public SlotConfigNode Config => GetNode(() => new SlotConfigNode(this));
}