using System.Threading.Tasks;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes;

public sealed class MatrixMixNode : AbstractBaseNode
{
    internal MatrixMixNode(AbstractBaseNode parent) : base(parent, "mix")
    {
    }

    public async Task On(bool on) => await SetValue("on", on).ConfigureAwait(false);

    public async Task<bool> On() => await GetBoolValue("on").ConfigureAwait(false);

    public async Task Fader(FaderFineLevel level) => await SetValue("fader", level).ConfigureAwait(false);

    public async Task<FaderFineLevel> Fader() =>
        FaderFineLevel.FromEncodedValue(await GetValue<float>("fader").ConfigureAwait(false));
}