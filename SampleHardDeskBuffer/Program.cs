using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardDeskBuffer;

namespace SampleHardDeskBuffer
{
    class Program
    {
        static string GenerateString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Enumerable.Range(0, 500000))
            {
                sb.Append(item.ToString());
            }
            return sb.ToString();
        }
        static void Main(string[] args)
        {
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

                for (int i = 6; i < rep.Count; i++)
                {
                    rep[i] = "ttttttttttttttttttttttttttttttttttttttttttttttttt" + i;
                }
                rep.Remove("ttttttttttttttttttttttttttttttttttttttttttttttttt" + 10);
                for (int i = 6; i < rep.Count; i++)
                {
                    Console.WriteLine(rep[i] + " index " + i);
                }
            }
        }
    }
}
