using System;

namespace Suhock.Osc
{
    public interface IOscMessageFactory
    {
        /// <summary>
        /// Creates an <see cref="OscMessage"/> object with the specified natively-typed values mapped to their
        /// repsective <see cref="OscArgument{T}"/> types.
        /// </summary>
        /// <param name="address">An OSC address</param>
        /// <param name="values">A list of natively-typed argument values</param>
        /// <returns>An <see cref="OscMessage"/> object with the specified address and argument values</returns>
        public OscMessage Create(string address, params object[] values);

        /// <summary>
        /// Creates an <see cref="OscMessage"/> object from a binary OSC packet.
        /// </summary>
        /// <param name="bytes">A span of bytes containing the OSC packet content</param>
        /// <param name="length">The number of bytes read from the <paramref name="bytes"/> parameter while
        /// constructing the <see cref="OscMessage"/>object</param>
        /// <returns>An <see cref="OscMessage"/> object corresponding to the binary OSC packet</returns>
        public OscMessage Create(ReadOnlySpan<byte> bytes, out int length);
    }
}
