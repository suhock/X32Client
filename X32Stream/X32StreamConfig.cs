namespace Suhock.X32Stream
{
    class X32StreamConfig
    {
        public X32Profile Source { get; set; } = new X32Profile();

        public X32Profile Destination { get; set; } = new X32Profile();

        public string[] Patterns { get; set; }

        public string[] Init { get; set; }

        public class X32Profile
        {
            public const int DefaultPort = 10023;

            public string Address { get; set; }

            public int Port { get; set; } = DefaultPort;
        }
    }
}
