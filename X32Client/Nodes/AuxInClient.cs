namespace Suhock.X32.Nodes
{
    public class AuxInClient : IndexedSlotClient<ChannelConfigClient>
    {
        internal AuxInClient(RootClient client, int id) : base(client, "auxin", 8, id) { }

        public PreampClient Preamp { get => GetNode<PreampClient>(); }

        public EqClient Eq { get => GetNode<EqClient>(4); }

        public MixClient Mix { get => GetNode<MixClient>(16); }

        public GroupClient Group { get => GetNode<GroupClient>(); }
    }
}
