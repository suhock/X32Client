using System;

namespace Suhock.Osc
{
    public class OscMessageFactory: IOscMessageFactory
    {
        public OscMessageFactory(IOscArgumentFactory argumentFactory)
        {
            ArgumentFactory = argumentFactory;
        }

        public IOscArgumentFactory ArgumentFactory { get; }

        public OscMessage Create(string address)
        {
            return new OscMessage(address);
        }

        public OscMessage Create(string address, params object[] args)
        {
            OscMessage msg = new OscMessage(address);

            foreach (object value in args)
            {
                IOscArgument arg = ArgumentFactory.FromValue(value);

                if (arg == null)
                {
                    throw new Exception("Invalid type: " + value.GetType().ToString());
                }

                msg.Arguments.Add(arg);
            }

            return msg;
        }

        public OscMessage Create(ReadOnlySpan<byte> bytes)
        {
            return Create(bytes, out _);
        }

        public OscMessage Create(ReadOnlySpan<byte> bytes, out int length)
        {
            string address = OscUtil.ReadString(bytes, out int partLength);
            length = partLength;
            bytes = bytes[partLength..];

            OscMessage msg = new OscMessage(address);
            ReadOnlySpan<byte> typeTagString = OscUtil.ReadByteString(bytes, out partLength);

            if (typeTagString.Length > 0 && typeTagString[0] == (byte)',')
            {
                length += partLength;

                for (int i = 1; i < typeTagString.Length; i++)
                {
                    bytes = bytes[partLength..];
                    IOscArgument arg = ArgumentFactory.FromBytes(typeTagString[i], bytes, out partLength);

                    if (arg == null)
                    {
                        throw new InvalidOperationException("Invalid message");
                    }

                    msg.Arguments.Add(arg);
                    length += partLength;
                }
            }

            return msg;
        }
    }
}
