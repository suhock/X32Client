using System;
using System.Collections.Generic;

namespace Suhock.X32.Types;

public sealed class UserRoutingOutputSource
{
    public int Value { get; }

    private static readonly Dictionary<int, UserRoutingOutputSource> Instances = new();

    private UserRoutingOutputSource(int value)
    {
        Value = value;
    }

    public static UserRoutingOutputSource FromInt(int value) => Get(1, 208, value);

    public static UserRoutingOutputSource Off => Get(0);
    public static UserRoutingOutputSource LocalIn(int offset) => Get(1, 32, offset);
    public static UserRoutingOutputSource Aes50A(int offset) => Get(33, 48, offset);
    public static UserRoutingOutputSource Aes50B(int offset) => Get(81, 48, offset);
    public static UserRoutingOutputSource CardIn(int offset) => Get(129, 32, offset);
    public static UserRoutingOutputSource AuxIn(int offset) => Get(161, 6, offset);
    public static UserRoutingOutputSource TbInternal => Get(167);
    public static UserRoutingOutputSource TbExternal => Get(168);
    public static UserRoutingOutputSource Output(int offset) => Get(169, 16, offset);
    public static UserRoutingOutputSource P16(int offset) => Get(185, 16, offset);
    public static UserRoutingOutputSource AuxOut(int offset) => Get(201, 6, offset);
    public static UserRoutingOutputSource MonitorLeft => Get(207);
    public static UserRoutingOutputSource MonitorRight => Get(208);

    private static UserRoutingOutputSource Get(int startValue, int valueRange = 1, int offset = 1)
    {
        if (offset < 0 || offset > valueRange)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, $"Must be between 0 and {valueRange}");
        }

        var value = startValue + offset - 1;

        return !Instances.ContainsKey(value) 
            ? Instances[value] = new UserRoutingOutputSource(value) 
            : Instances[value];
    }
}