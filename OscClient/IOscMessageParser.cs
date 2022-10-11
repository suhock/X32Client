using System;

namespace Suhock.Osc;

public interface IOscMessageParser
{
    /// <summary>
    /// Creates an <see cref="OscMessage"/> object from a binary OSC packet.
    /// </summary>
    /// <param name="bytes">A span of bytes containing the OSC packet content</param>
    /// <param name="length">The number of bytes read from the <paramref name="bytes"/> parameter while
    /// constructing the <see cref="OscMessage"/>object</param>
    /// <returns>An <see cref="OscMessage"/> object corresponding to the binary OSC packet</returns>
    public OscMessage ParseBytes(ReadOnlySpan<byte> bytes, out int length);
}