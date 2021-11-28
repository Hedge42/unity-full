using System.Collections;
using System.Collections.Generic;

namespace Neat.Experimental.Collections
{
    public class ArrayList<T> : IList<T>
    {
        private T[] data;

        public ArrayList()
        {
            data = new T[0];
        }
        public ArrayList(T[] items)
        {
            data = items;
        }

        // accessor...
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= data.Length)
                    throw new System.IndexOutOfRangeException("Index does not exist");

                return data[index];
            }

            set
            {
                if (index < 0 || index >= data.Length)
                    throw new System.IndexOutOfRangeException("Index does not exist");

                data[index] = value;
            }
        }

        public void AddFirst(T item)
        {
            if (item == null)
                throw new System.NullReferenceException("Cannot add null item");

            T[] newData = new T[data.Length + 1];
            newData[0] = item;

            for (int i = 0; i < data.Length; i++)
                newData[i + 1] = data[i];

            data = newData;
        }
        public void AddLast(T item)
        {
            if (item == null)
                throw new System.NullReferenceException("Cannot add null item");

            T[] newData = new T[data.Length + 1];

            for (int i = 0; i < data.Length; i++)
                newData[i] = data[i];

            newData[newData.Length - 1] = item;

            data = newData;
        }
        public void Insert(int index, T item)
        {
            if (index > data.Length)
                index = data.Length;
            else if (index < 0)
                index = 0;

            T[] newData = new T[data.Length + 1];

            // copy prior elements
            for (int i = 0; i < index; i++)
                newData[i] = data[i];

            // insert item
            newData[index] = item;

            // copy what remains, offset index
            for (int i = index + 1; i < newData.Length; i++)
                newData[i] = data[i - 1];

            data = newData;
        }
        public void InsertRange(int index, T[] items)
        {
            if (index > data.Length)
                index = data.Length;
            else if (index < 0)
                index = 0;

            T[] newData = new T[data.Length + items.Length];

            // copy prior elements
            for (int i = 0; i < index; i++)
                newData[i] = data[i];

            // insert items
            for (int i = 0; i < items.Length; i++)
                newData[index + i] = items[i];

            // copy what remains, offset index
            for (int i = index + items.Length; i < newData.Length; i++)
                newData[i] = data[i - items.Length];

            data = newData;
        }
        public void AddRange(T[] items)
        {
            T[] newData = new T[data.Length + items.Length];
            for (int i = 0; i < data.Length; i++)
                newData[i] = data[i];

            for (int i = 0; i < items.Length; i++)
                newData[data.Length + i] = items[i];

            data = newData;
        }
        public bool Contains(T item)
        {
            if (item == null)
                throw new System.NullReferenceException("Null item");

            foreach (T t in data)
                if (item.Equals(t))
                    return true;

            return false;
        }
        public T First()
        {
            if (data.Length < 1)
                throw new System.IndexOutOfRangeException("Empty list, item doesn't exist");

            return data[0];
        }
        public T Get(int index)
        {
            if (data.Length - 1 < index)
                throw new System.IndexOutOfRangeException("Index out of range");

            return data[index];
        }
        public bool IsEmpty()
        {
            if (data.Length == 0)
                return true;
            else
                return false;
        }
        public T Last()
        {
            if (IsEmpty())
                throw new System.IndexOutOfRangeException("Empty list, cannot get item.");
            return data[data.Length - 1];
        }
        public T RemoveFirst()
        {
            if (IsEmpty())
                throw new System.NullReferenceException("Empty list. Nothing to remove.");

            T[] newData = new T[data.Length - 1];
            for (int i = 0; i < newData.Length; i++)
                newData[i] = data[i + 1];

            T toReturn = data[0];
            data = newData;
            return toReturn;
        }
        public T RemoveLast()
        {
            if (IsEmpty())
                throw new System.NullReferenceException("Empty list. Nothing to remove.");

            T[] newData = new T[data.Length - 1];
            for (int i = 0; i < newData.Length; i++)
                newData[i] = data[i];

            T toReturn = data[data.Length - 1];
            data = newData;
            return toReturn;
        }
        public T Remove(int index)
        {
            if (index > data.Length)
                index = data.Length - 1;
            else if (index < 0)
                index = 0;

            T toRemove = data[index];

            T[] newData = new T[data.Length - 1];

            // copy prior elements
            for (int i = 0; i < index; i++)
                newData[i] = data[i];

            // copy the rest with offset index
            for (int i = index; i < data.Length - 1; i++)
                newData[i] = data[i + 1];

            data = newData;

            return toRemove;
        }
        public T Remove(T item)
        {
            return Remove(IndexOf(item));
        }
        public T[] RemoveRange(int index, int length)
        {
            if (index > data.Length)
                index = data.Length - 1;
            else if (index < 0)
                index = 0;

            if (length > data.Length - index)
                length = data.Length - index;

            T[] newData = new T[data.Length - length];

            // copy prior elements
            for (int i = 0; i < index; i++)
                newData[i] = data[i];

            // copy removal elements into array to return
            T[] toRemove = new T[length];
            for (int i = index; i < index + length; i++)
                toRemove[i - index] = data[i];

            // copy elements not int range...
            for (int i = index + length; i < data.Length; i++)
                newData[i - length] = data[i];

            data = newData;

            return toRemove;
        }
        public int Count()
        {
            return data.Length;
        }
        public int IndexOf(T item)
        {
            for (int i = 0; i < data.Length; i++)
                if (data[i].Equals(item))
                    return i;

            return -1;
        }
        public T[] ToArray()
        {
            return data;
        }
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < data.Length; i++)
                s += data[i] + "\n";
            return s;
        }

        public IEnumerator<T> GetEnumerator()
        {
            T current = data[0];
            int index = 0;
            while (index < data.Length)
                yield return data[index++];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
