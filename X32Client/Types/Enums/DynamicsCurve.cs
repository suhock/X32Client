using System;

namespace Suhock.X32.Types.Enums
{
    public enum DynamicsCurve
    {
        Linear,
        Logarithmic
    }

    public static class DynamicsCurveExtensions
    {
        private static readonly string[] mapping =
        {
            "LIN",
            "LOG"
        };

        public static string ToNodeString(this DynamicsCurve type)
        {
            return mapping[(int)type];
        }

        public static DynamicsCurve FromNodeString(string str)
        {
            int index = Array.IndexOf(mapping, str);

            return index >= 0 ? (DynamicsCurve)index : DynamicsCurve.Logarithmic;
        }
    }
}