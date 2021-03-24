namespace Suhock.X32.Nodes
{
    public class BusClient : IndexedSlotClient<SlotConfigClient>
    {
        internal BusClient(RootClient outer, int id) : base(outer, "bus", 16, id) { }

        public DynamicsClient Dynamics => GetNode<DynamicsClient>();

        public InsertClient Insert => GetNode<InsertClient>();

        public EqClient Eq => GetNode<EqClient>(6);

        public MixClient Mix => GetNode<MixClient>(6);

        public GroupClient Group => GetNode<GroupClient>();
    }
}
