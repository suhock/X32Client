using System;

namespace Suhock.Osc;

public interface IOscArgument
{
    /// <summary>
    /// The ASCII character that indicates this argument type in an OSC type tag string.
    /// </summary>
    public byte TypeTag { get; }

    /// <summary>
    /// </summary>
    /// <returns>The number of bytes in the binary-encoded OSC message argument</returns>
    int GetByteCount();

    /// <summary>
    /// Encodes the OSC message argument as a binary string
    /// </summary>
    /// <returns>A byte array containing the binary-encoded OSC message argument</returns>
    byte[] GetBytes();

    /// <summary>
    /// Writes the binary-encoded value of the argument to a byte array.
    /// </summary>
    /// <param name="target">The target byte array</param>
    /// <returns>The number of bytes written</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the length of the <c>bytes</c> is too small to fit the encoded argument
    /// </exception>
    int WriteBytes(byte[] target);

    /// <summary>
    /// Writes the binary-encoded value of the argument to a byte array starting at the specified offset.
    /// </summary>
    /// <param name="target">The target byte array</param>
    /// <param name="startIndex">The index in the byte array at which to start writing</param>
    /// <returns>The number of bytes written</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the length of the <c>bytes</c> starting from <c>startIndex</c> is too small to fit the encoded argument
    /// </exception>
    int WriteBytes(byte[] target, int startIndex);

    /// <summary>
    /// Writes the binary-encoded value of the argument to a <c>Span</c> of bytes.
    /// </summary>
    /// <param name="target">The target byte <c>Span</c></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the length of the <c>bytes</c> is too small to fit the encoded argument
    /// </exception>
    int WriteBytes(Span<byte> target);

    /// <summary>
    /// </summary>
    /// <returns>A string representation of the argument suitable for display to a user</returns>
    string ToString();
}