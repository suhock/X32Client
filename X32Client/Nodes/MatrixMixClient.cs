using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes
{
    public class MatrixMixClient : NodeClient
    {
        internal MatrixMixClient(MatrixClient outer) :
            base(outer.Client, outer.AddressPrefix + "mix/")
        { }

        public void SetOn(bool on) => SetValue("on", on);

        public bool IsOn() => GetBoolValue("on");

        public void SetFader(FaderFineLevel level) => SetValue("fader", level);

        public FaderFineLevel GetFader() => FaderFineLevel.FromEncodedValue(GetValue<float>("fader"));
    }
}
