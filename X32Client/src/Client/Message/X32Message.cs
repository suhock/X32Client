using Suhock.X32.Util;
using System;
using System.Text;

namespace Suhock.X32.Client.Message
{
    public partial class X32Message
    {
        public string Address { get; set; }
        public IX32Parameter[] Parameters { get; set; }

        public X32Message(string address, params IX32Parameter[] parameters)
        {
            Address = address;
            Parameters = parameters;
        }

        public X32Message(string address) : this(address, new IX32Parameter[0]) { }

        public X32Message(string address, string value) : this(address, new X32StringParameter(value)) { }

        public X32Message(string address, int value) : this(address, new X32IntParameter(value)) { }

        public X32Message(string address, float value) : this(address, new X32FloatParameter(value)) { }

        public X32Message(string address, params object[] parameters) : this(address)
        {
            Parameters = new IX32Parameter[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                System.Type t = parameters[i].GetType();

                if (t.Equals(typeof(string)))
                {
                    Parameters[i] = new X32StringParameter((string)parameters[i]);
                }
                if (t.Equals(typeof(int)) || t.IsEnum)
                {
                    Parameters[i] = new X32IntParameter((int)parameters[i]);
                }
                else if (t.Equals(typeof(float)))
                {
                    Parameters[i] = new X32FloatParameter((float)parameters[i]);
                }
                else if (t.Equals(typeof(byte[])))
                {
                    Parameters[i] = new X32BlobParameter((byte[])parameters[i]);
                }
                else
                {
                    throw new Exception("Invalid type: " + parameters[i].GetType().ToString());
                }
            }
        }

        public X32Message(byte[] bytes)
        {
            int index;

            for (index = 0; index < bytes.Length; index++)
            {
                if (bytes[index] == 0)
                {
                    break;
                }
            }

            string address = Encoding.ASCII.GetString(bytes, 0, index);
            index = X32Util.EncodedIncrement(index);

            if (index < bytes.Length && bytes[index++] == (byte)',')
            {
                int firstParamTypeIndex = index;

                for (; index < bytes.Length; index++)
                {
                    if (bytes[index] == 0)
                    {
                        break;
                    }
                }

                IX32Parameter[] parameters = new IX32Parameter[index - firstParamTypeIndex];
                index = X32Util.EncodedIncrement(index);

                for (int i = 0; i < parameters.Length; i++)
                {
                    char typeCode = (char)bytes[firstParamTypeIndex + i];
                    parameters[i] = IX32Parameter.FromMessageBytes(typeCode, bytes, index, out int length);
                    index += length;
                }

                Address = address;
                Parameters = parameters;
            }
            else
            {
                Address = address;
            }
        }

        public byte[] ToBytes()
        {
            int addressLength = X32Util.EncodedIncrement(Address.Length);
            int typesLength = X32Util.EncodedIncrement(Parameters.Length + 1);
            int length = addressLength + typesLength;

            foreach (IX32Parameter parameter in Parameters)
            {
                length += parameter.EncodedLength;
            }

            byte[] buffer = new byte[length];

            Buffer.BlockCopy(Encoding.ASCII.GetBytes(Address), 0, buffer, 0, Address.Length);

            int typesIndex = addressLength;
            int paramIndex = addressLength + typesLength;

            buffer[typesIndex++] = (byte)',';

            foreach (IX32Parameter parameter in Parameters)
            {
                buffer[typesIndex++] = (byte)parameter.TypeCode;
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
                    sb.Append(parameter.TypeCode);
                }

                foreach (IX32Parameter parameter in Parameters)
                {
                    sb.Append(" ");
                    sb.Append(parameter.ToString());
                }
            }

            return sb.ToString();
        }
    }
}
