using System;

namespace Suhock.X32.Types.Enums;

public enum DynamicsRatio
{
    R1p1,
    R1p3,
    R1p5,
    R2p0,
    R2p5,
    R3p0,
    R4p0,
    R5p0,
    R7p0,
    R10,
    R20,
    R100
}

public static class DynamicsRatioExtensions
{
    private static readonly string[] Mapping =
    {
        "1.1",
        "1.3",
        "1.5",
        "2.0",
        "2.5",
        "3.0",
        "4.0",
        "5.0",
        "7.0",
        "10",
        "20",
        "100"
    };

    private static readonly float[] FloatMapping =
    {
        1.1f,
        1.3f,
        1.5f,
        2.0f,
        2.5f,
        3.0f,
        4.0f,
        5.0f,
        7.0f,
        10.0f,
        20.0f,
        100.0f
    };

    public static string ToNodeString(this DynamicsRatio value)
    {
        return Mapping[(int)value];
    }

    public static float ToFloat(this DynamicsRatio value)
    {
        return FloatMapping[(int)value];
    }

    public static DynamicsRatio FromNodeString(string str)
    {
        var index = Array.IndexOf(Mapping, str);

        return index >= 0 ? (DynamicsRatio)index : DynamicsRatio.R2p0;
    }
}