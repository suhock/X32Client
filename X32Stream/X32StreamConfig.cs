using System.Collections.Generic;

namespace Suhock.X32.Stream
{
    public class X32StreamConfig
    {
        public X32ClientProfile Source { get; set; } = new X32ClientProfile();

        public X32ClientProfile Destination { get; set; } = new X32ClientProfile();

        public List<string> Patterns { get; } = new List<string>();

        public List<string> Init { get; } = new List<string>();
    }

    public class X32ClientProfile
    {
        public const int DefaultPort = 10023;

        public string Address { get; set; }

        public int Port { get; set; } = DefaultPort;
    }
}
