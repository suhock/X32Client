namespace Suhock.X32.Nodes;

public sealed class BusNode : IndexedSlotNode
{
    internal BusNode(AbstractBaseNode parent, int id) : base(parent, "bus", 16, id)
    {
        
    }

    public DynamicsNode Dynamics => GetNode(() => new DynamicsNode(this));

    public InsertNode Insert => GetNode(() => new InsertNode(this));

    public EqNode Eq => GetNode(() => new EqNode(this, 6));

    public MixNode Mix => GetNode(() => new MixNode(this, 6));

    public GroupNode Group => GetNode(() => new GroupNode(this));
    
    public SlotConfigNode Config => GetNode(() => new SlotConfigNode(this));
}