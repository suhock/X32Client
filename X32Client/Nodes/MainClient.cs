namespace Suhock.X32.Nodes
{
    public class MainClient : NodeClient
    {
        internal MainClient(RootClient outer) : base(outer.Client, outer.AddressPrefix + "main/") { }

        public MainMixClient Stereo => GetNode<MainMixClient>();

        public MainMixClient Mono => GetNode<MainMixClient>();
    }
}
