using System.Threading.Tasks;
using Suhock.X32.Types.Enums;

namespace Suhock.X32.Nodes;

public sealed class InsertNode : AbstractBaseNode
{
    internal InsertNode(AbstractBaseNode parent) : base(parent, "insert")
    {
    }

    public async Task On(bool on) => await SetValue("on", on).ConfigureAwait(false);

    public async Task<bool> On() => await GetBoolValue("on").ConfigureAwait(false);

    public async Task Position(Position value) => await SetValue("pos", value).ConfigureAwait(false);

    public async Task<Position> Position() => (Position)await GetValue<int>("pos").ConfigureAwait(false);

    public async Task Selection(InsertSelection value) => await SetValue("sel", value).ConfigureAwait(false);

    public async Task<InsertSelection> Selection() =>
        (InsertSelection)await GetValue<int>("sel").ConfigureAwait(false);
}