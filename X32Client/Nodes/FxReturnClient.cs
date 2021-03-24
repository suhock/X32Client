using System;

namespace Suhock.X32.Nodes
{
    public class FxReturnClient : IndexedSlotClient<SlotConfigClient>
    {
        internal FxReturnClient(RootClient outer, int id) : base(outer, "fxrtn", 8, id) { }

        public EqClient Eq { get => GetNode<EqClient>(4); }

        public MixClient Mix { get => GetNode<MixClient>(16); }

        public GroupClient Group { get => GetNode<GroupClient>(); }
    }
}
