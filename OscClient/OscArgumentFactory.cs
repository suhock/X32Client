using Suhock.Osc.Arguments;
using System;

namespace Suhock.Osc
{
    /// <summary>
    /// An <see cref="IOscArgumentFactory"/> implementation for working with the four basic OSC argument types
    /// specified in the OSC protocol.
    /// </summary>
    public class OscArgumentFactory : IOscArgumentFactory
    {
        /// <inheritdoc/>
        public virtual IOscArgument FromBytes(byte typeTag, ReadOnlySpan<byte> bytes, out int length)
        {
            length = 0;

            return typeTag switch
            {
                OscStringArgument.TypeTagChar => new OscStringArgument(OscUtil.ReadString(bytes, out length)),
                OscIntArgument.TypeTagChar => new OscIntArgument(OscUtil.ReadInt(bytes, out length)),
                OscFloatArgument.TypeTagChar => new OscFloatArgument(OscUtil.ReadFloat(bytes, out length)),
                OscBlobArgument.TypeTagChar => new OscBlobArgument(OscUtil.ReadBlob(bytes, out length)),
                _ => throw new ArgumentException()
            };
        }

        /// <inheritdoc/>
        public virtual IOscArgument FromValue(object value)
        {
            return value switch
            {
                string stringValue => new OscStringArgument(stringValue),
                int intValue => new OscIntArgument(intValue),
                float floatValue => new OscFloatArgument(floatValue),
                byte[] blobValue => new OscBlobArgument(blobValue),
                _ => throw new ArgumentException()
            };
        }
    }
}