using System.Threading.Tasks;
using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes;

public sealed class DynamicsNode : AbstractBaseNode
{
    internal DynamicsNode(AbstractBaseNode parent) : base(parent, "dyn")
    {
    }

    public async Task On(bool on) => await SetValue("on", on).ConfigureAwait(false);

    public async Task<bool> On() => await GetBoolValue("on").ConfigureAwait(false);

    public async Task Mode(DynamicsMode value) => await SetValue("mode", value).ConfigureAwait(false);

    public async Task<DynamicsMode> Mode() => (DynamicsMode)await GetValue<int>("mode").ConfigureAwait(false);

    public async Task Detection(DynamicsDetection value) => await SetValue("det", value).ConfigureAwait(false);

    public async Task<DynamicsDetection> Detection() =>
        (DynamicsDetection)await GetValue<int>("det").ConfigureAwait(false);

    public async Task Curve(DynamicsCurve value) => await SetValue("env", value).ConfigureAwait(false);

    public async Task<DynamicsCurve> Curve() => (DynamicsCurve)await GetValue<int>("env").ConfigureAwait(false);

    public async Task Threshold(DynamicsThreshold value) => await SetValue("thr", value).ConfigureAwait(false);

    public async Task<DynamicsThreshold> Threshold() =>
        DynamicsThreshold.FromEncodedValue(await GetValue<float>("thr").ConfigureAwait(false));

    public async Task Ratio(DynamicsRatio value) => await SetValue("ratio", value).ConfigureAwait(false);

    public async Task<DynamicsRatio> Ratio() => (DynamicsRatio)await GetValue<int>("ratio").ConfigureAwait(false);

    public async Task Knee(KneeValue value) => await SetValue("knee", value).ConfigureAwait(false);

    public async Task<KneeValue> Knee() => KneeValue.FromEncodedValue(await GetValue<float>("knee").ConfigureAwait(false));

    public async Task MakeupGain(MakeupGainValue value) => await SetValue("mgain", value).ConfigureAwait(false);

    public async Task<MakeupGainValue> MakeupGain() =>
        MakeupGainValue.FromEncodedValue(await GetValue<float>("mgain").ConfigureAwait(false));

    public async Task Attack(AttackTime value) => await SetValue("attack", value).ConfigureAwait(false);

    public async Task<AttackTime> Attack() =>
        AttackTime.FromEncodedValue(await GetValue<float>("attack").ConfigureAwait(false));

    public async Task Hold(HoldTime value) => await SetValue("hold", value.EncodedValue).ConfigureAwait(false);

    public async Task<HoldTime> Hold() =>
        HoldTime.FromEncodedValue(await GetValue<float>("hold").ConfigureAwait(false));

    public async Task Release(ReleaseTime value) => await SetValue("release", value).ConfigureAwait(false);

    public async Task<ReleaseTime> Release() =>
        ReleaseTime.FromEncodedValue(await GetValue<float>("release").ConfigureAwait(false));

    public async Task Position(Position value) => await SetValue("pos", value).ConfigureAwait(false);

    public async Task<Position> Position() => (Position)await GetValue<int>("pos").ConfigureAwait(false);

    public async Task KeySource(Source value) => await SetValue("keysrc", value).ConfigureAwait(false);

    public async Task<Source> KeySource() => (Source)await GetValue<int>("keysrc").ConfigureAwait(false);

    public async Task Mix(MixPercentage value) => await SetValue("mix", value).ConfigureAwait(false);

    public async Task<MixPercentage> MixPercent() =>
        MixPercentage.FromEncodedValue(await GetValue<float>("mix").ConfigureAwait(false));

    public async Task AutoTimeOn(bool on) => await SetValue("auto", on).ConfigureAwait(false);

    public async Task<bool> AutoTimeOn() => await GetBoolValue("auto").ConfigureAwait(false);

    public DynamicsFilterContainer Filter => GetNode(() => new DynamicsFilterContainer(this));
}