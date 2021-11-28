using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Neat.GameManager;

namespace Neat.Audio.Music
{
    public class USerializer<T> : ScriptableObject
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
