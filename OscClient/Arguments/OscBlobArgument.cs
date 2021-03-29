using System;

namespace Suhock.Osc.Arguments
{
    public class OscBlobArgument : OscArgument<byte[]>
    {
        public const byte TypeTagChar = (byte)'b';

        public OscBlobArgument() : this(Array.Empty<byte>()) { }

        public OscBlobArgument(ReadOnlySpan<byte> bytes) : base(TypeTagChar, bytes.ToArray())
        { }

        public override int GetByteCount()
        {
            return OscUtil.AlignOffset(4 + Value.Length);
        }

        public override byte[] GetBytes()
        {
            byte[] bytes = new byte[GetByteCount()];
            WriteBytes(bytes);

            return bytes;
        }

        public override int WriteBytes(Span<byte> bytes)
        {
            return OscUtil.WriteBlob(bytes, Value);
        }

        public override string ToString()
        {
            return "[blob:" + Value.Length + ']';
        }
    }
}
