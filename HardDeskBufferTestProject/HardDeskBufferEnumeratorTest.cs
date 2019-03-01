using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardDeskBuffer;
using System.Collections;

namespace HardDeskBufferTestProject
{
    [TestClass]
    public class HardDeskBufferEnumeratorTest
    {
        [TestMethod]
        public void TestMoveNext()
        {
            //arrange
            HardDeskBuffer.InHardDriveCollection<int> collection = new HardDeskBuffer.InHardDriveCollection<int>(50);
            for (int i = 0; i < 1000; i++)
            {
                collection.Add(i);
            }
            //act
            IEnumerator hardDeskBufferEnumerator = collection.GetEnumerator();

            for (int i = 0; i < 1000; i++)
            {
                //assert
                Assert.AreEqual(true, hardDeskBufferEnumerator.MoveNext());
            }
            Assert.AreEqual(false, hardDeskBufferEnumerator.MoveNext());

        }

        [TestMethod]
        public void TestCurrent()
        {
            //arrange
            HardDeskBuffer.InHardDriveCollection<int> collection = new HardDeskBuffer.InHardDriveCollection<int>(50);
            for (int i = 0; i < 1000; i++)
            {
                collection.Add(i);
            }
            //act
            IEnumerator hardDeskBufferEnumerator = collection.GetEnumerator();

            for (int i = 0; i < 1000; i++)
            {
                hardDeskBufferEnumerator.MoveNext();
                //assert
                Assert.AreEqual(i, hardDeskBufferEnumerator.Current);
            }
            Assert.AreEqual(false, hardDeskBufferEnumerator.MoveNext());

        }

        [TestMethod]
        public void TestReset()
        {
            //arrange
            HardDeskBuffer.InHardDriveCollection<int> collection = new HardDeskBuffer.InHardDriveCollection<int>(50);
            for (int i = 0; i < 1000; i++)
            {
                collection.Add(i);
            }
            //act
            IEnumerator hardDeskBufferEnumerator = collection.GetEnumerator();

            for (int i = 0; i < 1000; i++)
            {
                hardDeskBufferEnumerator.MoveNext();
            }
            hardDeskBufferEnumerator.Reset();
            //assert
            Assert.AreEqual(true, hardDeskBufferEnumerator.MoveNext());
        }
    }
}
