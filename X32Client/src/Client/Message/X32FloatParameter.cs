using Suhock.X32.Util;
using System;

namespace Suhock.X32.Client.Message
{
    public class X32FloatParameter : IX32Parameter
    {
        public const char Code = 'f';

        public char TypeCode { get; } = Code;

        public float Value { get; set; }

        public X32FloatParameter() : this(0.0f) { }

        public X32FloatParameter(float value)
        {
            Value = value;
        }

        public X32FloatParameter(byte[] bytes, int startIndex, out int length) :
            this(X32Util.ReadBigEndianFloat(bytes, startIndex, out length))
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
