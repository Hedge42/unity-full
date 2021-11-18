using System.Collections.Generic;
using UnityEngine;
using Neat.Audio;
using Neat.FileManagement;
using System.IO;

namespace Neat.Music
{
    [ExecuteAlways]
    public class ChartSerializer : MonoBehaviour
    {
        // pathing

        

        public static string directory => "C:/Users/tyler/Desktop/Charts/";
        public static string ext => ".chart";
        public string path => directory + chart.name + ext;

        // data
        private ChartPlayer _controller;
        public ChartPlayer controller
        {
            get
            {
                if (_controller == null)
                    _controller = GetComponent<ChartPlayer>();
                return _controller;
            }
        }

        public Chart chart;
        // public NoteMap map;

        private void Start()
        {
            Load(chart.filePath);
            // Load(chart);
            // controller.LoadChart(chart);
        }
        private void Update()
        {
            // ctrl+s
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public static void OpenDirectory()
        {
            FileManagement.FileManager.OpenExplorer(directory);
        }
        public void Save()
        {
            string fp = chart.filePath;
            chart.filePath = path;

            // reset filepath if invalid save
            if (!FileManager.Serialize(chart, path))
                chart.filePath = fp;
        }
        public void Load(string path)
        {
            if (FileManager.DeserializeBinary<Chart>(out Chart c, path))
            {
                c.filePath = path; // ???
                Load(c);
            }
        }
        public void Load(Chart c)
        {
            chart = c;
            chart.Initialize();
            controller.LoadChart(chart);
        }

        public void Load(int idx)
        {
            Load(paths[idx]);
        }

        public string[] names { get; private set; }


        private string[] paths { get; set; } // valid paths
        private string[] fileNames { get; set; } // literal fileNames (with extensions)
        private int pathIndex { get; set; } // index of selected data

        public string[] RefreshIO()
        {
            this.paths = GetPaths();
            this.fileNames = ConvertToFileNames(paths);

            if (pathIndex <= 0)
            {
                var values = new List<string>(fileNames);
                values.Insert(0, "* " + chart.name);
                this.names = values.ToArray();
            }
            else
            {
                this.names = fileNames;
            }

            return this.names;
        }
        private string[] GetPaths()
        {
            // re-retrive PATHS when... 
            // * data is saved
            // * extension or directory are changed

            List<string> paths = new List<string>();
            int numFiles = 0;
            foreach (string path in Directory.EnumerateFiles(directory))
            {
                // just checking the extension instead of loading the whole thing
                if (Path.GetExtension(path).Equals(ext))
                {
                    // found current data
                    if (Path.Equals(chart.filePath, path))
                        pathIndex = numFiles;

                    paths.Add(path);
                    numFiles++;
                }

            }

            return this.paths = paths.ToArray();
        }
        private string[] ConvertToFileNames(string[] paths)
        {
            // generate names by paths
            // change name at index when chart name changed
            string[] filenames = new string[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                filenames[i] = i + ": " + Path.GetFileName(paths[i]);
            }

            return filenames;
        }
    }
}
