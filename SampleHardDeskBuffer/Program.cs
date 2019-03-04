using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using HardDeskBuffer;

namespace SampleHardDeskBuffer
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct testStruct 
    {
        public int Id;
        public string SomeText;
        public double Value;
    }

    class Program
    {
        static string GenerateString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Enumerable.Range(0, 500))
            {
                sb.Append(item.ToString());
            }
            return sb.ToString();
        }

        static testStruct[] GenerateStructs()
        {
            int count = 100000;
            testStruct[] testStructs = new testStruct[count];
            for (int i = 0; i < testStructs.Length; i++)
            {
                testStructs[i] = new testStruct()
                {
                    Id = i + count,
                    SomeText = "ttttttttttttttttttttttttttttttttttttttttttttttttt",
                    Value = i / Math.PI
                };
            }
            return testStructs;
        }

        static void Main(string[] args)
        {
            int useCase = 0;
            switch (useCase)
            {
                case 0:
                    using (InHardDriveCollection<string> rep = new InHardDriveCollection<string>(40))
                    {
                        int size = 200;
                        string[] strArr = new string[size];
                        for (int i = 0; i < size; i++)
                        {
                            strArr[i] = GenerateString();//"_______________________________" + i + "_______________________________";
                        }

                        for (int i = 0; i < size; i++)
                        {
                            rep.Add(strArr[i]);
                        }
                        for (int i = 0; i < size; i++)
                        {
                            strArr[i] = "";
                        }
                        strArr = null;
                        GC.Collect();
                        //for (int i = 0; i < rep.Count; i++)
                        //{
                        //    Console.WriteLine(rep[i] + " index " + i);
                        //}

                        for (int i = 1; i < rep.Count; i++)
                        {
                            rep[i] = "ttttttttttttttttttttttttttttttttttttttttttttttttt" + i;
                        }
                        rep.Remove("ttttttttttttttttttttttttttttttttttttttttttttttttt" + 10);
                        for (int i = 6; i < rep.Count; i++)
                        {
                            Console.WriteLine(rep[i] + " index " + i);
                        }
                        rep.Insert(rep.Count, "insertedValue");
                        foreach (var item in rep)
                        {
                            Console.WriteLine(item);
                        }

                        //var enumerator = rep.GetEnumerator();
                        //while (enumerator.MoveNext())
                        //{
                        //    Console.WriteLine(enumerator.Current);
                        //}
                        //enumerator.Reset();
                    }
                    break;

                case 1:
                    using (InHardDriveCollection<testStruct> rep = new InHardDriveCollection<testStruct>(40))
                    {
                        testStruct[] testStructs = GenerateStructs();

                        for (int i = 0; i < testStructs.Length; i++)
                        {
                            rep.Add(testStructs[i]);
                        }

                        foreach (var item in rep)
                        {
                            Console.WriteLine(((testStruct)item).Value);
                        }
                    }
                    break;

                default:
                    break;
            }
            Console.ReadKey();
        }
    }
}
