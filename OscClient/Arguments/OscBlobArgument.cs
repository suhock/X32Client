using System;

namespace Suhock.Osc.Arguments;

public sealed class OscBlobArgument : OscArgument<byte[]>
{
    public const byte TypeTagByte = (byte)'b';

    public OscBlobArgument() : this(Array.Empty<byte>()) { }

    public OscBlobArgument(ReadOnlySpan<byte> bytes) : base(bytes.ToArray())
    { }

    public override byte TypeTag => TypeTagByte;

    public override int GetByteCount()
    {
        return OscUtil.AlignOffset(4 + Value.Length);
    }

    public override byte[] GetBytes()
    {
        var bytes = new byte[GetByteCount()];
        WriteBytes(bytes);

        return bytes;
    }

    public override int WriteBytes(Span<byte> target)
    {
        return OscUtil.WriteBlob(target, Value);
    }

    public override string ToString()
    {
        return "[blob:" + Value.Length + ']';
    }
}