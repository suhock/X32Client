using System;
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
                    int firstParamIndex = index;

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
            string str = Address;

            foreach (IX32Parameter parameter in Parameters)
            {
                str += " " + parameter.ToString();
            }

            return str;
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
                return Value;
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
                return Value.ToString();
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
                return Value.ToString();
            }
        }

        private static IX32Parameter DecodeParameterFromMessage(char typeCode, byte[] bytes, ref int index)
        {
            int firstIndex = index;

            switch (typeCode)
            {
                case X32StringParameter.TypeCode:
                    for (; index < bytes.Length; index++)
                    {
                        if (bytes[index] == 0)
                        {
                            break;
                        }
                    }

                    var count = index - firstIndex;
                    index = EncodedIncrement(index);

                    return new X32StringParameter
                    {
                        Value = Encoding.ASCII.GetString(bytes, firstIndex, count)
                    };

                case X32IntParameter.TypeCode:
                    byte[] intStr = new byte[4];
                    Array.ConstrainedCopy(bytes, firstIndex, intStr, 0, 4);
                    index += 4;

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(intStr);
                    }

                    return new X32IntParameter
                    {
                        Value = BitConverter.ToInt32(intStr)
                    };

                case X32FloatParameter.TypeCode:
                    byte[] floatStr = new byte[4];
                    Array.ConstrainedCopy(bytes, firstIndex, floatStr, 0, 4);
                    index += 4;

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(floatStr);
                    }

                    return new X32FloatParameter
                    {
                        Value = BitConverter.ToSingle(floatStr)
                    };

                default:
                    throw new NotSupportedException("Unknown code type");
            }
        }

        private static int EncodedIncrement(int value)
        {
            // encoded length is the first multiple of four greater than the string length
            return ((value + 4) & (int)0x7ffffffc);
        }
    }
}
