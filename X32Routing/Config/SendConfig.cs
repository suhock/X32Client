using Suhock.X32.Types.Enums;

namespace Suhock.X32.Routing.Config;

internal sealed class SendConfig
{
    public int Id { get; set; }
    public InputTap TapType { get; set; } = InputTap.Group;
}