using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suhock.Osc.Arguments;
using System;

namespace Suhock.Osc.Tests
{
    [TestClass()]
    public class OscBlobArgumentTests
    {
        [TestMethod()]
        public void OscBlobArgumentTest()
        {
            Assert.AreEqual(Array.Empty<byte>(), new OscBlobArgument().Value);
        }

        [TestMethod()]
        public void OscBlobArgumentTest1()
        {
            CollectionAssert.AreEqual(Array.Empty<byte>(), new OscBlobArgument(Array.Empty<byte>()).Value);
            CollectionAssert.AreEqual(new byte[] { 1 }, new OscBlobArgument(new byte[] { 1 }).Value);
        }

        [TestMethod()]
        public void GetByteCountTest()
        {
            Assert.AreEqual(4, new OscBlobArgument(Array.Empty<byte>()).GetByteCount());
            Assert.AreEqual(8, new OscBlobArgument(new byte[] { 1 }).GetByteCount());
            Assert.AreEqual(8, new OscBlobArgument(new byte[] { 1, 2, 3, 4 }).GetByteCount());
            Assert.AreEqual(12, new OscBlobArgument(new byte[] { 1, 2, 3, 4, 5 }).GetByteCount());
        }

        [TestMethod()]
        public void GetBytesTest()
        {
            CollectionAssert.AreEqual(
                new byte[] { 0, 0, 0, 0 },
                new OscBlobArgument(Array.Empty<byte>()).GetBytes());
            CollectionAssert.AreEqual(
                new byte[] { 0, 0, 0, 1, 2, 0, 0, 0 }, 
                new OscBlobArgument(new byte[] { 2 }).GetBytes());
            CollectionAssert.AreEqual(
                new byte[] { 0, 0, 0, 4, 5, 6, 7, 8 },
                new OscBlobArgument(new byte[] { 5, 6, 7, 8 }).GetBytes());
            CollectionAssert.AreEqual(
                new byte[] { 0, 0, 0, 5, 5, 6, 7, 8, 9, 0, 0, 0 },
                new OscBlobArgument(new byte[] { 5, 6, 7, 8, 9 }).GetBytes());
        }

        [TestMethod()]
        public void WriteBytesTest()
        {
            OscBlobArgument arg = new OscBlobArgument(Array.Empty<byte>());
            byte[] bytes = new byte[] { 1, 2, 3, 4 };
            Assert.AreEqual(4, arg.WriteBytes(bytes));
            CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0 }, bytes);

            arg = new OscBlobArgument(new byte[] { 2 });
            bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Assert.AreEqual(8, arg.WriteBytes(bytes));
            CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 1, 2, 0, 0, 0 }, bytes);

            arg = new OscBlobArgument(new byte[] { 5, 6, 7, 8 });
            bytes = new byte[] { 11, 12, 13, 14, 15, 16, 17, 18 };
            Assert.AreEqual(8, arg.WriteBytes(bytes));
            CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 4, 5, 6, 7, 8 }, bytes);

            arg = new OscBlobArgument(new byte[] { 5, 6, 7, 8, 9 });
            bytes = new byte[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            Assert.AreEqual(12, arg.WriteBytes(bytes));
            CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 5, 5, 6, 7, 8, 9, 0, 0, 0, 23, 24 }, bytes);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.AreEqual("[blob:0]", new OscBlobArgument(Array.Empty<byte>()).ToString());
            Assert.AreEqual("[blob:2]", new OscBlobArgument(new byte[] { 1, 2 }).ToString());
        }
    }
}