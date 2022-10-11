using System.Threading.Tasks;
using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes;

public sealed class DynamicsFilterContainer : AbstractBaseNode
{
    internal DynamicsFilterContainer(AbstractBaseNode parent) : base(parent, "filter")
    {
    }

    public async Task On(bool on) => await SetValue("on", on).ConfigureAwait(false);

    public async Task<bool> On() => await GetBoolValue("on").ConfigureAwait(false);

    public async Task FilterType(FilterType value) => await SetValue("type", value).ConfigureAwait(false);

    public async Task<FilterType> FilterType() => (FilterType)await GetValue<int>("type").ConfigureAwait(false);

    public async Task FilterFrequency(Frequency201 value) => await SetValue("f", value).ConfigureAwait(false);

    public async Task<Frequency201> FilterFrequency() =>
        Frequency201.FromEncodedValue(await GetValue<float>("f").ConfigureAwait(false));
}