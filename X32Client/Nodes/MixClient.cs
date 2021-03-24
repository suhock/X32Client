using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes
{
    public class MixClient : NodeClient
    {
        public int SendCount { get; }

        internal MixClient(NodeClient outer, int sendCount) :
            base(outer.Client, outer.AddressPrefix + "mix/")
        {
            SendCount = sendCount;
        }

        public void SetOn(bool on) => SetValue("on", on);

        public bool IsOn() => GetBoolValue("on");

        public void SetFader(FaderFineLevel level) => SetValue("fader", level);

        public FaderFineLevel GetFader() => FaderFineLevel.FromEncodedValue(GetValue<float>("fader"));

        public void SetStereoSendOn(bool on) => SetValue("st", on);

        public bool IsStereoSendOn() => GetBoolValue("st");

        public void SetPan(Pan value) => SetValue("pan", value);

        public Pan GetPan() => Pan.FromEncodedValue(GetValue<float>("pan"));

        public void SetMonoSendOn(bool on) => SetValue("mono", on);

        public bool IsMonoSendOn() => GetBoolValue("mono");

        public void SetMonoLevel(FaderLevel level) => SetValue("mlevel", level);

        public FaderLevel GetMonoLevel() => FaderLevel.FromEncodedValue(GetValue<float>("mlevel"));

        public MixSendClient Send(int id) =>
            GetGroupNode<MixSendClient>(SendCount, id - 1, (id & 1) == 0 ? Send(id - 1) : null);
    }
}
