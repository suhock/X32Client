using System;

namespace Suhock.X32.Type
{
    public enum X32Color
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

    public static class X32ColorExtensions
    {
        static readonly string[] mapping =
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

        public static string ToNodeString(this X32Color color)
        {
            return mapping[(int)color];
        }

        public static X32Color FromNodeString(string str)
        {
            int index = Array.BinarySearch(mapping, str);

            return index >= 0 ? (X32Color)index : X32Color.Black;
        }

        public static bool IsInverted(this X32Color color)
        {
            return color >= X32Color.BlackInverted;
        }
    }
}
