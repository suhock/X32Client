using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Suhock.X32.Routing.Config;

namespace Suhock.X32.Routing;

internal sealed class SheetParser
{
    private readonly IEnumerable<IList<object>> _rows;
    private readonly string _targetConsole;
        
    private readonly int _consoleIndex;
    private readonly int _onIndex;
    private readonly int _linkIndex;
    private readonly int _channelIndex;
    private readonly int _nameIndex;
    private readonly int _groupIndex;
    private readonly int _sourceIndex;

    public ILogger<GoogleSheetSource>? Logger { get; init; }

    public SheetParser(IEnumerable<IList<object>> rows, string targetConsole)
    {
        var rowList = rows as IList<object>[] ?? rows.ToArray();
        var headerRow = rowList.First();
        _rows = rowList.Skip(1);
        _targetConsole = targetConsole;
            
        _consoleIndex = ColumnIndex(headerRow, "Console");
        _onIndex = ColumnIndex(headerRow, "On");
        _linkIndex = ColumnIndex(headerRow, "Link");
        _channelIndex = ColumnIndex(headerRow, "Channel");
        _nameIndex = ColumnIndex(headerRow, "Name");
        _groupIndex = ColumnIndex(headerRow, "Group");
        _sourceIndex = ColumnIndex(headerRow, "Source");
    }
        
    private int ColumnIndex(IList<object> headerRow, string headerName)
    {
        int index;

        if ((index = headerRow.IndexOf(headerName)) < 0)
        {
            throw new Exception("Missing column named " + headerName);
        }

        Logger?.LogInformation("Found {headerName} in column {column}", headerName, (char)('A' + index));

        return index;
    }

    public IEnumerable<ChannelConfig> Parse()
    {
        var result = new Collection<ChannelConfig>();

        foreach (var row in _rows)
        {
            if (row.Count <= _consoleIndex ||
                row[_consoleIndex].ToString() != _targetConsole ||
                row.Count <= _channelIndex)
            {
                continue;
            }

            try
            {
                result.Add(CreateChannelStateFromRow(row));
            }
            catch (Exception e)
            {
                Logger?.LogWarning("Error while parsing row {{{row}}}: {message}", row, e.Message);
            }
        }

        return result;
    }
        
    private ChannelConfig CreateChannelStateFromRow(IList<object> row)
    {
        var channelIdString = GetStringValueFromRow(row, _channelIndex);

        if (!int.TryParse(channelIdString, out var channelId) || channelId is < 1 or > 32)
        {
            throw new InvalidDataException($"Unsupported channel string '{channelIdString}'");
        }

        var on = GetBoolValueFromRow(row, _onIndex);

        return new ChannelConfig()
        {
            Id = channelId,
            Link = on && GetBoolValueFromRow(row, _linkIndex),
            Name = on ? GetStringValueFromRow(row, _nameIndex) : "",
            Template = on ? GetStringValueFromRow(row, _groupIndex) : "",
            Source = on ? GetStringValueFromRow(row, _sourceIndex) : ""
        };
    }

    private static bool GetBoolValueFromRow(IList<object> row, int index)
    {
        return row.Count > index && row[index].Equals("TRUE");
    }
    
    private static string GetStringValueFromRow(IList<object> row, int index)
    {
        return row.Count > index ? (row[index].ToString() ?? "") : "";
    }
}