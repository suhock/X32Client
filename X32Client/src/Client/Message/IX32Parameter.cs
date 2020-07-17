using System;

namespace Suhock.X32.Client.Message
{
    public interface IX32Parameter
    {
        char TypeCode { get; }

        int EncodedLength { get; }

        byte[] EncodedValue { get; }

        string ToString();

        internal static IX32Parameter FromMessageBytes(char typeCode, byte[] bytes, int startIndex, out int length)
        {
            IX32Parameter result = typeCode switch
            {
                X32StringParameter.Code => new X32StringParameter(bytes, startIndex, out length),
                X32IntParameter.Code => new X32IntParameter(bytes, startIndex, out length),
                X32FloatParameter.Code => new X32FloatParameter(bytes, startIndex, out length),
                X32BlobParameter.Code => new X32BlobParameter(bytes, startIndex, out length),
                _ => throw new NotSupportedException("Unknown type: " + typeCode),
            };

            return result;
        }
    }
}
