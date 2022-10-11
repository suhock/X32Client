using System;

namespace Suhock.Osc;

public interface IOscArgumentParser
{
    /// <summary>
    /// Decodes a binary-encoded OSC message argument into an <c>OscMessage</c> object.
    /// </summary>
    /// <param name="typeTag">The type tag for this message from the OSC message's type tag string</param>
    /// <param name="bytes">The span of bytes starting at the beginning of the argument to be read</param>
    /// <param name="length">The number of bytes read</param>
    /// <returns>The decoded OSC message argument or <c>null</c> if the <c>typeTag</c> is not understood</returns>
    public IOscArgument FromBytes(byte typeTag, ReadOnlySpan<byte> bytes, out int length);
}