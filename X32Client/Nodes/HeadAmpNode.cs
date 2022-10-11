using System.Threading.Tasks;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes;

public sealed class HeadAmpNode : AbstractBaseNode
{
    internal HeadAmpNode(AbstractBaseNode parent, int id) : base(parent, $"headamp/{id:000}")
    {
    }

    public async Task Gain(HeadAmpGain gain) => await SetValue("gain", gain).ConfigureAwait(false);

    public async Task<HeadAmpGain> Gain() =>
        HeadAmpGain.FromEncodedValue(await GetValue<float>("gain").ConfigureAwait(false));

    public async Task Phantom(bool on) => await SetValue("phantom", on ? 1 : 0).ConfigureAwait(false);

    public async Task<bool> Phantom() => await GetBoolValue("phantom").ConfigureAwait(false);
}