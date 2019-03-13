using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HardDeskBuffer
{
    public class HardDeskBufferEnumerator<T> : IEnumerator<T>
        //where T : ISerializable
    {
        private InHardDriveCollection<T> inHardDriveCollection;
        private int position = -1;
        public HardDeskBufferEnumerator(InHardDriveCollection<T> inHardDriveCollection)
        {
            this.inHardDriveCollection = inHardDriveCollection;
        }

        public object Current => !(position == -1 || position >= inHardDriveCollection.Count) ? inHardDriveCollection[position] : throw new InvalidOperationException();

        T IEnumerator<T>.Current => !(position == -1 || position >= inHardDriveCollection.Count) ? inHardDriveCollection[position] : throw new InvalidOperationException();

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public bool MoveNext() => position++ < inHardDriveCollection.Count - 1;

        public void Reset()
        {
            position = -1;
        }
    }
}
