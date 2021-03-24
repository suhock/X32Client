using System;

namespace Suhock.X32.Types.Enums
{
    public enum DynamicsMode
    {
        Compressor,
        Expander
    }

    public static class DynamicsModeExtensions
    {
        private static readonly string[] mapping =
        {
            "COMP",
            "EXP"
        };

        public static string ToNodeString(this DynamicsMode type)
        {
            return mapping[(int)type];
        }

        public static DynamicsMode FromNodeString(string str)
        {
            int index = Array.IndexOf(mapping, str);

            return index >= 0 ? (DynamicsMode)index : DynamicsMode.Compressor;
        }
    }
}