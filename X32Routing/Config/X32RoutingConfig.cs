using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Suhock.X32.Routing.Config;

internal sealed class X32RoutingConfig
{
    public string Address { get; set; } = "localhost";
    public int Port { get; set; } = X32Client.DefaultPort;
    public GoogleSheetConfig GoogleSheet { get; set; } = new();
    public string ConsoleName { get; set; } = "";
    public bool DryRun { get; set; }
    public IList<SendConfig> Sends { get; set; } = new Collection<SendConfig>();
    public IList<TemplateConfig> Templates { get; set; } = new Collection<TemplateConfig>();
}