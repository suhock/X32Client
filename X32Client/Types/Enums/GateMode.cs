using System;

namespace Suhock.X32.Types.Enums
{
    public enum GateMode
    {
        Expander2to1,
        Expander3to1,
        Expander4to1,
        Gate,
        Duck
    }

    public static class GateModeExtensions
    {
        private static readonly string[] mapping =
        {
            "EXP2",
            "EXP3",
            "EXP4",
            "GATE",
            "DUCK"
        };

        public static string ToNodeString(this GateMode type)
        {
            return mapping[(int)type];
        }

        public static GateMode FromNodeString(string str)
        {
            int index = Array.IndexOf(mapping, str);

            return index >= 0 ? (GateMode)index : GateMode.Gate;
        }
    }
}