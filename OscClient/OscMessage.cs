using System;
using System.Collections.Generic;
using System.Text;

namespace Suhock.Osc
{
    public class OscMessage
    {
        public OscMessage(string address)
        {
            Address = address;
            Arguments = new List<IOscArgument>();
        }

        public OscMessage(string address, List<IOscArgument> arguments)
        {
            Address = address;
            Arguments = new List<IOscArgument>(arguments);
        }

        public OscMessage(string address, params IOscArgument[] arguments)
        {
            Address = address;
            Arguments = new List<IOscArgument>(arguments);
        }

        public string Address { get; set; }

        public List<IOscArgument> Arguments { get; }

        private byte[] GetTypeTagByteString()
        {
            byte[] bytes = new byte[Arguments.Count + 1];
            bytes[0] = (byte)',';

            for (int i = 0; i < Arguments.Count; i++)
            {
                bytes[i + 1] = Arguments[i].TypeTag;
            }

            return bytes;
        }

        public string GetTypeTagString()
        {
            return Encoding.ASCII.GetString(GetTypeTagByteString());
        }

        public T GetValue<T>(int index)
        {
            if (index >= Arguments.Count)
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

            length = OscUtil.WriteByteString(target, GetTypeTagByteString());
            totalLength += length;

            foreach (IOscArgument arg in Arguments)
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

            foreach (IOscArgument arg in Arguments)
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

            foreach (IOscArgument arg in Arguments)
            {
                sb.Append(' ').Append(arg.ToString());
            }

            return sb.ToString();
        }
    }
}
