using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suhock.Osc.Arguments;
using System;

namespace Suhock.Osc.Tests;

[TestClass]
public class OscArgumentFactoryTests
{
    [TestMethod]
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