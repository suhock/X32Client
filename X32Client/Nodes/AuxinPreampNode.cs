using System.Threading.Tasks;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes;

public class AuxinPreampNode : AbstractBaseNode
{
    internal AuxinPreampNode(AbstractBaseNode parent) : base(parent, "preamp")
    {
    }

    public async Task Trim(PreampTrim value) => await SetValue("trim", value).ConfigureAwait(false);

    public async Task<PreampTrim> Trim() =>
        PreampTrim.FromEncodedValue(await GetValue<float>("trim").ConfigureAwait(false));

    public async Task<bool> PhaseInverted() => await GetValue<bool>("invert").ConfigureAwait(false);

    public async Task PhaseInverted(bool invert) => await SetValue("invert", invert).ConfigureAwait(false);
}