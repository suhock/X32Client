using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes
{
    public class GateClient : NodeClient
    {
        internal GateClient(NodeClient outer) :
            base(outer.Client, outer.AddressPrefix + "gate/")
        { }

        public void SetOn(bool on) => SetValue("on", on);

        public bool IsOn() => GetBoolValue("on");

        public void SetMode(GateMode value) => SetValue("mode", value);

        public GateMode GetMode() => (GateMode)GetValue<int>("mode");

        public void SetThreshold(GateThreshold value) => SetValue("thr", value);

        public GateThreshold GetThreshold() => GateThreshold.FromEncodedValue(GetValue<float>("thr"));

        public void SetAttack(AttackTime value) => SetValue("attack", value);

        public AttackTime GetAttack() => AttackTime.FromEncodedValue(GetValue<float>("attack"));

        public void SetHold(HoldTime value) => SetValue("hold", value);

        public HoldTime GetHold() => HoldTime.FromEncodedValue(GetValue<float>("hold"));

        public void SetRelease(ReleaseTime value) => SetValue("release", value);

        public ReleaseTime GetRelease() => ReleaseTime.FromEncodedValue(GetValue<float>("release"));

        public void SetKeySource(Source value) => SetValue("keysrc", value);

        public Source GetKeySource() => (Source)GetValue<int>("keysrc");

        public X32DynamicsFilterContainer Filter { get => GetNode<X32DynamicsFilterContainer>(); }
    }
}
