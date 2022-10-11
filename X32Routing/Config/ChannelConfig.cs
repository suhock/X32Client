namespace Suhock.X32.Routing.Config;

internal sealed class ChannelConfig
{
    public int Id { get; set; }
    public bool Link { get; set; }
    public string Name { get; set; } = "";
    public string Template { get; set; } = "";
    public string Source { get; set; } = "";
}