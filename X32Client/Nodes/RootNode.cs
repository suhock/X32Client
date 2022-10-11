using System;
using System.Threading.Tasks;
using Suhock.Osc;
using Suhock.X32.Types.Enums;

namespace Suhock.X32.Nodes;

public sealed class RootNode : AbstractBaseNode
{
    public RootNode(IOscQueryClient client, IOscMessageFactory messageFactory) : base(client, messageFactory, "/")
    {
    }

    public ConfigNode Config => GetNode(() => new ConfigNode(this));

    public ChannelNode Channel(int id) => GetGroupNode(32, id - 1, () => new ChannelNode(this, id));

    public AuxInNode AuxIn(int id) => GetGroupNode(8, id - 1, () => new AuxInNode(this, id));

    public BusNode Bus(int id) => GetGroupNode(16, id - 1, () => new BusNode(this, id));

    public FxReturnNode FxReturn(int id) => GetGroupNode(8, id - 1, () => new FxReturnNode(this, id));

    public MainNode Main => GetNode(() => new MainNode(this));

    public DcaNode Dca(int id) => GetGroupNode(8, id - 1, () => new DcaNode(this, id));

    public HeadAmpNode HeadAmp(int id) => GetGroupNode(128, id, () => new HeadAmpNode(this, id));

    public async Task<int> HeadAmpIndex(Source source)
    {
        if (!source.IsChannel() && !source.IsAux())
        {
            throw new ArgumentOutOfRangeException(nameof(source), source, "Source must be a channel or aux");
        }

        return await GetValue<int>($"-ha/{(int)source - 1:00}/index").ConfigureAwait(false);
    }

    public async Task XRemote() => await Send("xremote").ConfigureAwait(false);
}