
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes
{
    public class DelayClient : NodeClient
    {
        internal DelayClient(NodeClient outer) :
            base(outer.Client, outer.AddressPrefix + "delay/")
        { }

        public void SetOn(bool on) => SetValue("on", on);

        public bool IsOn() => GetBoolValue("on");

        public void SetTime(DelayTime value) => SetValue("time", value);

        public DelayTime GetTime() => DelayTime.FromEncodedValue(GetValue<float>("time"));
    }
}
