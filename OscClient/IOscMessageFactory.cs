using System;

namespace Suhock.Osc
{
    public interface IOscMessageFactory
    {
        /// <summary>
        /// Creates an <c>OscMessage</c> object with the specified values mapped to their repsective OSC types.
        /// </summary>
        /// <param name="address">The OSC address</param>
        /// <param name="values">A list of natively-typed values</param>
        /// <returns>An <c>OscMessage</c> object</returns>
        public OscMessage Create(string address, params object[] values);

        /// <summary>
        /// Creates an <c>OscMessage</c> object from a binary OSC server response.
        /// </summary>
        /// <param name="bytes">A span of bytes corresponding to the OSC server datagram response</param>
        /// <param name="length">The number of bytes read from the <c>bytes</c> parameter while constructing the 
        /// <c>OscMessage</c></param>
        /// <returns>An <c>OscMessage</c> object</returns>
        public OscMessage Create(ReadOnlySpan<byte> bytes, out int length);
    }
}
