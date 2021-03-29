using System;

namespace Suhock.Osc.Arguments
{
    public class OscFloatArgument : OscArgument<float>
    {
        public const byte TypeTagChar = (byte)'f';

        public OscFloatArgument() : this(0.0f) { }

        public OscFloatArgument(float value) : base(TypeTagChar, value)
        {
            Value = value;
        }

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
