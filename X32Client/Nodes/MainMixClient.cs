namespace Suhock.X32.Nodes
{
    public class MainMixClient : SlotClient<SlotConfigClient>
    {
        internal MainMixClient(RootClient outer, string path) : base(outer, path) { }

        public DynamicsClient Dynamics => GetNode<DynamicsClient>();

        public InsertClient Insert => GetNode<InsertClient>();

        public EqClient Eq => GetNode<EqClient>(6);

        public MixClient Mix => GetNode<MixClient>(6);
    }
}
