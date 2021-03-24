using Suhock.X32.Types.Enums;

namespace Suhock.X32.Nodes
{
    public class InsertClient : NodeClient
    {
        internal InsertClient(NodeClient outer) :
            base(outer.Client, outer.AddressPrefix + "insert/")
        { }

        public void SetOn(bool on) => SetValue("on", on);

        public bool IsOn() => GetBoolValue("on");

        public void SetPosition(Position value) => SetValue("pos", value);

        public Position GetPosition() => (Position)GetValue<int>("pos");

        public void SetSelection(InsertSelection value) => SetValue("sel", value);

        public InsertSelection GetSelection() => (InsertSelection)GetValue<int>("sel");
    }
}
