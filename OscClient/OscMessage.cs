﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Suhock.Osc
{
    /// <summary>
    /// Encapsulates an OSC Message.
    /// </summary>
    public class OscMessage
    {
        /// <summary>
        /// Constructs an OSC message to the specified address with no OSC arguments.
        /// </summary>
        /// <param name="address">The target address</param>
        public OscMessage(string address)
        {
            Address = address;
            Arguments = new List<IOscArgument>();
        }

        /// <summary>
        /// Constructs an OSC message to the specified address with the OSC arguments specified in the provided list.
        /// </summary>
        /// <param name="address">The target address</param>
        /// <param name="arguments">The list of arguments to send with the message</param>
        public OscMessage(string address, List<IOscArgument> arguments)
        {
            Address = address;
            Arguments = new List<IOscArgument>(arguments);
        }

        /// <summary>
        /// Constructs an OSC message to the specified address with the OSC arguments specified as additional method
        /// arguments. 
        /// </summary>
        /// <param name="address">The target address</param>
        /// <param name="arguments">Arguments to send with the message</param>
        public OscMessage(string address, params IOscArgument[] arguments)
        {
            Address = address;
            Arguments = new List<IOscArgument>(arguments);
        }

        /// <summary>
        /// The target address of the message
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// The sequence of arguments to send with the message
        /// </summary>
        public IReadOnlyList<IOscArgument> Arguments { get; }

        private byte[] GetTypeTagStringBytes()
        {
            byte[] bytes = new byte[Arguments.Count + 1];
            bytes[0] = (byte)',';

            for (int i = 0; i < Arguments.Count; i++)
            {
                bytes[i + 1] = Arguments[i].TypeTag;
            }

            return bytes;
        }

        /// <summary>
        /// Returns an ASCII-encoded string representation of the argument type tag byte string.
        /// </summary>
        /// <returns>An ASCII-encoded string representation of the argument type tag byte string</returns>
        public string GetTypeTagString()
        {
            return Encoding.ASCII.GetString(GetTypeTagStringBytes());
        }

        /// <summary>
        /// Convenience function for retrieving a value of the specified type. Can only be used on arguments that
        /// inherit from the <see cref="OscArgument{T}"/> class and the type parameter <code>T</code> must match the
        /// argument's type parameter.
        /// </summary>
        /// <typeparam name="T">The type parameter corresponding to the argument value</typeparam>
        /// <param name="index">The zero-based index of the argument in the argument list</param>
        /// <returns>The value of the argument at the specified index</returns>
        public T GetArgumentValue<T>(int index)
        {
            if (index >= Arguments.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return ((OscArgument<T>)Arguments[index]).Value;
        }

        /// <summary>
        /// Writes this OSC message as a packet to the specified byte buffer.
        /// </summary>
        /// <param name="buffer">The target buffer</param>
        /// <returns>The total number of bytes written to the buffer</returns>
        public int WritePacket(Span<byte> buffer)
        {
            Span<byte> segment = buffer;
            int totalLength = 0;

            int segmentLength = OscUtil.WriteString(segment, Address);
            totalLength += segmentLength;
            segment = segment[segmentLength..];

            segmentLength = OscUtil.WriteString(segment, GetTypeTagStringBytes());
            totalLength += segmentLength;

            foreach (IOscArgument arg in Arguments)
            {
                segment = segment[segmentLength..];
                segmentLength = arg.WriteBytes(segment);
                totalLength += segmentLength;
            }

            return totalLength;
        }

        /// <summary>
        /// Returns the length of the message packet in bytes, i.e. number of bytes that would be returned by
        /// <see cref="WritePacket(Span{byte})"/>.
        /// </summary>
        /// <returns>the length of the message in bytes</returns>
        public int GetPacketLength()
        {
            int length = OscUtil.AlignOffset(Address.Length + 1) + OscUtil.AlignOffset(Arguments.Count + 1);

            foreach (IOscArgument arg in Arguments)
            {
                length += arg.GetByteCount();
            }
            
            return length;
        }

        /// <summary>
        /// Returns a byte array containing an OSC packet corresponding to this message object
        /// </summary>
        /// <returns>A byte array containing an OSC packet corresponding to this message object</returns>
        public byte[] GetPacketBytes()
        {
            byte[] bytes = new byte[GetPacketLength()];
            WritePacket(bytes);

            return bytes;
        }

        /// <summary>
        /// Returns a string version of this message.
        /// </summary>
        /// <returns>A string version of this message</returns>
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
