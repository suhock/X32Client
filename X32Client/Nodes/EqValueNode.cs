using System.Threading.Tasks;
using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Nodes;

public sealed class EqValueNode : AbstractBaseNode
{
    internal EqValueNode(AbstractBaseNode parent, int id) : base(parent, id.ToString())
    {
    }

    public async Task EqType(EqType type) => await SetValue("type", type).ConfigureAwait(false);

    public async Task<EqType> EqType() => (EqType)await GetValue<int>("type").ConfigureAwait(false);

    public async Task Frequency(Frequency201 value) => await SetValue("f", value).ConfigureAwait(false);

    public async Task<Frequency201> Frequency() =>
        Frequency201.FromEncodedValue(await GetValue<float>("f").ConfigureAwait(false));

    public async Task Gain(EqGain value) => await SetValue("g", value).ConfigureAwait(false);

    public async Task<EqGain> Gain() => EqGain.FromEncodedValue(await GetValue<float>("g").ConfigureAwait(false));

    public async Task Quality(QFactor value) => await SetValue("q", value).ConfigureAwait(false);

    public async Task<QFactor> Quality() =>
        QFactor.FromEncodedValue(await GetValue<float>("q").ConfigureAwait(false));
}