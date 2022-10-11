using System.Threading.Tasks;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes;

public sealed class MixNode : AbstractBaseNode
{
    public int SendCount { get; }

    internal MixNode(AbstractBaseNode parent, int sendCount) : base(parent, "mix")
    {
        SendCount = sendCount;
    }

    public async Task On(bool on) => await SetValue("on", on).ConfigureAwait(false);

    public async Task<bool> On() => await GetBoolValue("on").ConfigureAwait(false);

    public async Task Fader(FaderFineLevel level) => await SetValue("fader", level).ConfigureAwait(false);

    public async Task<FaderFineLevel> Fader() =>
        FaderFineLevel.FromEncodedValue(await GetValue<float>("fader").ConfigureAwait(false));

    public async Task StereoSendOn(bool on) => await SetValue("st", on).ConfigureAwait(false);

    public async Task<bool> StereoSendOn() => await GetBoolValue("st").ConfigureAwait(false);

    public async Task Pan(PanValue value) => await SetValue("pan", value).ConfigureAwait(false);

    public async Task<PanValue> Pan() => PanValue.FromEncodedValue(await GetValue<float>("pan").ConfigureAwait(false));

    public async Task MonoSendOn(bool on) => await SetValue("mono", on).ConfigureAwait(false);

    public async Task<bool> MonoSendOn() => await GetBoolValue("mono").ConfigureAwait(false);

    public async Task MonoLevel(FaderLevel level) => await SetValue("mlevel", level).ConfigureAwait(false);

    public async Task<FaderLevel> MonoLevel() =>
        FaderLevel.FromEncodedValue(await GetValue<float>("mlevel").ConfigureAwait(false));

    public MixSendNode Send(int id) => GetGroupNode(SendCount, id,
        () => new MixSendNode(this, id, (id & 1) == 0 ? Send(id - 1) : null));
}