using System;

namespace Suhock.Osc
{
    public class OscStringArgument : OscArgument<string>
    {
        public const char TypeTagChar = 's';

        public OscStringArgument() : this("") { }

        public OscStringArgument(string value) : base(TypeTagChar)
        {
            Value = value;
        }

        public OscStringArgument(ReadOnlySpan<byte> bytes, out int bytesRead) :
            this(OscUtil.ReadString(bytes, out bytesRead))
        { }

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
