using Suhock.X32.Util;
using System;

namespace Suhock.X32.Client.Message
{
    public class X32IntParameter : IX32Parameter
    {
        public const char Code = 'i';

        public char TypeCode { get; } = Code;


        public int Value { get; set; }

        public X32IntParameter() : this(0) { }

        public X32IntParameter(int value)
        {
            Value = value;
        }

        public X32IntParameter(byte[] bytes, int startIndex, out int length) :
            this(X32Util.ReadBigEndianInt(bytes, startIndex, out length))
        { }

        public int EncodedLength { get { return 4; } }

        public byte[] EncodedValue
        {
            get
            {
                byte[] bytes = BitConverter.GetBytes(Value);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }

                return bytes;
            }
        }

        public override string ToString()
        {
            return '[' + Value.ToString() + ']';
        }
    }
}
