using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Suhock.X32.Util
{
    public static class X32Util
    {
        private static byte[] ReadBigEndianBytes(byte[] bytes, int startIndex, int length)
        {
            byte[] buf = new byte[length];
            Array.ConstrainedCopy(bytes, startIndex, buf, 0, length);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buf);
            }

            return buf;
        }

        public static float ConvertFloatToFaderLevel(float value)
        {
            if (value >= 0.5f)
            {
                return 40.0f * Math.Min(value, 1.0f) - 30.0f;
            }
            else if (value >= 0.25f)
            {
                return 80.0f * value - 50.0f;
            }
            else if (value >= 0.0625f)
            {
                return 160.0f * value - 70.0f;
            }
            else if (value > 0.0f)
            {
                return 480.0f * value - 90.0f;
            }
            else
            {
                return float.NegativeInfinity;
            }
        }

        public static float ConvertFaderLevelToFloat(float db)
        {
            float value;

            if (db >= -10.0f)
            {
                value = (Math.Min(db, 10.0f) + 30.0f) / 40.0f;
            }
            else if (db >= -30.0f)
            {
                value = (db + 50.0f) / 80.0f;
            }
            else if (db >= -60.0f)
            {
                value = (db + 70.0f) / 160.0f;
            }
            else if (db > -90.0f)
            {
                value = (db + 90.0f) / 480.0f;
            }
            else
            {
                value = 0.0f;
            }

            return (float)Math.Floor(value * 1023.5f) / 1023.0f;
        }

        public static float ConvertFloatToLinear(float value, float rangeMin, float rangeMax, float step)
        {
            return value * (rangeMax - rangeMin) + rangeMin;
        }

        public static float ConvertLinearToFloat(float value, float rangeMin, float rangeMax, float step)
        {
            return (Math.Max(Math.Min(value, rangeMax), rangeMin) - rangeMin) / (rangeMax - rangeMin);
        }

        public static float ConvertFloatToHeadampGain(float value)
        {
            return ConvertFloatToLinear(value, -12.0f, 60.0f, 0.5f);
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
    }
}