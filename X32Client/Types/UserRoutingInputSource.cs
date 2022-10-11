using System;
using System.Collections.Generic;
using System.Linq;

namespace Suhock.X32.Types;

public sealed class UserRoutingInputSource
{
    public int Value { get; }

    private static readonly Dictionary<int, UserRoutingInputSource> Instances = new();

    private UserRoutingInputSource(int value)
    {
        Value = value;
    }

    public static UserRoutingInputSource FromInt(int value) => Get(1, 208, value);

    public static UserRoutingInputSource Off => Get(0);
    public static UserRoutingInputSource LocalIn(int offset) => Get(1, 32, offset);
    public static UserRoutingInputSource Aes50A(int offset) => Get(33, 48, offset);
    public static UserRoutingInputSource Aes50B(int offset) => Get(81, 48, offset);
    public static UserRoutingInputSource CardIn(int offset) => Get(129, 32, offset);
    public static UserRoutingInputSource AuxIn(int offset) => Get(161, 6, offset);
    public static UserRoutingInputSource TbInternal => Get(167);
    public static UserRoutingInputSource TbExternal => Get(168);

    private static UserRoutingInputSource Get(int startValue, int valueRange = 1, int offset = 1)
    {
        if (offset < 0 || offset > valueRange)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, $"Must be between 0 and {valueRange}");
        }

        var value = startValue + offset - 1;

        return !Instances.ContainsKey(value) 
            ? Instances[value] = new UserRoutingInputSource(value) 
            : Instances[value];
    }
}