using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Suhock.X32Stream
{
    public class X32Message
    {
        public string Address { get; set; }
        public IX32Parameter[] Parameters { get; set; }

        public X32Message(string address, IX32Parameter[] parameters)
        {
            Address = address;
            Parameters = parameters;
        }

        public X32Message(string address) : this(address, new IX32Parameter[0]) { }

        public static X32Message Decode(byte[] message)
        {
            int index;

            for (index = 0; index < message.Length; index++)
            {
                if (message[index] == 0)
                {
                    break;
                }
            }

            string address = Encoding.ASCII.GetString(message, 0, index);
            index = EncodedIncrement(index);

            if (index < message.Length && message[index++] == (byte)',')
            {
                int firstParamTypeIndex = index;

                for (; index < message.Length; index++)
                {
                    if (message[index] == 0)
                    {
                        break;
                    }
                }

                IX32Parameter[] parameters = new IX32Parameter[index - firstParamTypeIndex];
                index = EncodedIncrement(index);

                for (int i = 0; i < parameters.Length; i++)
                {
                    char typeCode = (char)message[firstParamTypeIndex + i];
                    parameters[i] = DecodeParameterFromMessage(typeCode, message, ref index);
                }

                return new X32Message(address, parameters);
            }
            else
            {
                return new X32Message(address);
            }
        }

        public byte[] Encode()
        {
            int addressLength = EncodedIncrement(this.Address.Length);
            int typesLength = EncodedIncrement(Parameters.Length + 1);
            int length = addressLength + typesLength;

            foreach (IX32Parameter parameter in this.Parameters)
            {
                length += parameter.EncodedLength;
            }

            byte[] buffer = new byte[length];

            Buffer.BlockCopy(Encoding.ASCII.GetBytes(Address), 0, buffer, 0, Address.Length);

            int typesIndex = addressLength;
            int paramIndex = addressLength + typesLength;

            buffer[typesIndex++] = (byte)',';

            foreach (IX32Parameter parameter in this.Parameters)
            {
                buffer[typesIndex++] = (byte)parameter.TypeChar;
                Buffer.BlockCopy(parameter.EncodedValue, 0, buffer, paramIndex, parameter.EncodedLength);
                paramIndex += parameter.EncodedLength;
            }

            return buffer;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Address);

            if (Parameters.Length > 0)
            {
                sb.Append(" ,");

                foreach (IX32Parameter parameter in Parameters)
                {
                    sb.Append(parameter.TypeChar);
                }

                foreach (IX32Parameter parameter in Parameters)
                {
                    sb.Append(" ");
                    sb.Append(parameter.ToString());
                }
            }

            return sb.ToString();
        }

        public interface IX32Parameter
        {
            char TypeChar { get; }
            int EncodedLength { get; }
            byte[] EncodedValue { get; }
        }

        public class X32StringParameter : IX32Parameter
        {
            public const char TypeCode = 's';

            public string Value { get; set; }

            public char TypeChar { get { return TypeCode; } }

            public int EncodedLength
            {
                get
                {
                    return EncodedIncrement(Value.Length);
                }
            }

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

        public class X32IntParameter : IX32Parameter
        {
            public const char TypeCode = 'i';

            public int Value { get; set; }

            public char TypeChar { get { return TypeCode; } }

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

        public class X32FloatParameter : IX32Parameter
        {
            public const char TypeCode = 'f';

            public float Value { get; set; }

            public char TypeChar { get { return TypeCode; } }

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

        public class X32BlobParameter : IX32Parameter
        {
            public const char TypeCode = 'b';

            public byte[] Value { get; set; }

            public char TypeChar { get { return TypeCode; } }

            public int EncodedLength { get { return EncodedIncrement(Value.Length);  } }

            public byte[] EncodedValue
            {
                get
                {
                    if (Value.Length % 4 == 0)
                    {
                        return Value;
                    }

                    byte[] bytes = new byte[EncodedLength];
                    Array.Copy(Value, bytes, Value.Length);

                    return bytes;
                }
            }

            public override string ToString()
            {
                return '[' + BitConverter.ToString(Value).Replace("-", "") + ']';
            }
        }

        private static IX32Parameter DecodeParameterFromMessage(char typeCode, byte[] bytes, ref int index)
        {
            switch (typeCode)
            {
                case X32StringParameter.TypeCode:
                    return new X32StringParameter
                    {
                        Value = ReadString(bytes, ref index)
                    };

                case X32IntParameter.TypeCode:
                    return new X32IntParameter
                    {
                        Value = ReadBigEndianInt(bytes, ref index)
                    };

                case X32FloatParameter.TypeCode:
                    return new X32FloatParameter
                    {
                        Value = ReadBigEndianFloat(bytes, ref index)
                    };

                case X32BlobParameter.TypeCode:
                    return new X32BlobParameter
                    {
                        Value = ReadBlob(bytes, ref index)
                    };

                default:
                    throw new NotSupportedException("Unknown type: " + typeCode);
            }
        }

        private static string ReadString(byte[] bytes, ref int index)
        {
            int startIndex = index;

            for (; index < bytes.Length; index++)
            {
                if (bytes[index] == 0)
                {
                    break;
                }
            }

            var count = index - startIndex;
            index = EncodedIncrement(index);

            return Encoding.ASCII.GetString(bytes, startIndex, count);
        }

        private static int ReadBigEndianInt(byte[] bytes, ref int index)
        {
            return BitConverter.ToInt32(ReadBigEndianBytes(4, bytes, ref index));
        }

        private static float ReadBigEndianFloat(byte[] bytes, ref int index)
        {
            return BitConverter.ToSingle(ReadBigEndianBytes(4, bytes, ref index));
        }

        private static byte[] ReadBigEndianBytes(int length, byte[] bytes, ref int index)
        {
            byte[] buf = new byte[length];
            Array.ConstrainedCopy(bytes, index, buf, 0, length);
            index += length;

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buf);
            }

            return buf;
        }

        private static byte[] ReadBlob(byte[] bytes, ref int index)
        {
            int length = ReadBigEndianInt(bytes, ref index);
            byte[] result = new byte[length];

            Array.ConstrainedCopy(bytes, index, result, 0, length);

            return result;
        }

        private static int EncodedIncrement(int value)
        {
            // encoded length is the first multiple of four greater than the string length
            return ((value + 4) & (int)0x7ffffffc);
        }
    }
}
