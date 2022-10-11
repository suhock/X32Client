using System.Threading.Tasks;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes;

public sealed class ChannelPreampNode : AbstractBaseNode
{
    internal ChannelPreampNode(AbstractBaseNode parent) : base(parent, "preamp")
    {
    }
    
    public async Task Trim(PreampTrim value) => await SetValue("trim", value).ConfigureAwait(false);

    public async Task<PreampTrim> Trim() =>
        PreampTrim.FromEncodedValue(await GetValue<float>("trim").ConfigureAwait(false));

    public async Task<bool> PhaseInverted() => await GetValue<bool>("invert").ConfigureAwait(false);

    public async Task PhaseInverted(bool invert) => await SetValue("invert", invert).ConfigureAwait(false);

    public async Task HpfOn(bool on) => await SetValue("hpon", on).ConfigureAwait(false);

    public async Task<bool> HpfOn() => await GetValue<bool>("hpon").ConfigureAwait(false);

    public async Task HpfSlope(int value) => await SetValue("hpslope", value).ConfigureAwait(false);

    public async Task<int> HpfSlope() => await GetValue<int>("hpslope").ConfigureAwait(false);

    public async Task HpfFrequency(HpfFrequency value) => await SetValue("hpf", value).ConfigureAwait(false);

    public async Task<HpfFrequency> HpfFrequency() =>
        Types.Floats.HpfFrequency.FromEncodedValue(await GetValue<float>("hpf").ConfigureAwait(false));
}