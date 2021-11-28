using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neat.GameManager;
using System.IO;
using UnityEngine;

namespace Neat.Audio.Music
{
    public class TuningSerializer : MonoBehaviour
    {
        // Editor: file:E:\Projects\Unity\NeetDemos\Assets\Neat\Rhythm\Editor\TuningSerializerEditor.cs
        public GuitarTuning tuning;

        public string directory;
        public string ext;
        public string path => directory + tuning.name + ext;

        public void Load(string path)
        {
            if (FileManager.DeserializeBinary(out GuitarTuning t, path, true))
            {
                Load(t);
            }
        }
        public void Load(GuitarTuning tuning)
        {
            this.tuning = tuning;
        }
        public void Save()
        {
            FileManager.Serialize(tuning, path);
        }


        private string[] names;
        private string[] paths;

        private void UpdateNames()
        {

        }
        public string[] GetFileNames()
        {
            if (Directory.Exists(directory))
                return Directory.GetFiles(directory, "*" + ext);
            else
            {
                Debug.LogError("Directory doesn't exist: " + directory);
                return null;
            }
        }
    }
}
