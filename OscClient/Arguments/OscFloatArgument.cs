using System;

namespace Suhock.Osc.Arguments;

public sealed class OscFloatArgument : OscArgument<float>
{
    public const byte TypeTagByte = (byte)'f';

    public OscFloatArgument() : this(0.0f) { }

    public OscFloatArgument(float value) : base(value)
    {
        Value = value;
    }

    public override byte TypeTag => TypeTagByte;

    public override int GetByteCount()
    {
        return 4;
    }

    public override byte[] GetBytes()
    {
        var bytes = new byte[4];
        WriteBytes(bytes);

        return bytes;
    }

    public override int WriteBytes(Span<byte> target)
    {
        return OscUtil.WriteFloat(target, Value);
    }
}