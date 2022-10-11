using System;

namespace Suhock.Osc.Arguments;

public sealed class OscIntArgument : OscArgument<int>
{
    public const byte TypeTagByte = (byte)'i';

    public OscIntArgument() : this(0) { }

    public OscIntArgument(int value) : base(value)
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
        return OscUtil.WriteInt(target, Value);
    }
}