using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Suhock.X32.Routing.Config;

namespace Suhock.X32.Routing;

internal static class X32Routing
{
    private const string DefaultConfigFilename = "x32routing.json";

    private static async Task Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(options => { options.AddConsole(); });

        var configFilename = args.Length > 0 ? args[0] : DefaultConfigFilename;
        var config = await LoadConfig(configFilename).ConfigureAwait(false);
        var channelConfig = await LoadChannelConfig(config, loggerFactory).ConfigureAwait(false);

        IX32Client client = !config.DryRun
            ? new X32Client(config.Address, config.Port)
            {
                Logger = loggerFactory.CreateLogger<X32Client>()
            }
            : new X32Client(new FakeOscQueryClient())
            {
                Logger = loggerFactory.CreateLogger<X32Client>()
            };

        using var x32Routing = new X32RoutingApplication(client, config.Sends, config.Templates, channelConfig)
        {
            Logger = loggerFactory.CreateLogger<X32RoutingApplication>()
        };
        await x32Routing.Run().ConfigureAwait(false);
    }

    private static async Task<X32RoutingConfig> LoadConfig(string configFilename)
    {
        await using var fileStream = File.OpenRead(configFilename);

        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };

        return await JsonSerializer.DeserializeAsync<X32RoutingConfig>(fileStream, jsonOptions) ??
               throw new InvalidDataException($"Could not parse config file '{configFilename}");
    }

    private static async Task<IEnumerable<ChannelConfig>> LoadChannelConfig(X32RoutingConfig config,
        ILoggerFactory loggerFactory)
    {
        var channelConfigSource = new GoogleSheetSource(config.GoogleSheet, config.ConsoleName)
        {
            Logger = loggerFactory.CreateLogger<GoogleSheetSource>()
        };

        return await channelConfigSource.GetChannelConfigAsync().ConfigureAwait(false);
    }
}