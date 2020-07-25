using System;
using System.Collections.Generic;
using System.Text;

namespace Suhock.Osc
{
    public class OscMessage
    {
        public string Address { get; set; }

        public List<OscArgument> Arguments { get; }

        public OscMessage(string address, List<OscArgument> arguments)
        {
            Address = address;
            Arguments = new List<OscArgument>(arguments);
        }

        public OscMessage(string address, params OscArgument[] arguments)
        {
            Address = address;
            Arguments = new List<OscArgument>(arguments);
        }

        public OscMessage(string address) : this(address, Array.Empty<OscArgument>()) { }

        public OscMessage(string address, string value) : this(address, new OscStringArgument(value)) { }

        public OscMessage(string address, int value) : this(address, new OscIntArgument(value)) { }

        public OscMessage(string address, float value) : this(address, new OscFloatArgument(value)) { }

        public OscMessage(string address, params object[] values)
        {
            Address = address;
            Arguments = new List<OscArgument>(values.Length);

            foreach (object value in values)
            {
                OscArgument arg = OscArgument.Factory.FromValue(value);

                if (arg == null)
                { 
                    throw new Exception("Invalid type: " + value.GetType().ToString());
                }

                Arguments.Add(arg);
            }
        }

        public OscMessage(ReadOnlySpan<byte> bytes): this(bytes, out _) { }

        public OscMessage(ReadOnlySpan<byte> bytes, out int length)
        {
            Address = OscUtil.ReadString(bytes, out int partLength);
            length = partLength;
            bytes = bytes[partLength..];

            string typeTagString = OscUtil.ReadString(bytes, out partLength);
            
            if (typeTagString.Length == 0 || typeTagString[0] != ',')
            {
                return;
            }

            length += partLength;

            Arguments = new List<OscArgument>(typeTagString.Length - 1);

            for (int i = 1; i < typeTagString.Length; i++)
            {
                bytes = bytes[partLength..];
                OscArgument arg = OscArgument.Factory.FromBytes(typeTagString[i], bytes, out partLength);

                if (arg == null)
                {
                    throw new InvalidOperationException("Invalid message");
                }

                Arguments.Add(arg);
                length += partLength;
            }
        }

        public string GetTypeTagString()
        {
            StringBuilder tags = new StringBuilder(Arguments.Count + 1);

            tags.Append(',');

            for (int i = 0; i < Arguments.Count; i++)
            {
                tags.Append(Arguments[i].TypeTag);
            }

            return tags.ToString();
        }

        public T GetValue<T>(int index)
        {
            if (index > Arguments.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return ((OscArgument<T>)Arguments[index]).Value;
        }

        public int WriteBytes(Span<byte> target)
        {
            int totalLength = 0;

            int length = OscUtil.WriteString(target, Address);
            totalLength += length;
            target = target[length..];

            length = OscUtil.WriteString(target, GetTypeTagString());
            totalLength += length;

            foreach (OscArgument arg in Arguments)
            {
                target = target[length..];
                length = arg.WriteBytes(target);
                totalLength += length;
            }

            return totalLength;
        }

        public int GetByteCount()
        {
            int length = OscUtil.AlignOffset(Address.Length + 1) + OscUtil.AlignOffset(Arguments.Count + 1);

            foreach (OscArgument arg in Arguments)
            {
                length += arg.GetByteCount();
            }

            return length;
        }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[GetByteCount()];
            WriteBytes(bytes);

            return bytes;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Address).Append(' ').Append(GetTypeTagString());

            foreach (OscArgument arg in Arguments)
            {
                sb.Append(' ').Append(arg.ToString());
            }

            return sb.ToString();
        }
    }
}
