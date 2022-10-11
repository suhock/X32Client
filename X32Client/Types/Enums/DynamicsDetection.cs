using System;

namespace Suhock.X32.Types.Enums;

public enum DynamicsDetection
{
    Peak,
    RMS
}

public static class DynamicsDetectionExtensions
{
    private static readonly string[] Mapping =
    {
        "PEAK",
        "RMS"
    };

    public static string ToNodeString(this EqType type)
    {
        return Mapping[(int)type];
    }

    public static DynamicsDetection FromNodeString(string str)
    {
        var index = Array.IndexOf(Mapping, str);

        return index >= 0 ? (DynamicsDetection)index : DynamicsDetection.Peak;
    }
}