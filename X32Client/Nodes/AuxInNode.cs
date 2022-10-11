﻿namespace Suhock.X32.Nodes;

public sealed class AuxInNode : IndexedSlotNode
{
    internal AuxInNode(AbstractBaseNode parent, int id) : base(parent, "auxin", 8, id)
    {
    }

    public AuxinPreampNode AuxinPreamp => GetNode(() => new AuxinPreampNode(this));

    public EqNode Eq => GetNode(() => new EqNode(this, 4));

    public MixNode Mix => GetNode(() => new MixNode(this, 16));

    public GroupNode Group => GetNode(() => new GroupNode(this));

    public SlotConfigNode Config => GetNode(() => new SlotConfigNode(this));
}