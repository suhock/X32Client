using Suhock.X32.Util;
using System.Text;

namespace Suhock.X32.Client.Message
{
    public class X32StringParameter : IX32Parameter
    {
        public const char Code = 's';

        public char TypeCode { get; } = Code;

        public string Value { get; set; }

        public X32StringParameter() : this("") { }

        public X32StringParameter(string value)
        {
            Value = value;
        }

        public X32StringParameter(byte[] bytes, int startIndex, out int length) :
            this(X32Util.ReadString(bytes, startIndex, out length))
        { }

        public int EncodedLength { get => X32Util.EncodedIncrement(Value.Length); }

        public byte[] EncodedValue
        {
            get
            {
                int length = EncodedLength;
                byte[] bytes = new byte[length];
                Encoding.ASCII.GetBytes(Value, 0, Value.Length, bytes, 0);
                return bytes;
            }
        }

        public override string ToString()
        {
            return '"' + Value + '"';
        }

    }
}
