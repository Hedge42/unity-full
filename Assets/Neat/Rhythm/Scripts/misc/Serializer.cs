using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.FileManagement;


namespace Neat.Experimental
{
    public class Serializer<T> : MonoBehaviour
    {
        // this class is an experimental solution to saving data
        // inherit from this class to create a serializer for any serializable type

        [Serializable]
        private class ExampleData
        {
            public List<T> data;
        }

        public T obj;
        public string fileName;
        public string directory;
        public string path => directory + fileName;

        private List<T> objects;

        public void Save()
        {
            FileManager.SerializeBinary(obj, path);
        }
        public List<T> Load()
        {
            FileManager.DeserializeBinary<T>(out T t, path);
            return null;
        }
    }
}