using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Logging;
using Suhock.X32.Routing.Config;

namespace Suhock.X32.Routing;

internal sealed class GoogleSheetSource : IChannelConfigSource
{
    private readonly GoogleSheetConfig _config;
    private readonly string _targetConsole;

    public ILogger<GoogleSheetSource>? Logger { get; init; }

    public GoogleSheetSource(GoogleSheetConfig config, string targetConsole)
    {
        _config = config;
        _targetConsole = targetConsole;
    }

    public async Task<IEnumerable<ChannelConfig>> GetChannelConfigAsync()
    {
        UserCredential credential;

        Logger?.LogInformation(
            $"Reading Google Spreadsheet Id: {_config.SpreadsheetId}, Range: {_config.SpreadsheetRange}");

        await using (var stream = new FileStream(_config.CredentialsFilename, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                new[] { SheetsService.Scope.SpreadsheetsReadonly },
                "user",
                CancellationToken.None,
                new FileDataStore("token.json", true)).Result;
        }

        IList<IList<object>> values;

        using (var service = new SheetsService(new BaseClientService.Initializer()
               {
                   HttpClientInitializer = credential,
                   ApplicationName = "X32 Routing Sync",
               }))
        {
            var request = service.Spreadsheets.Values.Get(_config.SpreadsheetId, _config.SpreadsheetRange);

            var response = await request.ExecuteAsync().ConfigureAwait(false);
            values = response.Values;
        }

        var sheetParser = new SheetParser(values, _targetConsole)
        {
            Logger = Logger
        };

        return sheetParser.Parse();
    }
}