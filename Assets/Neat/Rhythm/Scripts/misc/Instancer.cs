using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Experimental
{
    public class Instancer<T>
    {
        // more lists, functionality...
        private List<T> instances;

        public T Main()
        {
            if (instances.Count > 0)
                return instances[0];
            else
                return default(T);
        }

        public Instancer()
        {
            Debug.Log("Makin a new one...");
            instances = new List<T>();
        }
        public void Add(T item)
        {
            instances.Add(item);
        }
        public void Remove(T item)
        {
            instances.Remove(item);
        }
    }
}
