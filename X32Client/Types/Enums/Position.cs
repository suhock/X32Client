using System;

namespace Suhock.X32.Types.Enums
{
    public enum Position
    {
        Pre,
        Post
    }

    public static class PositionExtensions
    {
        private static readonly string[] mapping =
        {
            "PRE",
            "POST"
        };

        public static string ToNodeString(this Position type)
        {
            return mapping[(int)type];
        }

        public static Position FromNodeString(string str)
        {
            int index = Array.IndexOf(mapping, str);

            return index >= 0 ? (Position)index : Position.Pre;
        }
    }
}