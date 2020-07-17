using Suhock.X32.Util;
using System;

namespace Suhock.X32.Client.Message
{
    public class X32BlobParameter : IX32Parameter
    {
        public const char Code = 'b';

        public char TypeCode { get; } = Code;

        public byte[] Value { get; set; }

        public X32BlobParameter() : this(new byte[0]) { }

        public X32BlobParameter(byte[] value)
        {
            Value = value;
        }

        public X32BlobParameter(byte[] bytes, int startIndex, out int length) :
            this(X32Util.ReadBlob(bytes, startIndex, out length))
        { }

        public int EncodedLength { get { return X32Util.EncodedIncrement(Value.Length); } }

        public byte[] EncodedValue
        {
            get
            {
                var length = EncodedLength;
                byte[] blob = new byte[length + 4];

                BitConverter.TryWriteBytes(blob, length);

                if (BitConverter.IsLittleEndian)
                {
                    blob.AsSpan(0, 4).Reverse();
                }

                Array.Copy(Value, 0, blob, 4, Value.Length);

                return blob;
            }
        }

        public override string ToString()
        {
            return '[' + BitConverter.ToString(Value).Replace("-", "") + ']';
        }
    }
}