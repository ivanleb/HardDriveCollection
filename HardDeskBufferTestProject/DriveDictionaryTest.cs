using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardDeskBuffer;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace HardDeskBufferTestProject
{
    /// <summary>
    /// Summary description for DriveDictionaryTest
    /// </summary>
    [TestClass]
    public class DriveDictionaryTest
    {
        int iterationNumber = 100;
        int bulksize = 20;
        string teststring = "testtesttesttesttesttesttesttestvvtesttesttesttest";
        [TestMethod]
        public void TestMethod1()
        {            
            DriveDictionary<int, Bulk<string>> dict = new DriveDictionary<int, Bulk<string>>("temp", new BinaryFormatter());

            for (int i = 0; i < iterationNumber; i++)
            {
                Bulk<string> bulk = new Bulk<string>(bulksize);
                for (int j = 0; j < bulksize; j++)
                {
                    bulk.Add(teststring);
                }
                dict.Add(i, bulk);
            }

            Assert.AreEqual(dict.Count, iterationNumber);
            dict.Dispose();
        }
    }
}
