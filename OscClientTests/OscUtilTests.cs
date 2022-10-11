using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Suhock.Osc.Tests;

[TestClass]
public class OscUtilTests
{
    [TestMethod]
    public void AlignOffsetTest()
    {
        Assert.AreEqual(0, OscUtil.AlignOffset(0));
        Assert.AreEqual(4, OscUtil.AlignOffset(1));
        Assert.AreEqual(4, OscUtil.AlignOffset(2));
        Assert.AreEqual(4, OscUtil.AlignOffset(3));
        Assert.AreEqual(4, OscUtil.AlignOffset(4));
        Assert.AreEqual(8, OscUtil.AlignOffset(5));
        Assert.AreEqual(0x7ffffffc, OscUtil.AlignOffset(0x7ffffffb));
    }

    [TestMethod]
    public void ReadByteStringTest()
    {
        CollectionAssert.AreEqual(Array.Empty<byte>(),
            OscUtil.ReadByteString(Array.Empty<byte>(), out var length).ToArray());
        Assert.AreEqual(0, length);

        CollectionAssert.AreEqual(Array.Empty<byte>(),
            OscUtil.ReadByteString(new byte[] { 0, 0, 0, 0 }, out length).ToArray());
        Assert.AreEqual(4, length);

        CollectionAssert.AreEqual(new byte[] { 1 },
            OscUtil.ReadByteString(new byte[] { 1, 0, 0, 0 }, out length).ToArray());
        Assert.AreEqual(4, length);

        CollectionAssert.AreEqual(new byte[] { 1, 2, 3 },
            OscUtil.ReadByteString(new byte[] { 1, 2, 3, 0 }, out length).ToArray());
        Assert.AreEqual(4, length);

        CollectionAssert.AreEqual(new byte[] { 1, 2, 3, 4 },
            OscUtil.ReadByteString(new byte[] { 1, 2, 3, 4, 0, 0, 0, 0 }, out length).ToArray());
        Assert.AreEqual(8, length);
    }

    [TestMethod]
    public void ReadIntTests()
    {
        Assert.AreEqual(0, OscUtil.ReadInt(new byte[] { 0, 0, 0, 0 }, out var length));
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

    [TestMethod]
    public void ReadFloatTests()
    {
        Assert.AreEqual(0.0f, OscUtil.ReadFloat(new byte[] { 0, 0, 0, 0 }, out var length));
        Assert.AreEqual(4, length);

        Assert.AreEqual(1.0f, OscUtil.ReadFloat(new byte[] { 0x3F, 0x80, 0, 0 }, out length));
        Assert.AreEqual(4, length);

        Assert.AreEqual(-1.0f, OscUtil.ReadFloat(new byte[] { 0xBF, 0x80, 0, 0 }, out length));
        Assert.AreEqual(4, length);

        Assert.AreEqual(float.PositiveInfinity,
            OscUtil.ReadFloat(new byte[] { 0x7F, 0x80, 0, 0, 2, 3, 4, 5 }, out length));
        Assert.AreEqual(4, length);
    }

    [TestMethod]
    public void ReadStringTests()
    {
        Assert.AreEqual("", OscUtil.ReadString(new byte[] { 0, 0, 0, 0 }, out var length));
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

    [TestMethod]
    public void ReadBlobTests()
    {
        CollectionAssert.AreEqual(ReadOnlySpan<byte>.Empty.ToArray(),
            OscUtil.ReadBlob(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, out var length).ToArray());
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

    [TestMethod]
    public void WriteIntTest()
    {
        var buffer = new byte[8] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        var length = OscUtil.WriteInt(buffer, 1);
        CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 1, 0xff, 0xff, 0xff, 0xff }, buffer);
        Assert.AreEqual(4, length);

        buffer = new byte[8] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        length = OscUtil.WriteInt(buffer, 0);
        CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0, 0xff, 0xff, 0xff, 0xff }, buffer);
        Assert.AreEqual(4, length);
    }

    [TestMethod]
    public void WriteFloatTest()
    {
        var buffer = new byte[8] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        var length = OscUtil.WriteFloat(buffer, 1.0f);
        CollectionAssert.AreEqual(new byte[] { 0x3f, 0x80, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff }, buffer);
        Assert.AreEqual(4, length);

        buffer = new byte[8] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        length = OscUtil.WriteFloat(buffer, 0.0f);
        CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0, 0xff, 0xff, 0xff, 0xff }, buffer);
        Assert.AreEqual(4, length);
    }

    [TestMethod]
    public void WriteByteStringTest1()
    {
        var buffer = new byte[8] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        var length = OscUtil.WriteString(buffer, Array.Empty<byte>());
        CollectionAssert.AreEqual(new byte[8] { 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff }, buffer);
        Assert.AreEqual(4, length);

        buffer = new byte[8] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        length = OscUtil.WriteString(buffer, new byte[1] { (byte)'a' });
        CollectionAssert.AreEqual(new byte[8] { (byte)'a', 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff }, buffer);
        Assert.AreEqual(4, length);

        buffer = new byte[8] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        length = OscUtil.WriteString(buffer, new byte[4] { (byte)'a', (byte)'b', (byte)'c', (byte)'d' });
        CollectionAssert.AreEqual(
            new byte[8] { (byte)'a', (byte)'b', (byte)'c', (byte)'d', 0x00, 0x00, 0x00, 0x00 },
            buffer);
        Assert.AreEqual(8, length);
    }

    [TestMethod]
    public void WriteByteStringTest2()
    {
        var buffer = new byte[8] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        var length = OscUtil.WriteString(buffer, "");
        CollectionAssert.AreEqual(new byte[8] { 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff }, buffer);
        Assert.AreEqual(4, length);

        buffer = new byte[8] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        length = OscUtil.WriteString(buffer, "a");
        CollectionAssert.AreEqual(new byte[8] { (byte)'a', 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff }, buffer);
        Assert.AreEqual(4, length);

        buffer = new byte[8] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        length = OscUtil.WriteString(buffer, "abcd");
        CollectionAssert.AreEqual(
            new byte[8] { (byte)'a', (byte)'b', (byte)'c', (byte)'d', 0x00, 0x00, 0x00, 0x00 },
            buffer);
        Assert.AreEqual(8, length);
    }

    [TestMethod]
    public void WriteByteBlob()
    {
        var buffer = new byte[12] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        var length = OscUtil.WriteBlob(buffer, Array.Empty<byte>());
        CollectionAssert.AreEqual(
            new byte[12] { 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff },
            buffer);
        Assert.AreEqual(4, length);

        buffer = new byte[12] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        length = OscUtil.WriteBlob(buffer, new byte[1] { (byte)'a' });
        CollectionAssert.AreEqual(
            new byte[12] { 0x00, 0x00, 0x00, 0x01, (byte)'a', 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff },
            buffer);
        Assert.AreEqual(8, length);

        buffer = new byte[12] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        length = OscUtil.WriteBlob(buffer, new byte[4] { (byte)'a', (byte)'b', (byte)'c', (byte)'d' });
        CollectionAssert.AreEqual(
            new byte[12] {
                0x00, 0x00, 0x00, 0x04,
                (byte)'a', (byte)'b', (byte)'c', (byte)'d',
                0xff, 0xff, 0xff, 0xff
            },
            buffer);
        Assert.AreEqual(8, length);
    }
}