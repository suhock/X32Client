using System;
using System.Linq;
using System.Text;

namespace Suhock.Osc
{
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
            return (offset + 3) & 0x7ffffffc;
        }

        /// <summary>
        /// Aligns the given byte offset to the first four-byte boundary greater than it.
        /// </summary>
        /// 
        /// <param name="offset">The offset to align</param>
        /// <returns>The incremented and aligned offset</returns>
        public static int PaddedAlignOffset(int offset)
        {
            return (offset + 4) & 0x7ffffffc;
        }

        public static string ReadString(ReadOnlySpan<byte> bytes, out int length)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            int stringLength = bytes.IndexOf((byte)0);

            if (stringLength < 0)
            {
                length = stringLength = bytes.Length;
            }
            else
            {
                length = Math.Min(AlignOffset(stringLength + 1), bytes.Length);
            }

            return Encoding.ASCII.GetString(bytes[0..stringLength]);
        }

        public static int ReadInt(ReadOnlySpan<byte> bytes, out int length)
        {
            length = 4;
            return BitConverter.ToInt32(ReadBigEndianBytes(bytes, length));
        }

        public static float ReadFloat(ReadOnlySpan<byte> bytes, out int length)
        {
            length = 4;
            return BitConverter.ToSingle(ReadBigEndianBytes(bytes, length));
        }

        public static ReadOnlySpan<byte> ReadBlob(ReadOnlySpan<byte> bytes, out int length)
        {
            int blobLength = ReadInt(bytes, out length);
            length += blobLength;

            return bytes[0..blobLength];
        }

        private static ReadOnlySpan<byte> ReadBigEndianBytes(ReadOnlySpan<byte> bytes, int length)
        {
            if (BitConverter.IsLittleEndian)
            {
                byte[] arr = bytes[0..length].ToArray();
                Array.Reverse(arr);

                return arr;
            }
            else
            {
                return bytes[0..length];
            }
        }

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

        public static int WriteString(Span<byte> target, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            int paddedLength = AlignOffset(value.Length + 1);

            if (target.Length < paddedLength)
            {
                throw new ArgumentOutOfRangeException(nameof(target), target.Length, "Target too short");
            }

            Encoding.ASCII.GetBytes(value, target);
            target[value.Length..paddedLength].Fill(0);

            return paddedLength;
        }

        public static int WriteBlob(Span<byte> target, ReadOnlySpan<byte> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            int paddedLength = AlignOffset(4 + value.Length);

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
}
