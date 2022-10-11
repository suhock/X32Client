using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Suhock.X32.Show
{
    internal class X32ShowApp
    {
        private const string DefaultConfigFilename = "x32show.json";

        private static async Task Main(string[] args)
        {
            var configFilename = args.Length > 0 ? args[0] : DefaultConfigFilename;
            X32ShowConfig config;

            using (var fs = File.OpenRead(configFilename))
            {
                config = await JsonSerializer.DeserializeAsync<X32ShowConfig>(fs);
            }

            using var show = new X32Show(config);
            await show.Run().ConfigureAwait(true);
        }
    }
}
