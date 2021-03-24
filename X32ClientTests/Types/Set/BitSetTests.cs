using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suhock.X32.Types.Sets.Tests
{
    [TestClass()]
    public class BitSetTests
    {
        [TestMethod()]
        public void BitSetTest()
        {
            BitSet set = new BitSet(4);

            Assert.AreEqual(0, set.Count);
        }

        [TestMethod()]
        public void BitSetTest1()
        {
            BitSet set = new BitSet(4, 6);

            Assert.AreEqual(2, set.Count);
            Assert.IsTrue(set.Contains(2));
            Assert.IsTrue(set.Contains(3));
        }

        [TestMethod()]
        public void BitSetTest2()
        {
            BitSet set = new BitSet(4, new int[] { 1, 4 });

            Assert.AreEqual(2, set.Count);
            Assert.IsTrue(set.Contains(1));
            Assert.IsTrue(set.Contains(4));
        }

        [TestMethod()]
        public void AddTest()
        {
            BitSet set = new BitSet(4);

            Assert.IsTrue(set.Add(1));
            Assert.IsFalse(set.Add(1));
            Assert.AreEqual(1, set.Bits);

            Assert.IsTrue(set.Add(4));
            Assert.IsFalse(set.Add(4));
            Assert.AreEqual(9, set.Bits);
        }

        [TestMethod()]
        public void ClearTest()
        {
            BitSet set = new BitSet(4, 15);

            set.Clear();
            Assert.AreEqual(0, set.Bits);
        }

        [TestMethod()]
        public void ContainsTest()
        {
            BitSet set = new BitSet(4, 2);

            Assert.IsTrue(set.Contains(2));
            Assert.IsFalse(set.Contains(3));
        }

        [TestMethod()]
        public void CopyToTest()
        {
            BitSet set = new BitSet(4, 6);
            int[] result = new int[3];

            set.CopyTo(result, 1);
            CollectionAssert.AreEqual(new int[] { 0, 2, 3 }, result);
        }

        [TestMethod()]
        public void ExceptWithTest()
        {
            BitSet set = new BitSet(4, 15);

            set.ExceptWith(new BitSet(4, 9));
            Assert.AreEqual(6, set.Bits);
        }

        [TestMethod()]
        public void GetEnumeratorTest()
        {
            BitSet set = new BitSet(4, 6);
            int[] test = new int[2] { 2, 3 };
            int index = 0;

            foreach (int x in set)
            {
                Assert.AreEqual(test[index++], x);
            }

            Assert.AreEqual(2, index);
        }

        [TestMethod()]
        public void IntersectWithTest()
        {
            BitSet set1 = new BitSet(4, 6);
            BitSet set2 = new BitSet(4, 2);

            set1.IntersectWith(set2);
            Assert.AreEqual(2, set1.Bits);
        }

        [TestMethod()]
        public void IsProperSubsetOfTest()
        {
            BitSet set1 = new BitSet(4, 6);
            BitSet set2 = new BitSet(4, 2);

            Assert.IsTrue(set2.IsProperSubsetOf(set1));
            Assert.IsFalse(set1.IsProperSubsetOf(set2));

            set2.Add(3);
            Assert.IsFalse(set2.IsProperSubsetOf(set1));

            set1.Clear();
            set2.Clear();
            Assert.IsFalse(set1.IsProperSubsetOf(set2));
        }

        [TestMethod()]
        public void IsProperSupersetOfTest()
        {
            BitSet set1 = new BitSet(4, 6);
            BitSet set2 = new BitSet(4, 2);

            Assert.IsTrue(set1.IsProperSupersetOf(set2));
            Assert.IsFalse(set2.IsProperSupersetOf(set1));

            set2.Add(3);
            Assert.IsFalse(set1.IsProperSupersetOf(set2));

            set1.Clear();
            set2.Clear();
            Assert.IsFalse(set2.IsProperSupersetOf(set1));
        }

        [TestMethod()]
        public void IsSubsetOfTest()
        {
            BitSet set1 = new BitSet(4, 6);
            BitSet set2 = new BitSet(4, 2);

            Assert.IsTrue(set2.IsSubsetOf(set1));
            Assert.IsFalse(set1.IsSubsetOf(set2));

            set2.Add(3);
            Assert.IsTrue(set2.IsSubsetOf(set1));

            set1.Clear();
            set2.Clear();
            Assert.IsTrue(set1.IsSubsetOf(set2));
        }

        [TestMethod()]
        public void IsSupersetOfTest()
        {
            BitSet set1 = new BitSet(4, 6);
            BitSet set2 = new BitSet(4, 2);

            Assert.IsFalse(set2.IsSupersetOf(set1));
            Assert.IsTrue(set1.IsSupersetOf(set2));

            set1.Clear();
            set2.Clear();
            Assert.IsTrue(set1.IsSupersetOf(set2));
        }

        [TestMethod()]
        public void OverlapsTest()
        {
            BitSet set1 = new BitSet(4, 6);
            BitSet set2 = new BitSet(4, 3);

            Assert.IsTrue(set1.Overlaps(set2));
            Assert.IsTrue(set2.Overlaps(set1));

            set2.Remove(2);
            Assert.IsFalse(set1.Overlaps(set2));
            Assert.IsFalse(set2.Overlaps(set1));
        }

        [TestMethod()]
        public void RemoveTest()
        {
            BitSet set = new BitSet(4, 6);

            Assert.IsTrue(set.Remove(2));
            Assert.IsFalse(set.Remove(2));
        }

        [TestMethod()]
        public void SetEqualsTest()
        {
            BitSet set1 = new BitSet(4, 6);
            BitSet set2 = new BitSet(4, 2);

            Assert.IsFalse(set1.SetEquals(set2));

            set2.Add(3);
            Assert.IsTrue(set1.SetEquals(set2));

            set1.Clear();
            set2.Clear();
            Assert.IsTrue(set1.SetEquals(set2));
        }

        [TestMethod()]
        public void SymmetricExceptWithTest()
        {
            BitSet set = new BitSet(4, new int[] { 1, 2, 3 });

            set.SymmetricExceptWith(new BitSet(4, new int[] { 2, 3, 4 }));
            Assert.AreEqual(9, set.Bits);
        }

        [TestMethod()]
        public void UnionWithTest()
        {
            BitSet set1 = new BitSet(4, 4);
            BitSet set2 = new BitSet(4, 2);

            set1.UnionWith(set2);
            Assert.AreEqual(6, set1.Bits);

            set1.UnionWith(set2);
            Assert.AreEqual(6, set1.Bits);
        }
    }
}