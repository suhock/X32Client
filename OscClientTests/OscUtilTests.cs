using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Suhock.Osc.Tests
{
    [TestClass()]
    public class OscUtilTests
    {
        [TestMethod()]
        public void ReadIntTests()
        {
            Assert.AreEqual(0, OscUtil.ReadInt(new byte[] { 0, 0, 0, 0 }, out int length));
            Assert.AreEqual(4, length);

            Assert.AreEqual(1, OscUtil.ReadInt(new byte[] { 0, 0, 0, 1 }, out length));
            Assert.AreEqual(4, length);

            Assert.AreEqual(0x100, OscUtil.ReadInt(new byte[] { 0, 0, 1, 0, 2, 3, 4, 5 }, out length));
            Assert.AreEqual(4, length);

            Assert.AreEqual(0x10000, OscUtil.ReadInt(new byte[] { 0, 1, 0, 0 }, out length));
            Assert.AreEqual(4, length);

            Assert.AreEqual(0x1000000, OscUtil.ReadInt(new byte[] { 1, 0, 0, 0 }, out length));
            Assert.AreEqual(4, length);
        }

        [TestMethod()]
        public void ReadFloatTests()
        {
            Assert.AreEqual(0.0f, OscUtil.ReadFloat(new byte[] { 0, 0, 0, 0 }, out int length));
            Assert.AreEqual(4, length);

            Assert.AreEqual(1.0f, OscUtil.ReadFloat(new byte[] { 0x3F, 0x80, 0, 0 }, out length));
            Assert.AreEqual(4, length);

            Assert.AreEqual(-1.0f, OscUtil.ReadFloat(new byte[] { 0xBF, 0x80, 0, 0 }, out length));
            Assert.AreEqual(4, length);

            Assert.AreEqual(float.PositiveInfinity,
                OscUtil.ReadFloat(new byte[] { 0x7F, 0x80, 0, 0, 2, 3, 4, 5 }, out length));
            Assert.AreEqual(4, length);
        }

        [TestMethod()]
        public void ReadStringTests()
        {
            Assert.AreEqual("", OscUtil.ReadString(new byte[] { 0, 0, 0, 0 }, out int length));
            Assert.AreEqual(4, length);

            Assert.AreEqual("", OscUtil.ReadString(Array.Empty<byte>(), out length));
            Assert.AreEqual(0, length);

            Assert.AreEqual("a", OscUtil.ReadString(new byte[] { (byte)'a', 0, 0, 0 }, out length));
            Assert.AreEqual(4, length);

            Assert.AreEqual("a", OscUtil.ReadString(new byte[] { (byte)'a' }, out length));
            Assert.AreEqual(1, length);

            Assert.AreEqual("abcd", OscUtil.ReadString(new byte[] {
                (byte)'a', (byte)'b', (byte)'c', (byte)'d', 0, 0, (byte)'e', 0
            }, out length));
            Assert.AreEqual(8, length);
        }

        [TestMethod()]
        public void ReadBlobTests()
        {
            CollectionAssert.AreEqual(ReadOnlySpan<byte>.Empty.ToArray(),
                OscUtil.ReadBlob(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, out int length).ToArray());
            Assert.AreEqual(4, length);

            CollectionAssert.AreEqual(new byte[] { 2 },
                OscUtil.ReadBlob(new byte[] { 0, 0, 0, 1, 2, 0, 0, 0 }, out length).ToArray());
            Assert.AreEqual(8, length);

            CollectionAssert.AreEqual(new byte[] { 5, 6, 7, 8 },
                OscUtil.ReadBlob(new byte[] { 0, 0, 0, 4, 5, 6, 7, 8 }, out length).ToArray());
            Assert.AreEqual(8, length);

            CollectionAssert.AreEqual(new byte[] { 5, 6, 7, 8, 9 },
                OscUtil.ReadBlob(new byte[] { 0, 0, 0, 5, 5, 6, 7, 8, 9 }, out length).ToArray());
            Assert.AreEqual(9, length);
        }
    }
}