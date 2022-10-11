using System;

namespace Suhock.X32.Types.Enums;

public enum EqType
{
    LowCut,
    LowShelf,
    ParametricEq,
    VintageEq,
    HighShelf,
    HighCut
}

public static class EqTypeExtensions
{
    private static readonly string[] Mapping =
    {
        "LCut",
        "LShv",
        "PEQ",
        "VEQ",
        "HShv",
        "HCut"
    };

    public static string ToNodeString(this EqType type)
    {
        return Mapping[(int)type];
    }

    public static EqType FromNodeString(string str)
    {
        var index = Array.IndexOf(Mapping, str);

        return index >= 0 ? (EqType)index : EqType.ParametricEq;
    }
}