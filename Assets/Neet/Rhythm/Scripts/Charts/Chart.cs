using System.Collections;
using System.Collections.Generic;
using Neat.FileManagement;
using Neat.Music.Notes;
using UnityEngine;

namespace Neat.Music
{
    [System.Serializable]
    public class Chart
    {
        public static string directory
        {
            get
            {
                return "C:/Users/tyler/Desktop/Charts/";
            }
        }
        public static string ext
        {
            get
            {
                return ".chart";
            }
        }

        public string name = "new_chart";
        public string path => directory + name + ext;
        public string musicPath;

        public TimingMap timingMap;
        public List<Track> tracks;

        public Chart()
        {
            name = "name";
            timingMap = new TimingMap();
            tracks = new List<Track>();
        }

        public static void OpenDirectory()
        {
            FileManagement.FileManager.OpenExplorer(directory);
        }

        public void Save()
        {
            FileManager.SerializeBinary(this, path);
            Debug.Log("Saved " + name + " to " + path);
        }
        public static Chart Load(string path)
        {
            FileManager.DeserializeBinary<Chart>(out Chart c, path);
            return c;
        }
    }
}
