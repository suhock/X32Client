using System;

namespace Suhock.X32.Types.Enums
{
    public enum DynamicsDetection
    {
        Peak,
        RMS
    }

    public static class DynamicsDetectionExtensions
    {
        private static readonly string[] mapping =
        {
            "PEAK",
            "RMS"
        };

        public static string ToNodeString(this EqType type)
        {
            return mapping[(int)type];
        }

        public static DynamicsDetection FromNodeString(string str)
        {
            int index = Array.IndexOf(mapping, str);

            return index >= 0 ? (DynamicsDetection)index : DynamicsDetection.Peak;
        }
    }
}