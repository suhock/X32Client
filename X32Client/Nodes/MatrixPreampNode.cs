using System.Threading.Tasks;

namespace Suhock.X32.Nodes;

public sealed class MatrixPreampNode : AbstractBaseNode
{
    internal MatrixPreampNode(AbstractBaseNode parent) : base(parent, "preamp")
    {
    }

    public async Task<bool> PhaseInverted() => await GetValue<bool>("invert").ConfigureAwait(false);

    public async Task PhaseInverted(bool invert) => await SetValue("invert", invert).ConfigureAwait(false);
}