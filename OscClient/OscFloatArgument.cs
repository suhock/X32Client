using System;

namespace Suhock.Osc
{
    public class OscFloatArgument : OscArgument<float>
    {
        public const char TypeTagChar = 'f';

        public OscFloatArgument() : this(0.0f) { }

        public OscFloatArgument(float value) : base(TypeTagChar)
        {
            Value = value;
        }
        public OscFloatArgument(ReadOnlySpan<byte> bytes, out int bytesRead) :
            this(OscUtil.ReadFloat(bytes, out bytesRead))
        { }

        public override int GetByteCount()
        {
            return 4;
        }

        public override byte[] GetBytes()
        {
            byte[] bytes = new byte[4];
            WriteBytes(bytes);

            return bytes;
        }

        public override int WriteBytes(Span<byte> bytes)
        {
            return OscUtil.WriteFloat(bytes, Value);
        }
    }
}
