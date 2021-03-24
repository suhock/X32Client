using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suhock.Osc.Tests
{
    [TestClass()]
    public class OscMessageFactoryTests
    {
        [TestMethod()]
        public void CreateTest()
        {
            var factory = new OscMessageFactory(new OscArgumentFactory());

            var msg = factory.Create("/address");
            Assert.AreEqual("/address", msg.Address);
            Assert.AreEqual(0, msg.Arguments.Count);
        }

        [TestMethod()]
        public void CreateTest1()
        {
            var factory = new OscMessageFactory(new OscArgumentFactory());

            var msg = factory.Create("/address", 1, 1.0f, "a", new byte[] { 1, 2 });
            Assert.AreEqual("/address", msg.Address);
            Assert.AreEqual(1, msg.GetValue<int>(0));
            Assert.AreEqual(1.0f, msg.GetValue<float>(1));
            Assert.AreEqual("a", msg.GetValue<string>(2));
            CollectionAssert.AreEqual(new byte[] { 1, 2 }, msg.GetValue<byte[]>(3));
        }

        [TestMethod()]
        public void CreateTest2()
        {
            var factory = new OscMessageFactory(new OscArgumentFactory());

            var msg = factory.Create(factory.Create("/address", 1, 1.0f, "a", new byte[] { 1, 2 }).GetBytes(),
                out int length);
            Assert.AreEqual("/address", msg.Address);
            Assert.AreEqual(1, msg.GetValue<int>(0));
            Assert.AreEqual(1.0f, msg.GetValue<float>(1));
            Assert.AreEqual("a", msg.GetValue<string>(2));
            CollectionAssert.AreEqual(new byte[] { 1, 2 }, msg.GetValue<byte[]>(3));
            Assert.AreEqual(msg.GetByteCount(), length);
        }
    }
}