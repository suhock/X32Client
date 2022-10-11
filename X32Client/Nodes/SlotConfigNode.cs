using System.Threading.Tasks;
using Suhock.X32.Types.Enums;

namespace Suhock.X32.Nodes;

public class SlotConfigNode : AbstractBaseNode
{
    internal SlotConfigNode(AbstractBaseNode parent) : base(parent, "config")
    {
    }

    public async Task Name(string name) => await SetValue("name", name).ConfigureAwait(false);

    public async Task<string> Name() => await GetValue<string>("name").ConfigureAwait(false);

    public async Task Icon(int icon) => await SetValue("icon", icon).ConfigureAwait(false);

    public async Task<int> Icon() => await GetValue<int>("icon").ConfigureAwait(false);

    public async Task Color(StripColor color) => await SetValue("color", (int)color).ConfigureAwait(false);

    public async Task<StripColor> Color() => (StripColor)await GetValue<int>("color").ConfigureAwait(false);
}