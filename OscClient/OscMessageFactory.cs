using System;
using System.Collections.Generic;

namespace Suhock.Osc
{
    public class OscMessageFactory: IOscMessageFactory
    {
        public OscMessageFactory(IOscArgumentFactory argumentFactory)
        {
            ArgumentFactory = argumentFactory;
        }

        public IOscArgumentFactory ArgumentFactory { get; }

        /// <summary>
        /// Convenience method for creating a zero-argument OSC message
        /// </summary>
        /// <param name="address">The OSC address of the message</param>
        /// <returns>An <see cref="OscMessage"/> object with the specified address</returns>
        public OscMessage Create(string address)
        {
            return new OscMessage(address);
        }

        /// <inheritdoc/>
        public OscMessage Create(string address, params object[] args)
        {
            List<IOscArgument> argList = new List<IOscArgument>(args.Length);

            foreach (object value in args)
            {
                IOscArgument arg = ArgumentFactory.FromValue(value);

                if (arg == null)
                {
                    throw new Exception("Invalid type: " + value.GetType().ToString());
                }

                argList.Add(arg);
            }

            return new OscMessage(address, argList);
        }

        public OscMessage Create(ReadOnlySpan<byte> bytes)
        {
            return Create(bytes, out _);
        }

        /// <inheritdoc/>
        public OscMessage Create(ReadOnlySpan<byte> bytes, out int length)
        {
            ReadOnlySpan<byte> segmentStart = bytes;
            string address = OscUtil.ReadString(segmentStart, out int segmentLength);
            length = segmentLength;
            segmentStart = segmentStart[segmentLength..];

            ReadOnlySpan<byte> typeTagString = OscUtil.ReadByteString(segmentStart, out segmentLength);

            if (typeTagString.Length > 0 && typeTagString[0] == (byte)',')
            {
                // Only increase the length of the bytes used if this segment is a valid type tag string
                length += segmentLength;
            }

            if (typeTagString.Length <= 1 || typeTagString[0] != (byte)',')
            {
                // Address was followed by: nothing, a type tag string specifying no arguments, or a non-standard
                // value.
                // Return a message with no arguments.
                return new OscMessage(address);
            }

            List<IOscArgument> argList = new List<IOscArgument>(typeTagString.Length - 1);

            for (int i = 1; i < typeTagString.Length; i++)
            {
                segmentStart = segmentStart[segmentLength..];
                IOscArgument arg = ArgumentFactory.FromBytes(typeTagString[i], segmentStart, out segmentLength);

                if (arg == null)
                {
                    throw new InvalidOperationException("Invalid message");
                }

                argList.Add(arg);
                length += segmentLength;
            }

            return new OscMessage(address, argList);
        }
    }
}
