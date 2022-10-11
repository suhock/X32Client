using System;

namespace Suhock.Osc.Arguments;

public sealed class OscStringArgument : OscArgument<string>
{
    public const byte TypeTagByte = (byte)'s';

    public OscStringArgument() : this("") { }

    public OscStringArgument(string value) : base(value)
    {
        Value = value;
    }

    public override byte TypeTag => TypeTagByte;

    public override int GetByteCount()
    {
        return OscUtil.AlignOffset(Value.Length + 1);
    }

    public override byte[] GetBytes()
    {
        var bytes = new byte[GetByteCount()];
        WriteBytes(bytes);

        return bytes;
    }

    public override int WriteBytes(Span<byte> target)
    {
        return OscUtil.WriteString(target, Value);
    }

    public override string ToString()
    {
        return '"' + Value + '"';
    }
}