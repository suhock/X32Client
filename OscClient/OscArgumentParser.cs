using System;
using Suhock.Osc.Arguments;

namespace Suhock.Osc;

/// <summary>
/// An <see cref="IOscArgumentParser"/> implementation for working with the four basic OSC argument types specified in
/// the OSC protocol.
/// </summary>
public class OscArgumentParser : IOscArgumentParser
{
    public virtual IOscArgument FromBytes(byte typeTag, ReadOnlySpan<byte> bytes, out int length)
    {
        length = 0;

        return typeTag switch
        {
            OscStringArgument.TypeTagByte => new OscStringArgument(OscUtil.ReadString(bytes, out length)),
            OscIntArgument.TypeTagByte => new OscIntArgument(OscUtil.ReadInt(bytes, out length)),
            OscFloatArgument.TypeTagByte => new OscFloatArgument(OscUtil.ReadFloat(bytes, out length)),
            OscBlobArgument.TypeTagByte => new OscBlobArgument(OscUtil.ReadBlob(bytes, out length)),
            _ => throw new ArgumentException($"Unsupported type tag: {typeTag}", nameof(typeTag))
        };
    }

}