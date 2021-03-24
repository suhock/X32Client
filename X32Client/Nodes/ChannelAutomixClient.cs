using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;
using System;

namespace Suhock.X32.Nodes
{
    public class ChannelAutomixClient : NodeClient
    {
        internal ChannelAutomixClient(ChannelClient outer) :
            base(outer.Client, outer.AddressPrefix + "automix/")
        {
            if (outer.Id > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(outer), outer.Id,
                    "Only available for channels 1 to 8");
            }
        }

        public void SetGroup(AutomixGroup value) => SetValue("group", value);

        public AutomixGroup GetGroup() => (AutomixGroup)GetValue<int>("group");

        public void SetWeight(AutomixWeight value) => SetValue("weight", value);

        public AutomixWeight GetWeight() => AutomixWeight.FromEncodedValue(GetValue<float>("weight"));
    }
}
