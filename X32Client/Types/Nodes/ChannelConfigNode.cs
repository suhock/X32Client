using Suhock.X32.Types.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suhock.X32.Types.Nodes
{
    public class ChannelConfigNode
    {
        public string Name { get; set; }

        public int? Icon { get; set; }

        public StripColor? Color { get; set; }

        public int? Source { get; set; }

        public override string ToString()
        {
            return Name + " " + Icon + " " + Color?.ToNodeString() + " " + Source;
        }
    }
}
