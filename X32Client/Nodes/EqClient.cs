using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes
{
    public class EqClient : NodeClient
    {
        private readonly int _size;

        internal EqClient(ChannelClient channel, int size) :
            base(channel.Client, channel.AddressPrefix + "eq/")
        {
            _size = size;
        }

        public void SetOn(bool on) => SetValue("on", on);

        public bool IsOn() => GetBoolValue("on");

        public EqValueClient Get(int id) => GetGroupNode<EqValueClient>(_size, id - 1, id);
    }

    public class EqValueClient : NodeClient
    {
        internal EqValueClient(EqClient eq, int id) :
            base(eq.Client, eq.AddressPrefix + id + "/")
        { }

        public void SetEqType(EqType type) => SetValue("type", type);

        public EqType GetEqType() => (EqType)GetValue<int>("type");

        public void SetFrequency(Frequency201 value) => SetValue("f", value);

        public Frequency201 GetFrequency() => Frequency201.FromEncodedValue(GetValue<float>("f"));

        public void SetGain(EqGain value) => SetValue("g", value);

        public EqGain GetGain() => EqGain.FromEncodedValue(GetValue<float>("g"));

        public void SetQuality(QFactor value) => SetValue("q", value);

        public QFactor GetQuality() => QFactor.FromEncodedValue(GetValue<float>("q"));
    }
}
