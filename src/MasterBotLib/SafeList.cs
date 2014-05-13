using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterBot
{
    public class SafeList<T> : IList<T>
    {
        List<T> list = new List<T>();

        public int IndexOf(T item)
        {
            lock (this)
            {
                return list.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (this)
            {
                list.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (this)
            {
                list.RemoveAt(index);
            }
        }

        public T this[int index]
        {
            get
            {
                lock (this)
                {
                    return list[index]; 
                }
            }
            set
            {
                lock (this)
                {
                    list[index] = value; 
                }
            }
        }

        public void Add(T item)
        {
            lock (this)
            {
                list.Add(item); 
            }
        }

        public void Clear()
        {
            lock (this)
            {
                list.Clear(); 
            }
        }

        public bool Contains(T item)
        {
            lock (this)
            {
                return list.Contains(item); 
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (this)
            {
                list.CopyTo(array, arrayIndex); 
            }
        }

        public int Count
        {
            get
            {
                lock (this)
                {
                    return list.Count; 
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(T item)
        {
            lock (this)
            {
                return list.Remove(item); 
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (this)
            {
                return list.GetEnumerator(); 
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            lock (this)
            {
                return list.GetEnumerator(); 
            }
        }
    }
}
