using System;

namespace Suhock.Osc
{
    public class OscStringArgument : OscArgument<string>
    {
        public const byte TypeTagChar = (byte)'s';

        public OscStringArgument() : this("") { }

        public OscStringArgument(string value) : base(TypeTagChar, value)
        {
            Value = value;
        }

        public override int GetByteCount()
        {
            return OscUtil.AlignOffset(Value.Length + 1);
        }

        public override byte[] GetBytes()
        {
            byte[] bytes = new byte[GetByteCount()];
            WriteBytes(bytes);

            return bytes;
        }

        public override int WriteBytes(Span<byte> bytes)
        {
            return OscUtil.WriteString(bytes, Value);
        }

        public override string ToString()
        {
            return '"' + Value + '"';
        }
    }
}
