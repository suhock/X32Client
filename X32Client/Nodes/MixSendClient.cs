using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes
{
    public class MixSendClient : NodeClient
    {
        internal MixSendClient _leftSend;

        internal MixSendClient(MixClient outer, int send, MixSendClient leftSend) :
            base(outer.Client, outer.AddressPrefix + send.ToString().PadLeft(2, '0') + '/')
        {
            _leftSend = leftSend;
        }

        public void SetOn(bool on) => SetValue("on", on);

        public bool IsOn() => GetBoolValue("on");

        public void SetLevel(FaderLevel value) => SetValue("level", value);

        public FaderLevel GetLevel() => FaderLevel.FromEncodedValue(GetValue<float>("level"));

        private void SetStereoValue<T>(string path, T value)
        {
            if (_leftSend == null)
            {
                SetValue(path, value);
            }
            else
            {
                _leftSend.SetValue(path, value);
            }
        }

        private T GetStereoValue<T>(string path)
        {
            return _leftSend != null ? _leftSend.GetValue<T>(path) : GetValue<T>(path);
        }

        public void SetPan(Pan value) => SetStereoValue("pan", value);

        public Pan GetPan() => Pan.FromEncodedValue(GetStereoValue<float>("pan"));

        public void SetTapType(InputTap type) => SetStereoValue("type", type);

        public InputTap GetTapType() => (InputTap)GetStereoValue<int>("type");

        public void SetPanFollowOn(bool on) => SetStereoValue("panFollow", on);

        public bool IsPanFollowOn() => GetStereoValue<int>("panFollow") != 0;
    }
}
