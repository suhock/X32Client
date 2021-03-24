using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Suhock.X32.Stream
{
    internal class X32StreamApp
    {
        private const string DefaultConfigFilename = "x32stream.json";

        private static async Task Main(string[] args)
        {
            string configFilename = args.Length > 0 ? args[0] : DefaultConfigFilename;
            X32StreamConfig config;

            using (FileStream fs = File.OpenRead(configFilename))
            {
                config = await JsonSerializer.DeserializeAsync<X32StreamConfig>(fs);
            }

            using X32Stream x32Stream = new X32Stream(config);
            await x32Stream.Run().ConfigureAwait(true);
        }
    }
}
