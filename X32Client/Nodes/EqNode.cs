using System.Threading.Tasks;

namespace Suhock.X32.Nodes;

public sealed class EqNode : AbstractBaseNode
{
    private readonly int _bandCount;

    internal EqNode(AbstractBaseNode parent, int bandCount) : base(parent, "eq")
    {
        _bandCount = bandCount;
    }

    public async Task On(bool on) => await SetValue("on", on).ConfigureAwait(false);

    public async Task<bool> On() => await GetBoolValue("on").ConfigureAwait(false);

    public EqValueNode Band(int id) => GetGroupNode(_bandCount, id - 1, () => new EqValueNode(this, id));
}