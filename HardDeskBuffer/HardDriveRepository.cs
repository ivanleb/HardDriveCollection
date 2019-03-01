﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Collections;

namespace HardDeskBuffer
{
    public class InHardDriveCollection<T> : IDisposable, IEnumerable
    {
#if DEBUG
        int addcount = 0;
#endif
        //private
        private DriveDictionary<int, Bulk<T>> Pool;
        private Bulk<T> currentBulk;
        private int currentIndex;
        private readonly int _bufferSize;

        private void changeBulk(int index)
        {
            if (currentIndex != index)
            {
                Pool[currentIndex] = currentBulk;
                currentBulk = Pool[index];
                currentIndex = index;
            }
        }
        /// <summary>
        /// Перестройка контейнера. 
        /// Например после удаления элемента из 
        /// </summary>
        /// <param name="indexFrom">индекс начиная с которого нужно перестраивать</param>
        /// <param name="indexExcept"></param>
        private void rebuildPool(int indexFrom, int indexExcept = -1)
        {
#if DEBUG
            Console.WriteLine("Rebuild Pool: ");
#endif
            if (currentIndex == Pool.Count - 1)
            {
                return;
            }
            for (int i = 0; i < Pool.Count; i++)
            {
                if (i == indexExcept) continue;
                if (currentIndex + 1 != Pool.Count)
                {
                    //взять следующий Bulk с диска
                    Bulk<T> tmpBulk = Pool[currentIndex + 1];

                    //взять из него первый элемент и добавить в конец текущего 
                    currentBulk.Add(tmpBulk[0]);
                    tmpBulk.RemoveAt(0);
                    //положить текущий элемент на диск
                    Pool[currentIndex] = currentBulk;
                    currentBulk = tmpBulk;
                    currentIndex++;
                }
            }
        }
        private void addBulk(Bulk<T> bulk)
        {
            Pool.Add(currentIndex, bulk);// new Bulk<T>(_bufferSize));//add bulk
            currentBulk = new Bulk<T>(_bufferSize);
            ++currentIndex;
        }

        //Public 
        public InHardDriveCollection(int bufferSize)
        {
            _bufferSize = bufferSize;
            currentBulk = new Bulk<T>(_bufferSize);
            
            var formatter = new BinaryFormatter();
            var tp = typeof(Bulk<T>);
            Pool = new DriveDictionary<int, Bulk<T>>(@"\tmp", formatter);
            //Pool.Add(0, currentBulk);//
        }

        public int Count { get; set; }

        public T this[int i]
        {
            get
            {
                if (i >= currentIndex * _bufferSize && i < (currentIndex + 1) * _bufferSize)
                {
                    return currentBulk[i - currentIndex * _bufferSize];
                }
                else if (i < currentIndex * _bufferSize && i >= 0 || i >= (currentIndex + 1) * _bufferSize && i < Count) //  вне текущего bulk-а
                {
                    int index = (int)(Math.Floor((double)i / _bufferSize));
                    changeBulk(index);
                    return currentBulk[i - currentIndex * _bufferSize];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (i >= currentIndex * _bufferSize && i < (currentIndex + 1) * _bufferSize)
                {
                    currentBulk[i - currentIndex * _bufferSize] = value;
                }
                else if (i < currentIndex * _bufferSize && i >= 0 || i >= (currentIndex + 1) * _bufferSize && i < Count) //  вне текущего bulk-а
                {
                    int index = (int)(Math.Floor((double)i / _bufferSize));
                    changeBulk(index);
                    currentBulk[i - currentIndex * _bufferSize] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public void Add(T entity)
        {
            //добавлять только если мы находимся в конце pool-а и текущий Bulk не заполнен до конца
            int maxIndex = Pool.Count == 0 ? 0 : Pool.Count -1;
            if (currentIndex >= maxIndex)
            {
                if (currentBulk.Count < currentBulk.size)
                {
                    currentBulk.Add(entity);
                }
                else
                {
#if DEBUG
                    addcount++;
                    Console.WriteLine(addcount);
#endif
                    addBulk(currentBulk);//Добавить текущий Bulk в Pool
                    //changeBulk(Pool.Count - 1); //переключиться на последний
                    currentBulk.Add(entity);
                }
                ++Count;
            }
            else
            {
                changeBulk(maxIndex);
                Add(entity);
            }
        }

        public bool Remove(T entity)
        {
            //ищем объект во всем Pool-е
            //если он находится в последнем Bulke-е то удаляем просто из него
            //если он находится не в последнем, то переключаемся на нужный Bulk, удаляем из него, 
            //затем перемещаем в него элемент из последующего Bulk-а 
            //и так до тех пор пока незаполненным останется только последний Bulk.
            // если последний bulk получается пустым, то удаляем его
            int maxIndex = Pool.Count;

            int tmpIndex = currentIndex;
            int entityIndex = FindIndex(entity);
            if (entityIndex != -1)
            {
                int entityIndexInPool = (int)Math.Floor((double)entityIndex / _bufferSize);
                changeBulk(entityIndexInPool);
                if (currentIndex == maxIndex)
                {
                    currentBulk.Remove(entity);
                    if (currentBulk.Count == 0)
                    {
                        Pool.Remove(currentIndex);
                        --currentIndex;
                    }
                    --Count;
                    changeBulk(tmpIndex);
                    return true;
                }
                else
                {
                    currentBulk.Remove(entity);
                    rebuildPool(currentIndex);
                    changeBulk(tmpIndex);
                    --Count;
                    return true;
                }
            }
            else return false;
        }

        public int FindIndex(T entity)
        {
            //сначала ищщем в текущем Bulk-е
            //Если нет, то перебираем всю коллекцию, кроме того в котором уже искали
            int tmpIndex = currentIndex;
            int ib = currentBulk.FindIndex(entity);
            if (ib != -1) return ib + currentIndex * _bufferSize;
            else
            {
                for (int i = 0; i < Pool.Count; i++)
                {
                    if (i == tmpIndex) continue;
                    changeBulk(i);
                    int cib = currentBulk.FindIndex(entity);
                    if (cib != -1)
                    {
                        int result = cib + currentIndex * _bufferSize;
                        changeBulk(tmpIndex);
                        return result;
                    }
                }
                return -1;
            }
        }

        public void Dispose()
        {
            Pool.Dispose();
        }

        public void Clear()
        {
            Count = 0;
            currentIndex = 0;
            currentBulk.Clear();
            Pool.Dispose();
        }

        public IEnumerator GetEnumerator()
        {
            return new HardDeskBufferEnumerator<T>(this);
        }
    }
}

