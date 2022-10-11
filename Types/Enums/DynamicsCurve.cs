using System;

namespace Suhock.X32.Types.Enums;

public enum DynamicsCurve
{
    Linear,
    Logarithmic
}

public static class DynamicsCurveExtensions
{
    private static readonly string[] Mapping =
    {
        "LIN",
        "LOG"
    };

    public static string ToNodeString(this DynamicsCurve type)
    {
        return Mapping[(int)type];
    }

    public static DynamicsCurve FromNodeString(string str)
    {
        var index = Array.IndexOf(Mapping, str);

        return index >= 0 ? (DynamicsCurve)index : DynamicsCurve.Logarithmic;
    }
}