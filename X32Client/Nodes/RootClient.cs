namespace Suhock.X32.Nodes
{
    public class RootClient : NodeClient
    {
        internal RootClient(X32Client client) : base(client, "/") { }

        public ChannelClient Channel(int id) => GetGroupNode<ChannelClient>(32, id - 1, id);

        public AuxInClient AuxIn(int id) => GetGroupNode<AuxInClient>(8, id - 1, id);

        public BusClient Bus(int id) => GetGroupNode<BusClient>(16, id - 1, id);

        public FxReturnClient FxReturn(int id) => GetGroupNode<FxReturnClient>(8, id - 1, id);

        public MainClient Main => GetNode<MainClient>();

        public DcaClient Dca(int id) => GetGroupNode<DcaClient>(8, id - 1, id);
    }
}
