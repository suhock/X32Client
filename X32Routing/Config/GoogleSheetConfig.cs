namespace Suhock.X32.Routing.Config;

internal sealed class GoogleSheetConfig
{
    public string CredentialsFilename { get; set; } = "credentials.json";
    public string SpreadsheetId { get; set; } = "";
    public string SpreadsheetRange { get; set; } = "Channels!A1:G";
}