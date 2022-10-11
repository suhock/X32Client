using System;

namespace Suhock.X32.Types.Enums;

public enum StripColor
{
    Black = 0,
    Red = 1,
    Green = 2,
    Yellow = 3,
    Blue = 4,
    Magenta = 5,
    Cyan = 6,
    White = 7,
    BlackInverted = 8,
    RedInverted = 9,
    GreenInverted = 10,
    YellowInverted = 11,
    BlueInverted = 12,
    MagentaInverted = 13,
    CyanInverted = 14,
    WhiteInverted = 15
}

public static class StripColorExtensions
{
    private static readonly string[] Mapping =
    {
        "OFF",
        "RD",
        "GN",
        "YE",
        "BL",
        "MG",
        "CY",
        "WH",
        "OFFi",
        "RDi",
        "GNi",
        "YEi",
        "BLi",
        "MGi",
        "CYi",
        "WHi"
    };

    public static string ToNodeString(this StripColor color)
    {
        return Mapping[(int)color];
    }

    public static StripColor FromNodeString(string str)
    {
        var index = Array.IndexOf(Mapping, str);

        return index >= 0 ? (StripColor)index : StripColor.Black;
    }

    public static bool IsInverted(this StripColor color)
    {
        return color >= StripColor.BlackInverted;
    }
}