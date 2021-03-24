using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Suhock.X32.Routing
{
    internal class X32RoutingApp
    {
        private const string DefaultConfigFilename = "x32routing.json";

        private static async Task Main(string[] args)
        {
            string configFilename = args.Length > 0 ? args[0] : DefaultConfigFilename;
            X32RoutingConfig config;

            using (FileStream fs = File.OpenRead(configFilename))
            {
                config = await JsonSerializer.DeserializeAsync<X32RoutingConfig>(fs);
            }

            using X32Routing x32Routing = new X32Routing(config);
            await x32Routing.Run().ConfigureAwait(false);
        }
    }
}
