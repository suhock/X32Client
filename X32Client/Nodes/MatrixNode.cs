namespace Suhock.X32.Nodes;

public sealed class MatrixNode : IndexedSlotNode
{
    internal MatrixNode(AbstractBaseNode parent, int id) : base(parent, "mtx", 6, id)
    {
    }

    public MatrixPreampNode Preamp => GetNode(() => new MatrixPreampNode(this));

    public DynamicsNode Dynamics => GetNode(() => new DynamicsNode(this));

    public InsertNode Insert => GetNode(() => new InsertNode(this));

    public EqNode Eq => GetNode(() => new EqNode(this, 6));

    public MatrixMixNode Mix => GetNode(() => new MatrixMixNode(this));

    public SlotConfigNode Config => GetNode(() => new SlotConfigNode(this));
}