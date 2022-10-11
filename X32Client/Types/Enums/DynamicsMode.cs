using System;

namespace Suhock.X32.Types.Enums;

public enum DynamicsMode
{
    Compressor,
    Expander
}

public static class DynamicsModeExtensions
{
    private static readonly string[] Mapping =
    {
        "COMP",
        "EXP"
    };

    public static string ToNodeString(this DynamicsMode type)
    {
        return Mapping[(int)type];
    }

    public static DynamicsMode FromNodeString(string str)
    {
        var index = Array.IndexOf(Mapping, str);

        return index >= 0 ? (DynamicsMode)index : DynamicsMode.Compressor;
    }
}