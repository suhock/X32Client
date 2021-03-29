using System;

namespace Suhock.Osc
{
    /// <summary>
    /// Base class for OSC protocol message arguments
    /// </summary>
    public abstract class OscArgument : IOscArgument
    {
        protected OscArgument(byte typeTag)
        {
            TypeTag = typeTag;
        }

        public byte TypeTag { get; }

        public abstract int GetByteCount();

        public abstract byte[] GetBytes();

        public int WriteBytes(byte[] bytes)
        {
            return WriteBytes(bytes, 0);
        }

        public int WriteBytes(byte[] bytes, int startIndex)
        {
            return WriteBytes(bytes.AsSpan(startIndex, GetByteCount()));
        }

        public abstract int WriteBytes(Span<byte> bytes);

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
        public T Value { get; set; }

        protected OscArgument(byte typeTag, T value) : base(typeTag)
        {
            Value = value;
        }

        public override string ToString()
        {
            return '[' + Value.ToString() + ']';
        }
    }
}
