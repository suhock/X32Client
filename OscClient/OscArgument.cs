using System;
using System.Collections.Generic;

namespace Suhock.Osc
{
    public abstract class OscArgument
    {
        public static OscArgumentFactoryBag Factory { get; } = new OscArgumentFactoryBag();

        static OscArgument()
        {
            Factory.AddFactory(new OscArgumentFactory());
        }

        protected OscArgument(char typeTag)
        {
            TypeTag = typeTag;
        }

        public char TypeTag { get; }

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

    public abstract class OscArgument<T> : OscArgument
    {
        public T Value { get; set; }

        protected OscArgument(char typeTag) : base(typeTag) { }

        public override string ToString()
        {
            return '[' + Value.ToString() + ']';
        }
    }

    public interface IOscArgumentFactory
    {
        public OscArgument FromBytes(char typeTag, ReadOnlySpan<byte> bytes, out int length);

        public OscArgument FromValue(object value);
    }

    public class OscArgumentFactory: IOscArgumentFactory
    {
        public OscArgument FromBytes(char typeTag, ReadOnlySpan<byte> bytes, out int length)
        {
            length = 0;

            return (char)typeTag switch
            {
                OscStringArgument.TypeTagChar => new OscStringArgument(bytes, out length),
                OscIntArgument.TypeTagChar => new OscIntArgument(bytes, out length),
                OscFloatArgument.TypeTagChar => new OscFloatArgument(bytes, out length),
                OscBlobArgument.TypeTagChar => new OscBlobArgument(bytes, out length),
                _ => null
            };
        }

        public OscArgument FromValue(object value)
        {
            return value switch
            {
                string stringValue => new OscStringArgument(stringValue),
                int intValue => new OscIntArgument(intValue),
                float floatValue => new OscFloatArgument(floatValue),
                byte[] blobValue => new OscBlobArgument(blobValue),
                _ => null,
            };
        }
    }

    public class OscArgumentFactoryBag : IOscArgumentFactory
    {
        private readonly Stack<IOscArgumentFactory> Factories = new Stack<IOscArgumentFactory>();

        public void AddFactory(IOscArgumentFactory factory)
        {
            Factories.Push(factory);
        }

        public OscArgument FromBytes(char typeTag, ReadOnlySpan<byte> bytes, out int length)
        {
            foreach (IOscArgumentFactory factory in Factories)
            {
                OscArgument arg = factory.FromBytes(typeTag, bytes, out length);

                if (arg != null)
                {
                    return arg;
                }
            }

            length = 0;

            return null;
        }

        public OscArgument FromValue(object value)
        {
            foreach (IOscArgumentFactory factory in Factories)
            {
                OscArgument arg = factory.FromValue(value);

                if (arg != null)
                {
                    return arg;
                }
            }

            return null;
        }
    }
}
