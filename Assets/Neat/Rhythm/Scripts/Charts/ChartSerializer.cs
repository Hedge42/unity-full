using System;
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

        public NoteMap map;

        private void Start()
        {
            Load(chart.filePath);
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

        public string[] GetPaths()
        {
            List<string> paths = new List<string>();
            foreach (string path in Directory.EnumerateFiles(directory))
            {
                // just checking the extension instead of loading the whole thing
                if (Path.GetExtension(path).Equals(ext))
                {
                    paths.Add(path);
                }
            }

            return paths.ToArray();
        }
        public string[] GetFileNames(string[] paths)
        {
            string[] filenames = new string[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                filenames[i] = Path.GetFileName(paths[i]);
            }
            return filenames;
        }
    }
}
