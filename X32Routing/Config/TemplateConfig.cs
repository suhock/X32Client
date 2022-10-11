using System.Collections.Generic;
using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Sets;

namespace Suhock.X32.Routing.Config;

internal sealed class TemplateConfig
{
    public string Name { get; set; } = "";
    public StripColor Color { get; set; } = StripColor.Black;
    public bool On { get; set; } = true;
    public MuteGroupSet MuteGroups { get; set; } = new();
    public DcaGroupSet DcaGroups { get; set; } = new();
    public bool StereoSendOn { get; set; } = true;
    public ISet<int> Sends { get; set; } = new HashSet<int>();
}