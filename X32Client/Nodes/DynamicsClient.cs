
using Suhock.X32.Types;
using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes
{
    public class DynamicsClient : NodeClient
    {
        internal DynamicsClient(NodeClient outer) :
            base(outer.Client, outer.AddressPrefix + "dyn/")
        { }

        public void SetOn(bool on) => SetValue("on", on);

        public bool IsOn() => GetBoolValue("on");

        public void SetMode(DynamicsMode value) => SetValue("mode", value);

        public DynamicsMode GetMode() => (DynamicsMode)GetValue<int>("mode");

        public void SetDetection(DynamicsDetection value) => SetValue("det", value);

        public DynamicsDetection GetDetection() => (DynamicsDetection)GetValue<int>("det");

        public void SetCurve(DynamicsCurve value) => SetValue("env", value);

        public DynamicsCurve GetCurve() => (DynamicsCurve)GetValue<int>("env");

        public void SetThreshold(DynamicsThreshold value) => SetValue("thr", value);

        public DynamicsThreshold GetThreshold() => DynamicsThreshold.FromEncodedValue(GetValue<float>("thr"));

        public void SetRatio(DynamicsRatio value) => SetValue("ratio", value);

        public DynamicsRatio GetRatio() => (DynamicsRatio)GetValue<int>("ratio");

        public void SetKnee(Knee value) => SetValue("knee", value);

        public Knee GetKnee() => Knee.FromEncodedValue(GetValue<float>("knee"));

        public void SetMakeupGain(MakeupGain value) => SetValue("mgain", value);

        public MakeupGain GetMakeupGain() => MakeupGain.FromEncodedValue(GetValue<float>("mgain"));

        public void SetAttack(AttackTime value) => SetValue("attack", value);

        public AttackTime GetAttack() => AttackTime.FromEncodedValue(GetValue<float>("attack"));

        public void SetHold(HoldTime value) => SetValue("hold", value?.EncodedValue);

        public HoldTime GetHold() => HoldTime.FromEncodedValue(GetValue<float>("hold"));

        public void SetRelease(ReleaseTime value) => SetValue("release", value);

        public ReleaseTime GetRelease() => ReleaseTime.FromEncodedValue(GetValue<float>("release"));

        public void SetPosition(Position value) => SetValue("pos", value);

        public Position GetPosition() => (Position)GetValue<int>("pos");

        public void SetKeySource(Source value) => SetValue("keysrc", value);

        public Source GetKeySource() => (Source)GetValue<int>("keysrc");

        public void SetMix(MixPercent value) => SetValue("mix", value);

        public MixPercent GetMixPercent() => MixPercent.FromEncodedValue(GetValue<float>("mix"));

        public void SetAutoTimeOn(bool on) => SetValue("auto", on);

        public bool IsAutoTimeOn() => GetBoolValue("auto");

        public X32DynamicsFilterContainer Filter { get => GetNode<X32DynamicsFilterContainer>(); }
    }

    public class X32DynamicsFilterContainer : NodeClient
    {
        internal X32DynamicsFilterContainer(NodeClient outer) :
            base(outer.Client, outer.AddressPrefix + "filter/")
        { }

        public void SetOn(bool on) => SetValue("on", on);

        public bool IsOn() => GetBoolValue("on");

        public void SetFilterType(FilterType value) => SetValue("type", value);

        public FilterType GetFilterType() => (FilterType)GetValue<int>("type");

        public void SetFilterFrequency(Frequency201 value) => SetValue("f", value);

        public Frequency201 GetFilterFrequency() => Frequency201.FromEncodedValue(GetValue<float>("f"));
    }
}
