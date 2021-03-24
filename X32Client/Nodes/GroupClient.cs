using Suhock.X32.Types.Sets;

namespace Suhock.X32.Nodes
{
    public class GroupClient : NodeClient
    {
        internal GroupClient(NodeClient outer) : base(outer.Client, outer.AddressPrefix + "grp/") { }

        public void SetMuteGroups(MuteGroupSet muteGroups) => SetValue("mute", muteGroups);

        public MuteGroupSet GetMuteGroups() => new MuteGroupSet(GetValue<int>("mute"));

        public void SetMuteGroupOn(int muteGroup, bool on)
        {
            MuteGroupSet muteGroups = GetMuteGroups();

            if ((on && muteGroups.Add(muteGroup)) || (!on && muteGroups.Remove(muteGroup)))
            {
                SetMuteGroups(muteGroups);
            }
        }

        public void SetDcaGroups(DcaGroupSet dcaGroups) => SetValue("dca", dcaGroups);

        public DcaGroupSet GetDcaGroups() => new DcaGroupSet(GetValue<int>("dca"));

        public void SetDcaGroupOn(int dcaGroup, bool on)
        {
            DcaGroupSet dcaGroups = GetDcaGroups();

            if ((on && dcaGroups.Add(dcaGroup)) || (!on && dcaGroups.Remove(dcaGroup)))
            {
                SetDcaGroups(dcaGroups);
            }
        }
    }
}
