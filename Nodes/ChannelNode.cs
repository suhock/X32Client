namespace Suhock.X32.Nodes;

public sealed class ChannelNode : IndexedSlotNode
{
    internal ChannelNode(AbstractBaseNode parent, int id) : base(parent, "ch", 32, id)
    {
    }

    public DelayNode Delay => GetNode(() => new DelayNode(this));

    public ChannelPreampNode Preamp => GetNode(() => new ChannelPreampNode(this));

    public GateNode Gate => GetNode(() => new GateNode(this));

    public DynamicsNode Dynamics => GetNode(() => new DynamicsNode(this));

    public InsertNode Insert => GetNode(() => new InsertNode(this));

    public EqNode Eq => GetNode(() => new EqNode(this, 4));

    public MixNode Mix => GetNode(() => new MixNode(this, 16));

    public GroupNode Group => GetNode(() => new GroupNode(this));

    public ChannelAutoMixNode AutoMix => GetNode(() => new ChannelAutoMixNode(this));

    public ChannelConfigNode Config => GetNode(() => new ChannelConfigNode(this));
}