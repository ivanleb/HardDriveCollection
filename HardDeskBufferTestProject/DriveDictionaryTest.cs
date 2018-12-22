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
        [TestMethod]
        public void TestMethod1()
        {
            DriveDictionary<int, string> dict = new DriveDictionary<int, string>("temp", new BinaryFormatter());

            for (int i = 0; i < iterationNumber; i++)
            {
                dict.Add(i, "testtesttesttesttesttesttesttestvvtesttesttesttest");
            }


        }
    }
}
