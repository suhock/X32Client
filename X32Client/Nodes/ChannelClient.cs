namespace Suhock.X32.Nodes
{

    public class ChannelClient : IndexedSlotClient<ChannelConfigClient>
    {
        internal ChannelClient(RootClient outer, int id) : base(outer, "ch", 32, id) { }

        public DelayClient Delay { get => GetNode<DelayClient>(); }

        public ChannelPreampClient Preamp { get => GetNode<ChannelPreampClient>(); }

        public GateClient Gate { get => GetNode<GateClient>(); }

        public DynamicsClient Dynamics { get => GetNode<DynamicsClient>(); }

        public InsertClient Insert { get => GetNode<InsertClient>(); }

        public EqClient Eq { get => GetNode<EqClient>(4); }

        public MixClient Mix { get => GetNode<MixClient>(16); }

        public GroupClient Group { get => GetNode<GroupClient>(); }

        public ChannelAutomixClient Automix { get => GetNode<ChannelAutomixClient>(); }
    }
}
