using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardDeskBuffer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (InHardDriveCollection<string> rep = new InHardDriveCollection<string>(50))
            {
                int size = 100;
                string[] strArr = new string[size];
                for (int i = 0; i < size; i++)
                {
                    strArr[i] = "shfihdfhshfdishfiuhsdfiuhsdfhsdhfhsdifhsidhfishdfihsdifhsihdfihsdfhsufhusd";
                }

                for (int i = 0; i < size; i++)
                {
                    rep.Add(strArr[i]);
                }

                for (int i = 0; i < rep.Count; i++)
                {
                    Console.WriteLine(rep[i] + " index " + i);
                }

                for (int i = 85; i < rep.Count; i++)
                {
                    rep[i] = "ttttttttttttttttttttttttttttttttttttttttttttttttt" + i;
                }

                rep.Remove("ttttttttttttttttttttttttttttttttttttttttttttttttt" + 87);
                for (int i = 80; i < rep.Count; i++)
                {
                    Console.WriteLine(rep[i] + " index " + i);
                }
            }
        }
    }
}
