using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suhock.Osc.Arguments;
using System.Collections.Generic;

namespace Suhock.Osc.Tests;

[TestClass]
public class OscMessageTests
{
    [TestMethod]
    public void OscMessageTest()
    {
        var msg = new OscMessage("/address");
        Assert.AreEqual("/address", msg.Address);
        Assert.AreEqual(0, msg.Arguments.Count);
    }

    [TestMethod]
    public void OscMessageTest1()
    {
        var args = new List<IOscArgument>(new IOscArgument[] {
            new OscIntArgument()
        });
        var msg = new OscMessage("/address", args);
        Assert.AreEqual("/address", msg.Address);
        Assert.AreNotSame(args, msg.Arguments);
        Assert.AreEqual(1, msg.Arguments.Count);
    }

    [TestMethod]
    public void OscMessageTest2()
    {
        var args = new IOscArgument[] {
            new OscIntArgument()
        };
        var msg = new OscMessage("/address", args);
        Assert.AreEqual("/address", msg.Address);
        Assert.AreEqual(1, msg.Arguments.Count);
    }

    [TestMethod]
    public void GetTypeTagStringTest()
    {
        Assert.AreEqual(",", new OscMessage("/address").GetTypeTagString());
        Assert.AreEqual(",i", new OscMessage("/address", new IOscArgument[] {
            new OscIntArgument()
        }).GetTypeTagString());
        Assert.AreEqual(",si", new OscMessage("/address", new IOscArgument[] {
            new OscStringArgument(),
            new OscIntArgument()
        }).GetTypeTagString());
    }

    [TestMethod]
    public void GetValueTest()
    {
        Assert.AreEqual(1, new OscMessage("/address", new IOscArgument[] 
        {
            new OscIntArgument(1)
        }).GetArgumentValue<int>(0));
        Assert.AreEqual(1.0f, new OscMessage("/address", new IOscArgument[]
        {
            new OscFloatArgument(1.0f)
        }).GetArgumentValue<float>(0));
        Assert.AreEqual("a", new OscMessage("/address", new IOscArgument[]
        {
            new OscIntArgument(1),
            new OscStringArgument("a")
        }).GetArgumentValue<string>(1));
        CollectionAssert.AreEqual(new byte[] { 1 }, new OscMessage("/address", new IOscArgument[]
        {
            new OscBlobArgument(new byte[] { 1 })
        }).GetArgumentValue<byte[]>(0));
    }

    [TestMethod]
    public void WriteBytesTest()
    {
        var bytes = new byte[16];
        Assert.AreEqual(16, new OscMessage("/address").WritePacket(bytes));
        CollectionAssert.AreEqual(
            System.Text.Encoding.ASCII.GetBytes("/address\0\0\0\0,\0\0\0"),
            bytes);

        bytes = new byte[24];
        Assert.AreEqual(24, new OscMessage("/address", new IOscArgument[]
        {
            new OscIntArgument(1),
            new OscStringArgument("a")
        }).WritePacket(bytes));
        CollectionAssert.AreEqual(
            System.Text.Encoding.ASCII.GetBytes("/address\0\0\0\0,is\0\0\0\0\x0001a\0\0\0"),
            bytes);
    }

    [TestMethod]
    public void GetByteCountTest()
    {
        Assert.AreEqual(16, new OscMessage("/address").GetPacketLength());
        Assert.AreEqual(24, new OscMessage("/address", new IOscArgument[]
        {
            new OscIntArgument(1),
            new OscStringArgument("a")
        }).GetPacketLength());
    }

    [TestMethod]
    public void GetBytesTest()
    {
        CollectionAssert.AreEqual(
            System.Text.Encoding.ASCII.GetBytes("/address\0\0\0\0,\0\0\0"),
            new OscMessage("/address").GetPacketBytes());

        CollectionAssert.AreEqual(
            System.Text.Encoding.ASCII.GetBytes("/address\0\0\0\0,is\0\0\0\0\x0001a\0\0\0"),
            new OscMessage("/address", new IOscArgument[]
            {
                new OscIntArgument(1),
                new OscStringArgument("a")
            }).GetPacketBytes());
    }

    [TestMethod]
    public void ToStringTest()
    {
        Assert.AreEqual("/address ,", new OscMessage("/address").ToString());
        Assert.AreEqual("/address ,is [1] \"a\"", new OscMessage("/address", new IOscArgument[]
        {
            new OscIntArgument(1),
            new OscStringArgument("a")
        }).ToString());
    }
}