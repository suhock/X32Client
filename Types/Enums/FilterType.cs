using System;

namespace Suhock.X32.Types.Enums;

public enum FilterType
{
    LC6,
    LC12,
    HC6,
    HC12,
    Slope1,
    Slope2,
    Slope3,
    Slope5,
    Slope10
}

public static class FilterTypeExtensions
{
    private static readonly string[] Mapping =
    {
        "LC6",
        "LC12",
        "HC6",
        "HC12",
        "1.0",
        "2.0",
        "3.0",
        "5.0",
        "10.0"
    };

    public static string ToNodeString(this FilterType filterType)
    {
        return Mapping[(int)filterType];
    }

    public static FilterType FromNodeString(string str)
    {
        var index = Array.IndexOf(Mapping, str);

        return index >= 0 ? (FilterType)index : FilterType.Slope1;
    }
}