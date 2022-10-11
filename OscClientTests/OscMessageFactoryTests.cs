using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suhock.Osc.Tests;

[TestClass]
public class OscMessageFactoryTests
{
    [TestMethod]
    public void CreateTest()
    {
        var factory = new OscMessageFactory();

        var msg = factory.Create("/address");
        Assert.AreEqual("/address", msg.Address);
        Assert.AreEqual(0, msg.Arguments.Count);
    }

    [TestMethod]
    public void CreateTest1()
    {
        var factory = new OscMessageFactory();

        var msg = factory.Create("/address", 1, 1.0f, "a", new byte[] { 1, 2 });
        Assert.AreEqual("/address", msg.Address);
        Assert.AreEqual(1, msg.GetArgumentValue<int>(0));
        Assert.AreEqual(1.0f, msg.GetArgumentValue<float>(1));
        Assert.AreEqual("a", msg.GetArgumentValue<string>(2));
        CollectionAssert.AreEqual(new byte[] { 1, 2 }, msg.GetArgumentValue<byte[]>(3));
    }
}