namespace Suhock.X32.Nodes;

public sealed class FxReturnNode : IndexedSlotNode
{
    internal FxReturnNode(AbstractBaseNode parent, int id) : base(parent, "fxrtn", 8, id)
    {
    }

    public EqNode Eq => GetNode(() => new EqNode(this, 4));

    public MixNode Mix => GetNode(() => new MixNode(this, 16));

    public GroupNode Group => GetNode(() => new GroupNode(this));

    public SlotConfigNode Config => GetNode(() => new SlotConfigNode(this));
}