using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suhock.Osc.Arguments;
using System;

namespace Suhock.Osc.Tests
{
    [TestClass()]
    public class OscArgumentFactoryTests
    {
        [TestMethod()]
        public void FromBytesTest()
        {
            var factory = new OscArgumentFactory();

            {
                Assert.AreEqual(1,
                    ((OscIntArgument)factory.FromBytes((byte)'i', new byte[] { 0, 0, 0, 1 }, out int length)).Value);
                Assert.AreEqual(4, length);
            }

            {
                Assert.AreEqual(1.0f,
                    ((OscFloatArgument)factory.FromBytes((byte)'f', new byte[] { 0x3F, 0x80, 0, 0 }, out int length)).Value);
                Assert.AreEqual(4, length);
            }

            {
                Assert.AreEqual("a",
                    ((OscStringArgument)factory.FromBytes((byte)'s',
                    new byte[] { (byte)'a', 0, 0, 0 }, out int length)).Value);
                Assert.AreEqual(4, length);
            }

            {
                CollectionAssert.AreEqual(new byte[] { 1, 2, 3 },
                    ((OscBlobArgument)factory.FromBytes((byte)'b',
                    new byte[] { 0, 0, 0, 3, 1, 2, 3, 0 }, out int length)).Value);
                Assert.AreEqual(8, length);
            }

            {
                Assert.ThrowsException<ArgumentException>(() => factory.FromBytes((byte)'c', new byte[] { 1, 2, 3, 4 }, out int length));
            }
        }

        [TestMethod()]
        public void FromValueTest()
        {
            var factory = new OscArgumentFactory();

            Assert.AreEqual(1, ((OscIntArgument)factory.FromValue(1)).Value);
            Assert.AreEqual(1.0f, ((OscFloatArgument)factory.FromValue(1.0f)).Value);
            Assert.AreEqual("a", ((OscStringArgument)factory.FromValue("a")).Value);
            CollectionAssert.AreEqual(new byte[] { 1, 2 },
                ((OscBlobArgument)factory.FromValue(new byte[] { 1, 2 })).Value);
            Assert.ThrowsException<ArgumentException>(() => factory.FromValue(1.0));
        }
    }
}