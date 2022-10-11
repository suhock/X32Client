using System.Threading.Tasks;
using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes;

public sealed class GateNode : AbstractBaseNode
{
    internal GateNode(AbstractBaseNode parent) : base(parent, "gate")
    {
    }

    public async Task On(bool on) => await SetValue("on", on).ConfigureAwait(false);

    public async Task<bool> On() => await GetBoolValue("on").ConfigureAwait(false);

    public async Task Mode(GateMode value) => await SetValue("mode", value).ConfigureAwait(false);

    public async Task<GateMode> Mode() => (GateMode)await GetValue<int>("mode").ConfigureAwait(false);

    public async Task Threshold(GateThreshold value) => await SetValue("thr", value).ConfigureAwait(false);

    public async Task<GateThreshold> Threshold() =>
        GateThreshold.FromEncodedValue(await GetValue<float>("thr").ConfigureAwait(false));

    public async Task Attack(AttackTime value) => await SetValue("attack", value).ConfigureAwait(false);

    public async Task<AttackTime> Attack() =>
        AttackTime.FromEncodedValue(await GetValue<float>("attack").ConfigureAwait(false));

    public async Task Hold(HoldTime value) => await SetValue("hold", value).ConfigureAwait(false);

    public async Task<HoldTime> Hold() =>
        HoldTime.FromEncodedValue(await GetValue<float>("hold").ConfigureAwait(false));

    public async Task Release(ReleaseTime value) => await SetValue("release", value).ConfigureAwait(false);

    public async Task<ReleaseTime> Release() =>
        ReleaseTime.FromEncodedValue(await GetValue<float>("release").ConfigureAwait(false));

    public async Task KeySource(Source value) => await SetValue("keysrc", value).ConfigureAwait(false);

    public async Task<Source> KeySource() => (Source)await GetValue<int>("keysrc").ConfigureAwait(false);

    public DynamicsFilterContainer Filter => GetNode(() => new DynamicsFilterContainer(this));
}