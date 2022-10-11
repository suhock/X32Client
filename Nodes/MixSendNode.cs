using System.Threading.Tasks;
using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes;

public sealed class MixSendNode : AbstractBaseNode
{
    private readonly MixSendNode? _leftSend;

    internal MixSendNode(AbstractBaseNode parent, int send, MixSendNode? leftSend) : base(parent, $"{send:00}")
    {
        _leftSend = leftSend;
    }

    public async Task On(bool on) => await SetValue("on", on).ConfigureAwait(false);

    public async Task<bool> On() => await GetBoolValue("on").ConfigureAwait(false);

    public async Task Level(FaderLevel value) => await SetValue("level", value).ConfigureAwait(false);

    public async Task<FaderLevel> Level() =>
        FaderLevel.FromEncodedValue(await GetValue<float>("level").ConfigureAwait(false));

    private async Task SetStereoValue<T>(string path, T value)
    {
        if (_leftSend == null)
        {
            await SetValue(path, value).ConfigureAwait(false);
        }
        else
        {
            await _leftSend.SetValue(path, value).ConfigureAwait(false);
        }
    }

    private async Task<T> GetStereoValue<T>(string path)
    {
        return _leftSend != null
            ? await _leftSend.GetValue<T>(path).ConfigureAwait(false)
            : await GetValue<T>(path).ConfigureAwait(false);
    }

    public async Task Pan(PanValue value) => await SetStereoValue("pan", value).ConfigureAwait(false);

    public async Task<PanValue> Pan() =>
        PanValue.FromEncodedValue(await GetStereoValue<float>("pan").ConfigureAwait(false));

    public async Task TapType(InputTap type) => await SetStereoValue("type", (int)type).ConfigureAwait(false);

    public async Task<InputTap> TapType() => (InputTap)await GetStereoValue<int>("type").ConfigureAwait(false);

    public async Task PanFollowOn(bool on) => await SetStereoValue("panFollow", on).ConfigureAwait(false);

    public async Task<bool> PanFollowOn() => await GetStereoValue<int>("panFollow").ConfigureAwait(false) != 0;
}