using System;

namespace Suhock.Osc
{
    public class OscIntArgument : OscArgument<int>
    {
        public const byte TypeTagChar = (byte)'i';

        public OscIntArgument() : this(0) { }

        public OscIntArgument(int value) : base(TypeTagChar, value)
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
            return OscUtil.WriteInt(bytes, Value);
        }
    }
}
