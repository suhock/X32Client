using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suhock.Osc.Arguments;

namespace Suhock.Osc.Tests;

[TestClass]
public class OscArgumentParserTests
{
    [TestMethod]
    public void FromBytesTest()
    {
        var parser = new OscArgumentParser();

        {
            Assert.AreEqual(1,
                ((OscIntArgument)parser.FromBytes((byte)'i', new byte[] { 0, 0, 0, 1 }, out var length)).Value);
            Assert.AreEqual(4, length);
        }

        {
            Assert.AreEqual(1.0f,
                ((OscFloatArgument)parser.FromBytes((byte)'f', new byte[] { 0x3F, 0x80, 0, 0 }, out var length)).Value);
            Assert.AreEqual(4, length);
        }

        {
            Assert.AreEqual("a",
                ((OscStringArgument)parser.FromBytes((byte)'s',
                    new byte[] { (byte)'a', 0, 0, 0 }, out var length)).Value);
            Assert.AreEqual(4, length);
        }

        {
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3 },
                ((OscBlobArgument)parser.FromBytes((byte)'b',
                    new byte[] { 0, 0, 0, 3, 1, 2, 3, 0 }, out var length)).Value);
            Assert.AreEqual(8, length);
        }

        {
            Assert.ThrowsException<ArgumentException>(() =>
                parser.FromBytes((byte)'c', new byte[] { 1, 2, 3, 4 }, out var length));
        }
    }
}