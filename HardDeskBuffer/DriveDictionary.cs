using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace HardDeskBuffer
{
    public class DriveDictionary<T, U> : IDisposable
        where T : IComparable
        where U : ISerializable
    {
        //Private
        private SortedDictionary<T, string> filePaths;
        private string defaultFileName;
        private IFormatter formatter;


        private U GetObjectFromHardDrive(string filePath)
        {
            U result = default(U);
            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                result = (U)formatter.Deserialize(fs);
            }
            return result;
        }

        private void SaveObjectToHardDrive(U obj, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, obj);
            }
        }

        //Public
        public DriveDictionary(string directoryPath, IFormatter formatter)
        {
            filePaths = new SortedDictionary<T, string>();
            this.formatter = formatter;
            this.defaultFileName = "data.tmp";
        }

        public int Count { get; set; }

        public U this[T key]
        {
            get
            {
                return GetObjectFromHardDrive(filePaths[key]);
            }
            set
            {
                if (!filePaths.ContainsKey(key))
                {
                    var newFilePath = Count + defaultFileName;
                    ++Count;
                    filePaths.Add(key, newFilePath);
                }
                SaveObjectToHardDrive(value, filePaths[key]);
            }
        }

        public void Add(T key, U obj)
        {
            var newFilePath = Count + defaultFileName;
            //T lastKey = default(T);
            //filePaths.Keys.ToList().ForEach(x => lastKey = x);
            //var afterLastKey = (T)((dynamic)lastKey + 1);
            filePaths.Add(key, newFilePath);
            SaveObjectToHardDrive(obj, newFilePath);
            ++Count;
        }

        public bool Remove(T key)
        {
            if (filePaths.ContainsKey(key))
            {
                File.Delete(filePaths[key]);
                --Count;
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            foreach (var path in filePaths.Values)
            {
                File.Delete(path);
            }
        }
    }
}
