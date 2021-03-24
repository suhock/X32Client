
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes
{
    public class DcaClient : IndexedSlotClient<SlotConfigClient>
    {
        internal DcaClient(RootClient outer, int id) : base(outer, "dca", 8, id) { }

        public void SetOn(bool on) => SetValue("on", on);

        public bool IsOn() => GetBoolValue("on");

        public void SetFader(FaderFineLevel level) => SetValue("fader", level);

        public FaderFineLevel GetFader() => FaderFineLevel.FromEncodedValue(GetValue<float>("fader"));
    }
}
