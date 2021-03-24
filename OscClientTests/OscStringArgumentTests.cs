using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Suhock.Osc.Tests
{
    [TestClass()]
    public class OscStringArgumentTests
    {
        [TestMethod()]
        public void OscStringArgumentTest()
        {
            Assert.AreEqual("", new OscStringArgument().Value);
        }

        [TestMethod()]
        public void OscStringArgumentTest1()
        {
            Assert.AreEqual("", new OscStringArgument("").Value);
            Assert.AreEqual("a", new OscStringArgument("a").Value);
            Assert.AreEqual("ab", new OscStringArgument("ab").Value);
            Assert.AreEqual("abc", new OscStringArgument("abc").Value);
            Assert.AreEqual("abcd", new OscStringArgument("abcd").Value);
            Assert.AreEqual("abcde", new OscStringArgument("abcde").Value);
        }

        [TestMethod()]
        public void GetByteCountTest()
        {
            Assert.AreEqual(4, new OscStringArgument("").GetByteCount());
            Assert.AreEqual(4, new OscStringArgument("a").GetByteCount());
            Assert.AreEqual(4, new OscStringArgument("ab").GetByteCount());
            Assert.AreEqual(4, new OscStringArgument("abc").GetByteCount());
            Assert.AreEqual(8, new OscStringArgument("abcd").GetByteCount());
            Assert.AreEqual(8, new OscStringArgument("abcde").GetByteCount());
        }

        [TestMethod()]
        public void GetBytesTest()
        {
            CollectionAssert.AreEqual
                (new byte[] { 0, 0, 0, 0 },
                new OscStringArgument("").GetBytes());
            CollectionAssert.AreEqual(
                new byte[] { (byte)'a', 0, 0, 0 }, 
                new OscStringArgument("a").GetBytes());
            CollectionAssert.AreEqual(
                new byte[] { (byte)'a', (byte)'b', 0, 0 },
                new OscStringArgument("ab").GetBytes());
            CollectionAssert.AreEqual(
                new byte[] { (byte)'a', (byte)'b', (byte)'c', 0 },
                new OscStringArgument("abc").GetBytes());
            CollectionAssert.AreEqual(
                new byte[] { (byte)'a', (byte)'b', (byte)'c', (byte)'d', 0, 0, 0, 0 },
                new OscStringArgument("abcd").GetBytes());
            CollectionAssert.AreEqual(
                new byte[] { (byte)'a', (byte)'b', (byte)'c', (byte)'d', (byte)'e', 0, 0, 0 },
                new OscStringArgument("abcde").GetBytes());
        }

        [TestMethod()]
        public void WriteBytesTest()
        {
            OscStringArgument arg = new OscStringArgument("");
            byte[] bytes = new byte[] { 1, 2, 3, 4 };
            Assert.AreEqual(4, arg.WriteBytes(bytes));
            CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0 }, bytes);

            arg = new OscStringArgument("a");
            bytes = new byte[] { 1, 2, 3, 4 };
            Assert.AreEqual(4, arg.WriteBytes(bytes));
            CollectionAssert.AreEqual(new byte[] { (byte)'a', 0, 0, 0 }, bytes);

            arg = new OscStringArgument("ab");
            bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Assert.AreEqual(4, arg.WriteBytes(bytes));
            CollectionAssert.AreEqual(new byte[] { (byte)'a', (byte)'b', 0, 0, 5, 6, 7, 8 }, bytes);

            arg = new OscStringArgument("abcd");
            bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Assert.AreEqual(8, arg.WriteBytes(bytes));
            CollectionAssert.AreEqual(new byte[] { (byte)'a', (byte)'b', (byte)'c', (byte)'d', 0, 0, 0, 0 }, bytes);

            arg = new OscStringArgument("abcde");
            bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Assert.AreEqual(8, arg.WriteBytes(bytes));
            CollectionAssert.AreEqual(new byte[] { (byte)'a', (byte)'b', (byte)'c', (byte)'d', (byte)'e', 0, 0, 0 },
                bytes);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            OscStringArgument arg = new OscStringArgument("Test");
            Assert.AreEqual("\"Test\"", arg.ToString());

            arg = new OscStringArgument("");
            Assert.AreEqual("\"\"", arg.ToString());
        }
    }
}