namespace Suhock.X32.Nodes;

public sealed class MainNode : AbstractBaseNode
{
    private readonly AbstractBaseNode _parent;

    internal MainNode(AbstractBaseNode parent) : base(parent, "main")
    {
        _parent = parent;
    }

    public MainMixNode Stereo => GetNode(() => new MainMixNode(_parent, "st"));

    public MainMixNode Mono => GetNode(() => new MainMixNode(_parent, "m"));
}