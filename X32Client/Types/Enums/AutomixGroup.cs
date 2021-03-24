using System;

namespace Suhock.X32.Types.Enums
{
    public enum AutomixGroup
    {
        Off,
        X,
        Y
    }

    public static class AutomixGroupExtensions
    {
        private static readonly string[] mapping =
        {
            "OFF",
            "X",
            "Y"
        };

        public static string ToNodeString(this FilterType filterType)
        {
            return mapping[(int)filterType];
        }

        public static AutomixGroup FromNodeString(string str)
        {
            int index = Array.IndexOf(mapping, str);

            return index >= 0 ? (AutomixGroup)index : AutomixGroup.Off;
        }
    }
}
