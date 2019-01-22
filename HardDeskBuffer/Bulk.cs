using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace HardDeskBuffer
{
    [Serializable]
    [XmlRootAttribute("Bulk", Namespace = " ", IsNullable = false)]
    public struct Bulk<T> : ISerializable
    {
        //private Bulk(){ size = 0; data = new List<T>(); }
        public int size;

        [XmlIgnoreAttribute]
        public int Count
        {
            get { return data.Count(); }
        }

        public int Add(T entity)
        {
            if (data.Count >= size) return -1;
            data.Add(entity);
            return 1;
        }

        public bool Remove(T entity)
        {
            return data.Remove(entity);
        }
        public void RemoveAt(int i)
        {
            data.RemoveAt(i);
        }

        public int FindIndex(T entity)
        {
            return data.FindIndex(x => x.Equals(entity));
        }

        [XmlIgnoreAttribute]
        public T this[int index]
        {
            get
            {
                if (index >= 0 && index < data.Count)
                {
                    return data[index];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (index >= 0 && index < data.Count)
                {
                    data[index] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public List<T> data;

        public Bulk(int size)
        {
            this.size = size;
            data = new List<T>();
        }

        //two methods for serialization
        // The special constructor is used to deserialize values.
        public Bulk(SerializationInfo info, StreamingContext context)
        {
            this.size = (int)info.GetValue("size", typeof(int));
            data = (List<T>)info.GetValue("data", typeof(List<T>)); ;
        }
        // Implement this method to serialize data. The method is called 
        // on serialization.
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("size", size, typeof(int));
            info.AddValue("data", data, typeof(List<T>));
        }

        public void Clear()
        {
            data.Clear();
        }

    }
}
