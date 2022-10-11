using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suhock.Osc.Tests;

[TestClass]
public class OscMessageParserTests
{
    [TestMethod]
    public void TestParseBytes_WithComplexMessage()
    {
        var parser = new OscMessageParser();
        var factory = new OscMessageFactory();

        var msg = parser.ParseBytes(factory.Create("/address", 1, 1.0f, "a", new byte[] { 1, 2 }).GetPacketBytes(),
            out var length);
        Assert.AreEqual("/address", msg.Address);
        Assert.AreEqual(1, msg.GetArgumentValue<int>(0));
        Assert.AreEqual(1.0f, msg.GetArgumentValue<float>(1));
        Assert.AreEqual("a", msg.GetArgumentValue<string>(2));
        CollectionAssert.AreEqual(new byte[] { 1, 2 }, msg.GetArgumentValue<byte[]>(3));
        Assert.AreEqual(msg.GetPacketLength(), length);
    }

    [TestMethod]
    public void TestParseBytes_WithExtraBytes()
    {
        var parser = new OscMessageParser();

        var msg = parser.ParseBytes(System.Text.Encoding.ASCII.GetBytes("/address\0\0\0\0abc"), out var length);
        Assert.AreEqual("/address", msg.Address);
        Assert.AreEqual(0, msg.Arguments.Count);
        Assert.AreEqual(12, length);
    }
}