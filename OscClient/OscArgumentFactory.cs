using Suhock.Osc.Arguments;
using System;

namespace Suhock.Osc;

/// <summary>
/// An <see cref="IOscArgumentFactory"/> implementation for working with the four basic OSC argument types specified in
/// the OSC protocol.
/// </summary>
public class OscArgumentFactory : IOscArgumentFactory
{
    public virtual IOscArgument FromValue(object value)
    {
        return value switch
        {
            string stringValue => new OscStringArgument(stringValue),
            int intValue => new OscIntArgument(intValue),
            float floatValue => new OscFloatArgument(floatValue),
            byte[] blobValue => new OscBlobArgument(blobValue),
            _ => throw new ArgumentException($"Unsupported type: {value.GetType()}", nameof(value))
        };
    }
}