using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Suhock.X32.Util
{
    public static class X32Util
    {
        public static float ConvertFloatToHeadampGain(float value)
        {
            return FloatConversions.AlignEncoded(FloatConversions.EncodedToLinear(value, -12.0f, 60.0f), 145);
        }

        public static string ConvertUserInIndexToString(int index)
        {
            if (index > 168)
            {
                return "Unknown";
            }
            else if (index == 168)
            {
                return "TBExt";
            }
            else if (index == 167)
            {
                return "TBInt";
            }
            else if (index > 160)
            {
                return "Aux" + (index - 160);
            }
            else if (index > 128)
            {
                return "C" + (index - 128).ToString().PadLeft(2, '0');
            }
            else if (index > 80)
            {
                return "B" + (index - 80).ToString().PadLeft(2, '0');
            }
            else if (index > 32)
            {
                return "A" + (index - 32).ToString().PadLeft(2, '0');
            }
            else if (index > 0)
            {
                return "In" + index.ToString().PadLeft(2, '0');
            }
            else
            {
                return "";
            }
        }

        public static int ConvertStringToUserInIndex(string value)
        {
            Match m;

            if ((m = Regex.Match(value, @"^In(\d\d)$")).Value.Length > 0)
            {
                return int.Parse(m.Groups[1].Value); // Local input
            }
            else if ((m = Regex.Match(value, @"^A(\d\d)$")).Value.Length > 0)
            {
                return int.Parse(m.Groups[1].Value) + 32; // AES50-A offset
            }
            else if ((m = Regex.Match(value, @"^B(\d\d)$")).Value.Length > 0)
            {
                return int.Parse(m.Groups[1].Value) + 80; // AES50-B offset
            }
            else if ((m = Regex.Match(value, @"^C(\d\d)$")).Value.Length > 0)
            {
                return int.Parse(m.Groups[1].Value) + 128; // Card offset
            }
            else if ((m = Regex.Match(value, @"^Aux(\d\d)$")).Value.Length > 0)
            {
                return int.Parse(m.Groups[1].Value) + 160; // Auxin offset
            }
            else
            {
                return 0;
            }
        }

        public static int ConvertUserInIndexToHeadampIndex(int index)
        {
            if (index >= 1 && index <= 128)
            {
                return index - 1;
            }
            else
            {
                return -1;
            }
        }

        public static HashSet<int> ConvertBitSetToSet(int bitSet)
        {
            int index = 0;
            HashSet<int> result = new HashSet<int>();

            while (bitSet > 0)
            {
                ++index;

                if ((bitSet & 1) == 1)
                {
                    result.Add(index);
                }

                bitSet >>= 1;
            }

            return result;
        }

        public static int ConvertSetToBitSet(ICollection<int> set, int maxValue)
        {
            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            int result = 0;

            foreach (int value in set)
            {
                if (value < 1 || value > maxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(set), value, "Must be between 1 and " + maxValue);
                }

                result |= 1 << (value - 1);
            }

            return result;
        }

        public static int SetBitSetIndexOn(int bitSet, int value, int maxValue, bool on)
        {
            if (value < 1 || value > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Must be between 1 and " + maxValue);
            }

            int mask = 1 << (value - 1);

            return on ? (bitSet | mask) : (bitSet & ~mask);
        }

    }
}