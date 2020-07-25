using System;

namespace Suhock.Osc
{
    public class OscBlobArgument : OscArgument<byte[]>
    {
        public const char TypeTagChar = 'b';

        public OscBlobArgument() : this(Array.Empty<byte>()) { }

        public OscBlobArgument(ReadOnlySpan<byte> bytes) : base(TypeTagChar)
        {
            Value = bytes.ToArray();
        }

        public OscBlobArgument(ReadOnlySpan<byte> bytes, out int bytesRead) :
            this(OscUtil.ReadBlob(bytes, out bytesRead))
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
