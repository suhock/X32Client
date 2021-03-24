namespace Suhock.X32.Nodes
{
    public abstract class SlotClient<TConfig> : NodeClient where TConfig : SlotConfigClient
    {
        internal SlotClient(RootClient outer, string path) : base(outer.Client, outer.AddressPrefix + path) { }

        public virtual TConfig Config { get => GetNode<TConfig>(); }
    }
}
