using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes
{
    public class ChannelPreampClient : PreampClient
    {
        internal ChannelPreampClient(NodeClient outer) : base(outer) { }

        public void SetHpfOn(bool on) => SetValue("hpon", on);

        public bool IsHpfOn() => GetValue<bool>("hpon");

        public void SetHpfSlope(int value) => SetValue("hpslope", value);

        public int GetHpfSlope() => GetValue<int>("hpslope");

        public void SetHpfFrequency(HpfFrequency value) => SetValue("hpf", value);

        public HpfFrequency GetHpfFrequency() => HpfFrequency.FromEncodedValue(GetValue<float>("hpf"));
    }
}
