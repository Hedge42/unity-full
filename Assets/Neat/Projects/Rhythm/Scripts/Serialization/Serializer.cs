using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.GameManager;


namespace Neat.Experimental
{
    public class Serializer<T> : MonoBehaviour
    {
        [SerializeField] public T obj;

        public string fileName;
        public string directory;// = FileManager.RootPath;
        public string extension = ".sav";

        public string path => directory + fileName;

        public void Save()
        {
            FileManager.SerializeBinary(obj, path);
        }
        public T Load(string path)
        {


            FileManager.DeserializeBinary(out T file, path, true);
            return file;
        }
    }
}