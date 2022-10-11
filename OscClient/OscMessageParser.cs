using System;
using System.Collections.Generic;

namespace Suhock.Osc;

public sealed class OscMessageParser: IOscMessageParser
{
    private readonly IOscArgumentParser _argumentParser;

    public OscMessageParser() : this(new OscArgumentParser())
    {
    }
    
    public OscMessageParser(IOscArgumentParser argumentParser)
    {
        _argumentParser = argumentParser;
    }

    public OscMessage ParseBytes(ReadOnlySpan<byte> bytes, out int length)
    {
        var segmentStart = bytes;
        var address = OscUtil.ReadString(segmentStart, out var segmentLength);
        length = segmentLength;
        segmentStart = segmentStart[segmentLength..];

        var typeTagString = OscUtil.ReadByteString(segmentStart, out segmentLength);

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

        var argList = new List<IOscArgument>(typeTagString.Length - 1);

        for (var i = 1; i < typeTagString.Length; i++)
        {
            segmentStart = segmentStart[segmentLength..];
            var arg = _argumentParser.FromBytes(typeTagString[i], segmentStart, out segmentLength);

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