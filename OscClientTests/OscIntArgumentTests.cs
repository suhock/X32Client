using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suhock.Osc.Arguments;

namespace Suhock.Osc.Tests;

[TestClass]
public class OscIntArgumentTests
{
    [TestMethod]
    public void OscIntArgumentTest()
    {
        Assert.AreEqual(0, new OscIntArgument().Value);
    }

    [TestMethod]
    public void OscIntArgumentTest1()
    {
        Assert.AreEqual(-1, new OscIntArgument(-1).Value);
        Assert.AreEqual(0, new OscIntArgument(0).Value);
        Assert.AreEqual(1, new OscIntArgument(1).Value);
    }

    [TestMethod]
    public void GetByteCountTest()
    {
        Assert.AreEqual(4, new OscIntArgument(-1).GetByteCount());
        Assert.AreEqual(4, new OscIntArgument(0).GetByteCount());
        Assert.AreEqual(4, new OscIntArgument(1).GetByteCount());
    }

    [TestMethod]
    public void GetBytesTest()
    {
        CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0 }, new OscIntArgument(0).GetBytes());
        CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 1 }, new OscIntArgument(1).GetBytes());
        CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0 }, new OscIntArgument(0x1000000).GetBytes());
        CollectionAssert.AreEqual(new byte[] { 255, 255, 255, 255 }, new OscIntArgument(-1).GetBytes());
    }

    [TestMethod]
    public void WriteBytesTest()
    {
        var arg = new OscIntArgument(0);
        var bytes = new byte[] { 1, 2, 3, 4 };
        Assert.AreEqual(4, arg.WriteBytes(bytes));
        CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0 }, bytes);

        arg = new OscIntArgument(1);
        bytes = new byte[] { 2, 3, 4, 5 };
        Assert.AreEqual(4, arg.WriteBytes(bytes));
        CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 1 }, bytes);

        arg = new OscIntArgument(0x1000000);
        bytes = new byte[] { 2, 3, 4, 5, 6, 7, 8, 9 };
        Assert.AreEqual(4, arg.WriteBytes(bytes));
        CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0, 6, 7, 8, 9 }, bytes);

        arg = new OscIntArgument(-1);
        bytes = new byte[] { 2, 3, 4, 5 };
        Assert.AreEqual(4, arg.WriteBytes(bytes));
        CollectionAssert.AreEqual(new byte[] { 255, 255, 255, 255 }, bytes);
    }

    [TestMethod]
    public void ToStringTest()
    {
        Assert.AreEqual("[0]", new OscIntArgument(0).ToString());
        Assert.AreEqual("[1]", new OscIntArgument(1).ToString());
        Assert.AreEqual("[-1]", new OscIntArgument(-1).ToString());
    }
}