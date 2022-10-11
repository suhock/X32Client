using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suhock.Osc.Arguments;

namespace Suhock.Osc.Tests;

[TestClass]
public class OscFloatArgumentTests
{
    [TestMethod]
    public void OscFloatArgumentTest()
    {
        Assert.AreEqual(0.0f, new OscFloatArgument().Value);
    }

    [TestMethod]
    public void OscFloatArgumentTest1()
    {
        Assert.AreEqual(-1.0f, new OscFloatArgument(-1.0f).Value);
        Assert.AreEqual(0.0f, new OscFloatArgument(0.0f).Value);
        Assert.AreEqual(1.0f, new OscFloatArgument(1.0f).Value);
    }

    [TestMethod]
    public void GetByteCountTest()
    {
        Assert.AreEqual(4, new OscFloatArgument(-1.0f).GetByteCount());
        Assert.AreEqual(4, new OscFloatArgument(0.0f).GetByteCount());
        Assert.AreEqual(4, new OscFloatArgument(1.0f).GetByteCount());
    }

    [TestMethod]
    public void GetBytesTest()
    {
        CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0 }, new OscFloatArgument(0.0f).GetBytes());
        CollectionAssert.AreEqual(new byte[] { 0x3F, 0x80, 0, 0 }, new OscFloatArgument(1.0f).GetBytes());
        CollectionAssert.AreEqual(new byte[] { 0xBF, 0x80, 0, 0 }, new OscFloatArgument(-1.0f).GetBytes());
        CollectionAssert.AreEqual(new byte[] { 0x7F, 0x80, 0, 0 },
            new OscFloatArgument(float.PositiveInfinity).GetBytes());
    }

    [TestMethod]
    public void WriteBytesTest()
    {
        var arg = new OscFloatArgument(0.0f);
        var bytes = new byte[] { 1, 2, 3, 4 };
        Assert.AreEqual(4, arg.WriteBytes(bytes));
        CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0 }, bytes);

        arg = new OscFloatArgument(1.0f);
        bytes = new byte[] { 2, 3, 4, 5 };
        Assert.AreEqual(4, arg.WriteBytes(bytes));
        CollectionAssert.AreEqual(new byte[] { 0x3F, 0x80, 0, 0 }, bytes);

        arg = new OscFloatArgument(-1.0f);
        bytes = new byte[] { 2, 3, 4, 5, 6, 7, 8, 9 };
        Assert.AreEqual(4, arg.WriteBytes(bytes));
        CollectionAssert.AreEqual(new byte[] { 0xBF, 0x80, 0, 0, 6, 7, 8, 9 }, bytes);

        arg = new OscFloatArgument(float.PositiveInfinity);
        bytes = new byte[] { 2, 3, 4, 5 };
        Assert.AreEqual(4, arg.WriteBytes(bytes));
        CollectionAssert.AreEqual(new byte[] { 0x7F, 0x80, 0, 0 }, bytes);
    }

    [TestMethod]
    public void ToStringTest()
    {
        Assert.AreEqual("[0]", new OscFloatArgument(0.0f).ToString());
        Assert.AreEqual("[1]", new OscFloatArgument(1.0f).ToString());
        Assert.AreEqual("[-1]", new OscFloatArgument(-1.0f).ToString());
        Assert.AreEqual("[0.5]", new OscFloatArgument(0.5f).ToString());
    }
}