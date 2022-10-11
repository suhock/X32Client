using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Suhock.X32.Routing;

public static class X32Util
{
    public static string ConvertUserInIndexToString(int index)
    {
        return index switch
        {
            > 168 => "Unknown",
            168 => "TBExt",
            167 => "TBInt",
            > 160 => "Aux" + (index - 160),
            > 128 => "C" + (index - 128).ToString().PadLeft(2, '0'),
            > 80 => "B" + (index - 80).ToString().PadLeft(2, '0'),
            > 32 => "A" + (index - 32).ToString().PadLeft(2, '0'),
            > 0 => "In" + index.ToString().PadLeft(2, '0'),
            _ => ""
        };
    }

    public static int ConvertStringToUserInIndex(string value)
    {
        Match m;

        if ((m = Regex.Match(value, @"^In(\d\d)$")).Value.Length > 0)
        {
            return int.Parse(m.Groups[1].Value); // Local input
        }

        if ((m = Regex.Match(value, @"^A(\d\d)$")).Value.Length > 0)
        {
            return int.Parse(m.Groups[1].Value) + 32; // AES50-A offset
        }
        
        if ((m = Regex.Match(value, @"^B(\d\d)$")).Value.Length > 0)
        {
            return int.Parse(m.Groups[1].Value) + 80; // AES50-B offset
        }
        
        if ((m = Regex.Match(value, @"^C(\d\d)$")).Value.Length > 0)
        {
            return int.Parse(m.Groups[1].Value) + 128; // Card offset
        }
        
        if ((m = Regex.Match(value, @"^Aux(\d\d)$")).Value.Length > 0)
        {
            return int.Parse(m.Groups[1].Value) + 160; // Auxin offset
        }
        
        return 0;
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
        var index = 0;
        var result = new HashSet<int>();

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

        var result = 0;

        foreach (var value in set)
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

        var mask = 1 << (value - 1);

        return on ? (bitSet | mask) : (bitSet & ~mask);
    }

}