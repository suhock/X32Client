using Suhock.X32.Types.Enums;

namespace Suhock.X32.Nodes
{
    public class ChannelConfigClient : SlotConfigClient
    {
        internal ChannelConfigClient(NodeClient outer) : base(outer) { }

        public void SetSource(Source value) => SetValue("source", value);

        public Source GetSource() => (Source)GetValue<int>("source");
    }
}
