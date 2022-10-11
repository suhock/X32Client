using System;

namespace Suhock.X32.Types.Enums;

public enum AutomixGroup
{
    Off,
    X,
    Y
}

public static class AutomixGroupExtensions
{
    private static readonly string[] Mapping =
    {
        "OFF",
        "X",
        "Y"
    };

    public static string ToNodeString(this FilterType filterType)
    {
        return Mapping[(int)filterType];
    }

    public static AutomixGroup FromNodeString(string str)
    {
        var index = Array.IndexOf(Mapping, str);

        return index >= 0 ? (AutomixGroup)index : AutomixGroup.Off;
    }
}