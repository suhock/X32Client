
namespace Suhock.X32.Nodes
{
    public class MatrixClient : IndexedSlotClient<SlotConfigClient>
    {
        internal MatrixClient(RootClient outer, int id) : base(outer, "mtx", 6, id) { }

        public MatrixPreampClient Preamp { get => GetNode<MatrixPreampClient>(); }

        public DynamicsClient Dynamics { get => GetNode<DynamicsClient>(); }

        public InsertClient Insert { get => GetNode<InsertClient>(); }

        public EqClient Eq { get => GetNode<EqClient>(6); }

        public MatrixMixClient Mix { get => GetNode<MatrixMixClient>(); }
    }
}
