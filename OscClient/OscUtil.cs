using System;
using System.Text;

namespace Suhock.Osc;

public static class OscUtil
{
    /// <summary>
    /// Aligns the given byte offset to the first four-byte boundary greater than or equal to it.
    /// </summary>
    /// 
    /// <param name="offset">The offset to align</param>
    /// <returns>The aligned offset</returns>
    public static int AlignOffset(int offset)
    {
        return (offset + 3) & ~0x03;
    }

    /// <summary>
    /// Reads a null-terminated, four-byte-aligned OSC-string from a span of bytes. If no null character is
    /// encountered before the end of the span, the value up until the end of the span is returned.
    /// </summary>
    /// <param name="bytes">The span of bytes from which to read</param>
    /// <param name="length">The four-byte aligned number of bytes read, including null bytes, or the length of
    /// <code>bytes</code> if there are no null bytes</param>
    /// <returns>The span of bytes up to, but not including the first null byte in <code>bytes</code>, or the
    /// entire <code>bytes</code> span if there are no null bytes</returns>
    public static ReadOnlySpan<byte> ReadByteString(ReadOnlySpan<byte> bytes, out int length)
    {
        var stringLength = bytes.IndexOf((byte)0);

        if (stringLength < 0)
        {
            length = stringLength = bytes.Length;
        }
        else
        {
            length = Math.Min(AlignOffset(stringLength + 1), bytes.Length);
        }

        return bytes[..stringLength];
    }

    /// <summary>
    /// Reads a null-terminated, four-byte-aligned OSC-string from a span of bytes. If no null character is
    /// encountered before the end of the span, the value up until the end of the span is returned.
    /// </summary>
    /// <param name="bytes">The span of bytes from which to read</param>
    /// <param name="length">The number of bytes read</param>
    /// <returns>The decoded string</returns>
    public static string ReadString(ReadOnlySpan<byte> bytes, out int length)
    {
        return Encoding.ASCII.GetString(ReadByteString(bytes, out length));
    }

    /// <summary>
    /// Reads a 32-bit big-endian two's complement integer from a span of bytes.
    /// </summary>
    /// <param name="bytes">The span of bytes from which to read</param>
    /// <param name="length">The number of bytes read</param>
    /// <returns>The decoded integer</returns>
    public static int ReadInt(ReadOnlySpan<byte> bytes, out int length)
    {
        length = 4;
        return BitConverter.ToInt32(ReadBigEndianBytes(bytes, length));
    }

    /// <summary>
    /// Reads a 32-bit big-endian IEEE 754 floating point number from a span of bytes.
    /// </summary>
    /// <param name="bytes">The span of bytes from which to read</param>
    /// <param name="length">The number of bytes read</param>
    /// <returns>The decoded floating point number</returns>
    public static float ReadFloat(ReadOnlySpan<byte> bytes, out int length)
    {
        length = 4;
        return BitConverter.ToSingle(ReadBigEndianBytes(bytes, length));
    }

    /// <summary>
    /// Reads a four-byte-aligned OSC-blob value from a span of bytes.
    /// </summary>
    /// <param name="bytes">The span of bytes from which to read</param>
    /// <param name="length">The number of bytes read</param>
    /// <returns>The decoded OSC-blob</returns>
    public static ReadOnlySpan<byte> ReadBlob(ReadOnlySpan<byte> bytes, out int length)
    {
        var blobLength = ReadInt(bytes, out length);
        length += AlignOffset(blobLength);

        if (length > bytes.Length)
        {
            length = bytes.Length;
        }

        return bytes[4..Math.Min(4 + blobLength, bytes.Length)];
    }

    private static ReadOnlySpan<byte> ReadBigEndianBytes(ReadOnlySpan<byte> bytes, int length)
    {
        if (BitConverter.IsLittleEndian)
        {
            var arr = bytes[0..length].ToArray();
            Array.Reverse(arr);

            return arr;
        }
        else
        {
            return bytes[0..length];
        }
    }

    /// <summary>
    /// Writes a 32-bit big-endian two's complement integer to the specified span of bytes.
    /// </summary>
    /// <param name="target">The span of bytes to which to write</param>
    /// <param name="value">The integer to write</param>
    /// <returns>The number of bytes written</returns>
    public static int WriteInt(Span<byte> target, int value)
    {
        if (target.Length < 4)
        {
            throw new ArgumentOutOfRangeException(nameof(target), target.Length, "Target too short");
        }

        if (!BitConverter.TryWriteBytes(target, value))
        {
            throw new Exception("Could not convert value");
        }

        if (BitConverter.IsLittleEndian)
        {
            target[0..4].Reverse();
        }

        return 4;
    }

    /// <summary>
    /// Writes a 32-bit big-endian IEEE 754 floating point number to the specified span of bytes.
    /// </summary>
    /// <param name="target">The span of bytes to which to write</param>
    /// <param name="value">The floating point number to write</param>
    /// <returns>The number of bytes written</returns>
    public static int WriteFloat(Span<byte> target, float value)
    {
        if (target.Length < 4)
        {
            throw new ArgumentOutOfRangeException(nameof(target), target.Length, "Target too short");
        }

        if (!BitConverter.TryWriteBytes(target, value))
        {
            throw new Exception("Could not convert value");
        }

        if (BitConverter.IsLittleEndian)
        {
            target[0..4].Reverse();
        }

        return 4;
    }

    /// <summary>
    /// Writes a four-byte-aligned null-terminated OSC-string to the specified span of bytes.
    /// </summary>
    /// <param name="target">The span of bytes to which to write</param>
    /// <param name="value">The string of bytes to write</param>
    /// <returns>The number of bytes written</returns>
    public static int WriteString(Span<byte> target, ReadOnlySpan<byte> value)
    {
        var paddedLength = AlignOffset(value.Length + 1);

        if (target.Length < paddedLength)
        {
            throw new ArgumentOutOfRangeException(nameof(target), target.Length, "Target too short");
        }

        value.CopyTo(target);
        target[value.Length..paddedLength].Fill(0);

        return paddedLength;
    }

    /// <summary>
    /// Writes a four-byte-aligned null-terminated OSC-string to the specified span of bytes.
    /// </summary>
    /// <param name="target">The span of bytes to which to write</param>
    /// <param name="value">The string to write</param>
    /// <returns>The number of bytes written</returns>
    public static int WriteString(Span<byte> target, string value)
    {
        var paddedLength = AlignOffset(value.Length + 1);

        if (target.Length < paddedLength)
        {
            throw new ArgumentOutOfRangeException(nameof(target), target.Length, "Target too short");
        }

        Encoding.ASCII.GetBytes(value, target);
        target[value.Length..paddedLength].Fill(0);

        return paddedLength;
    }

    /// <summary>
    /// Writes a four-byte aligned OSC-blob to the specified span of bytes.
    /// </summary>
    /// <param name="target">The span of bytes to which to write</param>
    /// <param name="value">The sequence of bytes to write</param>
    /// <returns>The number of bytes written</returns>
    public static int WriteBlob(Span<byte> target, ReadOnlySpan<byte> value)
    {
        var paddedLength = AlignOffset(4 + value.Length);

        if (target.Length < paddedLength)
        {
            throw new ArgumentOutOfRangeException(nameof(target), target.Length, "Target too short");
        }

        WriteInt(target, value.Length);
        value.CopyTo(target[4..]);

        if (paddedLength > value.Length)
        {
            target[(4 + value.Length)..paddedLength].Fill(0);
        }

        return paddedLength;
    }
}