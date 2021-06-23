using System.Collections.Generic;

namespace Neet.Collections
{
    public interface IList<T> : IEnumerable<T>
    {
        T this[int index] { get; set; }

        /// <summary>
        /// Returns the number of elements in the list.
        /// </summary>
        /// <returns></returns>
        int Count();
        /// <summary>
        /// Returns true if the size is 0
        /// </summary>
        /// <returns></returns>
        bool IsEmpty();
        /// <summary>
        /// Returns true if the list contains the given element
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Contains(T item);
        /// <summary>
        /// Returns the first element in the list
        /// </summary>
        /// <returns></returns>
        T First();
        /// <summary>
        /// Returns the last element in the list
        /// </summary>
        /// <returns></returns>
        T Last();
        /// <summary>
        /// Returns the element at the given index.
        /// Throws out of range excepction if the index is not available
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        T Get(int index);
        /// <summary>
        /// Returns the index of the first item found that is equal to the given element.
        /// Returns -1 if the item is not contained
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int IndexOf(T item);
        /// <summary>
        /// Adds the element to the beginning of the list
        /// </summary>
        /// <param name="item"></param>
        void AddFirst(T item);
        /// <summary>
        /// Adds the element to the end of the list
        /// </summary>
        /// <param name="item"></param>
        void AddLast(T item);
        /// <summary>
        /// Inserts the given element at the given index
        /// Throws out of range exception if index is not available
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        void Insert(int index, T item);
        /// <summary>
        /// Inserts the first element of the array at the given index.
        /// Throws out of range exception if the index is not available.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="items"></param>
        void InsertRange(int index, T[] items);
        /// <summary>
        /// Adds the array to the end of the list
        /// </summary>
        /// <param name="items"></param>
        void AddRange(T[] items);
        /// <summary>
        /// Removes the first element in the list and returns it
        /// </summary>
        /// <returns></returns>
        T RemoveFirst();
        /// <summary>
        /// Removes the last element in the list and returns it
        /// </summary>
        /// <returns></returns>
        T RemoveLast();
        /// <summary>
        /// Removes the element at the given index and returns it
        /// Throws out of range exception if the index is not available
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        T Remove(int index);
        /// <summary>
        /// Removes the element at the given index and returns it
        /// Throws out of range exception if the index is not available
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        T Remove(T item);
        /// <summary>
        /// Removes the range of items beginning at the given index with the given length and returns it
        /// Throws out of range exception if index is not available or if length yields an unavailable index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        T[] RemoveRange(int index, int length);
        /// <summary>
        /// Converts the contents of the list to a standard array
        /// </summary>
        /// <returns></returns>
        T[] ToArray();
    }
}
