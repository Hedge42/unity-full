using Neat.Collections;
using System.Collections;
using System.Collections.Generic;

namespace Neat.Collections
{
    /// <summary>
    /// A doubly linked list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class List<T> : IList<T>, IEnumerable<T>
    {
        private class Node
        {
            private Node next;
            private Node prev;
            private T data;

            public Node(T data)
            {
                this.data = data;
                next = prev = null;
            }
            public T GetData()
            {
                return data;
            }
            public Node GetNext()
            {
                return next;
            }
            public void SetNext(Node node)
            {
                next = node;
            }
            public Node GetPrev()
            {
                return prev;
            }
            public void SetPrev(Node node)
            {
                prev = node;
            }
        }

        // TODO setter
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= size)
                    throw new System.IndexOutOfRangeException("Index does not exist");

                return Get(index);
            }

            set
            {
                if (index < 0 || index >= size)
                    throw new System.IndexOutOfRangeException("Index does not exist");

                Insert(index, value);
            }
        }

        private Node head;
        private Node tail;
        private int size;

        public List()
        {
            head = tail = null;
            size = 0;
        }
        public List(T[] arr)
        {
            if (arr == null)
                throw new System.NullReferenceException();

            if (arr.Length == 0)
                return;

            head = new Node(arr[0]);
            Node current = head;

            for (int i = 1; i < arr.Length; i++)
            {
                Node n = new Node(arr[i]);
                current.SetNext(n);
                n.SetPrev(current);
                current = n;
            }

            tail = current;
            size = arr.Length;
        }

        public void AddFirst(T item)
        {
            if (item == null)
                throw new System.NullReferenceException();

            Node newNode = new Node(item);

            if (IsEmpty())
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                newNode.SetNext(head);
                head.SetPrev(newNode);
                head = newNode;
            }

            size++;
        }
        public void AddLast(T item)
        {
            if (item == null)
                throw new System.NullReferenceException();

            Node newNode = new Node(item);
            if (IsEmpty())
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.SetNext(newNode);
                newNode.SetPrev(tail);
                tail = newNode;
            }

            size++;
        }
        public T First()
        {
            if (head == null)
                throw new System.NullReferenceException();

            return head.GetData();
        }
        public T Last()
        {
            if (tail == null)
                throw new System.NullReferenceException();

            return tail.GetData();
        }
        public bool IsEmpty()
        {
            return size == 0;
        }
        public T RemoveFirst()
        {
            if (head == null)
                throw new System.NullReferenceException();

            T toRemove = head.GetData();
            head = head.GetNext();
            if (head != null)
                head.SetPrev(null);
            size--;
            return toRemove;
        }
        public T RemoveLast()
        {
            if (IsEmpty())
                throw new System.NullReferenceException();

            T toRemove = tail.GetData();
            tail = tail.GetPrev();

            if (tail != null)
                tail.SetNext(null);

            size--;
            return toRemove;
        }
        public int Count()
        {
            return size;
        }
        public bool Contains(T item)
        {
            if (IsEmpty() || item == null)
                return false;

            Node current = head;
            while (current != null)
            {
                // == operator compares reference...
                // .Equals() compares contents...
                if (current.GetData().Equals(item))
                    return true;

                current = current.GetNext();
            }

            return false;
        }
        public T Get(int index)
        {
            if (index >= size || index < 0)
                throw new System.IndexOutOfRangeException();

            // TODO make me efficient
            Node current;
            int midpoint = size / 2;
            if (index > midpoint)
            {
                current = tail;
                for (int i = size - 1; i > index; i--)
                    current = current.GetPrev();
            }
            else
            {
                current = head;
                for (int i = 0; i < index; i++)
                    current = current.GetNext();
            }

            return current.GetData();
        }
        public int IndexOf(T item)
        {
            if (item == null)
                return -1;

            Node current = head;
            for (int i = 0; i < size; i++)
            {
                if (current.GetData() != null && current.GetData().Equals(item))
                    return i;
                current = current.GetNext();
            }

            return -1;
        }
        public void Insert(int index, T item)
        {
            if (index > size || index < 0)
                throw new System.IndexOutOfRangeException();
            if (item == null)
                return;

            // get node just before index'd
            Node current = head;
            for (int i = 0; i < index - 1; i++)
                current = current.GetNext();

            Node n = new Node(item);
            Node next = current.GetNext();
            current.SetNext(n);
            n.SetPrev(current);
            n.SetNext(next);
            next.SetPrev(n);
            size++;
        }
        public void InsertRange(int index, T[] items)
        {
            if (items == null || items.Length == 0)
                return;
            if (index > size || index < 0)
                throw new System.IndexOutOfRangeException();

            // get node before inserted range
            Node current = head;
            for (int i = 0; i < index - 1; i++)
                current = current.GetNext();

            // get node after inserted range
            Node after = current.GetNext();

            // insert range
            for (int i = 0; i < items.Length; i++)
            {
                Node n = new Node(items[i]);
                current.SetNext(n);
                n.SetPrev(current);
                current = current.GetNext();
                size++;
            }

            current.SetNext(after);
            after.SetPrev(current);
        }
        public void AddRange(T[] items)
        {
            foreach (T t in items)
            {
                Node n = new Node(t);
                tail.SetNext(n);
                n.SetPrev(tail);
                tail = n;
                size++;
            }
        }
        public T Remove(int index)
        {
            if (index >= size || index < 0)
                throw new System.IndexOutOfRangeException();

            Node before = head;
            // get node before the one that will be removed
            for (int i = 0; i < index - 1; i++)
                before = before.GetNext();

            T toRemove = before.GetNext().GetData();
            Node after = before.GetNext().GetNext();

            if (before != null)
                before.SetNext(after);

            if (after != null)
                after.SetPrev(before);
            size--;
            return toRemove;
        }
        public T Remove(T item)
        {
            return Remove(IndexOf(item));
        }
        public T[] RemoveRange(int index, int length)
        {
            if (index + length > size || index < 0)
                throw new System.IndexOutOfRangeException();

            T[] toRemove = new T[length];

            // get node before removal range
            Node before = head;
            for (int i = 0; i < index; i++)
                before = before.GetNext();

            // first index to remove
            Node current = before.GetNext();
            for (int i = 0; i < length; i++)
                toRemove[i] = current.GetData();

            // connect before & after nodes
            Node after = current.GetNext();
            after.SetPrev(before);
            before.SetNext(after);
            size -= length;

            return toRemove;
        }
        public T[] ToArray()
        {
            T[] arr = new T[size];
            Node current = head;
            for (int i = 0; i < size; i++)
            {
                arr[i] = current.GetData();
                current = current.GetNext();
            }

            return arr;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;
            while (current != null)
            {
                yield return current.GetData();
                current = current.GetNext();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
