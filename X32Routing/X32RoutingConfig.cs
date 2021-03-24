using Suhock.X32.Types.Sets;
using Suhock.X32.Types.Enums;
using System.Collections.Generic;

namespace Suhock.X32.Routing
{
    public class X32RoutingConfig
    {
        public string Address { get; set; }
        public int Port { get; set; } = X32Client.DefaultPort;
        public string CredentialsFilename { get; set; } = "credentials.json";
        public string SpreadsheetId { get; set; }
        public string SpreadsheetRange { get; set; } = "Channels!A1:G";
        public string ConsoleName { get; set; }
        public bool DryRun { get; set; } = false;

        public List<X32RoutingConfigGroup> Groups { get; } = new List<X32RoutingConfigGroup>();
    }

    public class X32RoutingConfigGroup
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Id { get; set; }
        public StripColor Color { get; set; }
        public MuteGroupSet MuteGroups { get; } = new MuteGroupSet();
    }

}