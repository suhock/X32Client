using System;

namespace Suhock.Osc;

/// <summary>
/// Base class for OSC protocol message arguments
/// </summary>
public abstract class OscArgument : IOscArgument
{
    public abstract byte TypeTag { get; }

    public abstract int GetByteCount();

    public abstract byte[] GetBytes();

    public int WriteBytes(byte[] target) => 
        WriteBytes(target, 0);

    public int WriteBytes(byte[] target, int startIndex) => 
        WriteBytes(target.AsSpan(startIndex, GetByteCount()));

    public abstract int WriteBytes(Span<byte> target);

    public abstract override string ToString();
}

/// <summary>
/// Represents an OSC protocol message argument with a value of native type <c>T</c>
/// </summary>
/// <typeparam name="T">The native type that this argument type represents</typeparam>
public abstract class OscArgument<T> : OscArgument
{
    /// <summary>
    /// The value of the argument
    /// </summary>
    public T Value { get; protected init; }

    protected OscArgument(T value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return '[' + Value?.ToString() + ']';
    }
}