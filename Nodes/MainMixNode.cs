namespace Suhock.X32.Nodes;

public sealed class MainMixNode : AbstractSlotNode
{
    internal MainMixNode(AbstractBaseNode parent, string path) : base(parent, path)
    {
    }

    public DynamicsNode Dynamics => GetNode(() => new DynamicsNode(this));

    public InsertNode Insert => GetNode(() => new InsertNode(this));

    public EqNode Eq => GetNode(() => new EqNode(this, 6));

    public MixNode Mix => GetNode(() => new MixNode(this, 6));

    public SlotConfigNode Config => GetNode(() => new SlotConfigNode(this));
}