using System;

namespace Suhock.X32.Types.Enums;

public enum Source
{
    Off,
    In01, In02, In03, In04, In05, In06, In07, In08,
    In09, In10, In11, In12, In13, In14, In15, In16,
    In17, In18, In19, In20, In21, In22, In23, In24,
    In25, In26, In27, In28, In29, In30, In31, In32,
    Aux1, Aux2, Aux3, Aux4, Aux5, Aux6, UsbL, UsbR,
    Fx1L, Fx1R, Fx2L, Fx2R, Fx3L, Fx3R, Fx4L, Fx4R,
    Bus01, Bus02, Bus03, Bus04, Bus05, Bus06, Bus07, Bus08,
    Bus09, Bus10, Bus11, Bus12, Bus13, Bus14, Bus15, Bus16
}

public static class SourceExtensions
{
    public static Source In(int index)
    {
        if (index is < 1 or > 32)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "Must be between 1 and 32");
        }
        
        return Source.In01 + index - 1;
    }
    
    public static Source Aux(int index)
    {
        if (index is < 1 or > 6)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "Must be between 1 and 6");
        }
        
        return Source.Aux1 + index - 1;
    }
    
    public static Source FxL(int index)
    {
        if (index is < 1 or > 4)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "Must be between 1 and 4");
        }
        
        return Source.Fx1L + 2 * index - 2;
    }
    
    public static Source FxR(int index)
    {
        if (index is < 1 or > 4)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "Must be between 1 and 4");
        }
        
        return Source.Fx1R + 2 * index - 2;
    }
    
    public static Source Bus(int index)
    {
        if (index is < 1 or > 16)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "Must be between 1 and 16");
        }
        
        return Source.Bus01 + index - 1;
    }
    
    public static bool IsOff(this Source source)
    {
        return source == Source.Off;
    }

    public static bool IsChannel(this Source source)
    {
        return source is >= Source.In01 and <= Source.In32;
    }

    public static bool IsAux(this Source source)
    {
        return source is >= Source.Aux1 and <= Source.UsbR;
    }

    public static bool IsFx(this Source source)
    {
        return source is >= Source.Fx1L and <= Source.Fx4R;
    }

    public static bool IsBus(this Source source)
    {
        return source >= Source.Bus01;
    }
}