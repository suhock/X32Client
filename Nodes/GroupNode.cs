using System.Threading.Tasks;
using Suhock.X32.Types.Sets;

namespace Suhock.X32.Nodes;

public sealed class GroupNode : AbstractBaseNode
{
    internal GroupNode(AbstractBaseNode parent) : base(parent, "grp")
    {
    }

    public async Task MuteGroups(MuteGroupSet muteGroups) => await SetValue("mute", muteGroups).ConfigureAwait(false);

    public async Task<MuteGroupSet> MuteGroups() => new(await GetValue<int>("mute").ConfigureAwait(false));

    public async Task MuteGroupOn(int muteGroup, bool on)
    {
        var muteGroups = await MuteGroups().ConfigureAwait(false);

        if ((on && muteGroups.Add(muteGroup)) || (!on && muteGroups.Remove(muteGroup)))
        {
            await MuteGroups(muteGroups).ConfigureAwait(false);
        }
    }

    public async Task DcaGroups(DcaGroupSet dcaGroups) => await SetValue("dca", dcaGroups).ConfigureAwait(false);

    public async Task<DcaGroupSet> DcaGroups() => new(await GetValue<int>("dca").ConfigureAwait(false));

    public async Task DcaGroupOn(int dcaGroup, bool on)
    {
        var dcaGroups = await DcaGroups().ConfigureAwait(false);

        if ((on && dcaGroups.Add(dcaGroup)) || (!on && dcaGroups.Remove(dcaGroup)))
        {
            await DcaGroups(dcaGroups).ConfigureAwait(false);
        }
    }
}