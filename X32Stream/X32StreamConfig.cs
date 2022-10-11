using System.Collections.Generic;

namespace Suhock.X32.Stream
{
    public class X32StreamConfig
    {
        public X32ClientProfile Source { get; set; } = new();

        public X32ClientProfile Destination { get; set; } = new();

        public IEnumerable<string> Patterns { get; } = new List<string>();

        public List<string> Init { get; } = new();
    }

    public class X32ClientProfile
    {
        public const int DefaultPort = 10023;

        public string Address => "localhost";

        public int Port => DefaultPort;
    }
}
