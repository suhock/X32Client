using Suhock.X32.Types.Enums;

namespace Suhock.X32.Nodes
{
    public class SlotConfigClient : NodeClient
    {
        internal SlotConfigClient(NodeClient outer) :
            base(outer.Client, outer.AddressPrefix + "config/")
        { }

        public void SetName(string name) => SetValue("name", name);

        public string GetName() => GetValue<string>("name");

        public void SetIcon(int icon) => SetValue("icon", icon);

        public int GetIcon() => GetValue<int>("icon");

        public void SetColor(StripColor color) => SetValue("color", color);

        public StripColor GetColor() => (StripColor)GetValue<int>("color");
    }
}
