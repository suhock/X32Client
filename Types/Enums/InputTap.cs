using System;

namespace Suhock.X32.Types.Enums;

public enum InputTap
{
    Input = 0,
    PreEq = 1,
    PostEq = 2,
    PreFader = 3,
    PostFader = 4,
    Group = 5
}

public static class InputTapExtensions
{
    private static readonly string[] Mapping =
    {
        "IN/LC",
        "<-EQ",
        "EQ->",
        "PRE",
        "POST",
        "GRP"
    };

    public static string ToNodeString(this InputTap type)
    {
        return Mapping[(int)type];
    }

    public static InputTap FromNodeString(string str)
    {
        var index = Array.IndexOf(Mapping, str);

        return index >= 0 ? (InputTap)index : InputTap.PreFader;
    }
}