using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardDeskBuffer;
using System.Diagnostics;
namespace HardDeskBufferTestProject
{
    [TestClass]
    public class BulkTest
    {
        private int iterationNumber = 10000000;
        [TestMethod]
        public void TestAdd()
        {
            Bulk<int> bulk = new Bulk<int>(iterationNumber);
            for (int i = 0; i < iterationNumber; i++)
            {
                Assert.AreEqual(1, bulk.Add(1));
            }
            Assert.AreEqual(-1, bulk.Add(1));            
        }

        [TestMethod]
        public void TestRemove()
        {
            Bulk<int> bulk = new Bulk<int>(iterationNumber);
            for (int i = 0; i < iterationNumber; i++)
            {
                bulk.Add(1);
            }
            Assert.AreEqual(true, bulk.Remove(1));
        }

        [TestMethod]
        public void TestCount()
        {
            Bulk<int> bulk = new Bulk<int>(iterationNumber);
            for (int i = 0; i < iterationNumber; i++)
            {
                bulk.Add(1);
            }
            Assert.AreEqual(iterationNumber, bulk.Count);
        }

        [TestMethod]
        public void TestFindIndex()
        {
            Bulk<int> bulk = new Bulk<int>(iterationNumber);
            for (int i = 0; i < iterationNumber; i++)
            {
                bulk.Add(i);
            }
            Assert.AreEqual(5, bulk.FindIndex(5));
        }

        [TestMethod]
        public void TestRemoveAt()
        {
            Bulk<int> bulk = new Bulk<int>(iterationNumber);
            for (int i = 0; i < iterationNumber; i++)
            {
                bulk.Add(i);
            }
            bulk.RemoveAt(5);
            Assert.AreEqual(-1, bulk.FindIndex(5));
        }

        [TestMethod]
        public void TestIndexer()
        {
            Bulk<int> bulk = new Bulk<int>(iterationNumber);
            for (int i = 0; i < iterationNumber; i++)
            {
                bulk.Add(i);
            }
            for (int i = 0; i < bulk.Count; i++)
            {
                Assert.AreEqual(i, bulk[i]);
            }
        }
    }
}
