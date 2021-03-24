using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes
{
    public class PreampClient : NodeClient
    {
        internal PreampClient(NodeClient outer) :
            base(outer.Client, outer.AddressPrefix + "preamp/")
        { }

        public void SetTrim(PreampTrim value) => SetValue("trim", value);

        public PreampTrim GetTrim() => PreampTrim.FromEncodedValue(GetValue<float>("trim"));

        public bool IsInverted() => GetValue<bool>("invert");

        public void SetInverted(bool invert) => SetValue("invert", invert);

    }
}
