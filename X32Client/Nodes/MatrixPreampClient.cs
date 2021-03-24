namespace Suhock.X32.Nodes
{
    public class MatrixPreampClient : NodeClient
    {
        internal MatrixPreampClient(NodeClient outer) :
            base(outer.Client, outer.AddressPrefix + "preamp/")
        { }

        public bool IsInverted() => GetValue<bool>("invert");

        public void SetInverted(bool invert) => SetValue("invert", invert);

    }
}
