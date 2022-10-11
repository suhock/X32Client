using System;

namespace Suhock.X32.Types.Enums;

public enum Position
{
    Pre,
    Post
}

public static class PositionExtensions
{
    private static readonly string[] Mapping =
    {
        "PRE",
        "POST"
    };

    public static string ToNodeString(this Position type)
    {
        return Mapping[(int)type];
    }

    public static Position FromNodeString(string str)
    {
        var index = Array.IndexOf(Mapping, str);

        return index >= 0 ? (Position)index : Position.Pre;
    }
}