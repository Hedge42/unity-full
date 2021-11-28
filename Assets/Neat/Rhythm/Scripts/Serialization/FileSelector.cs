using System;
using System.Collections.Generic;
using System.IO;
using Neat.Experimental;
using UnityEngine;

namespace Neat.Audio.Music
{
    [Serializable]
    public class FileSelector<T> 
    {
        // simplify to not be generic

        public Serializer<T> ser;
        public T file => ser.obj;

        public FileSelector(Serializer<T> s)
        {
            this.ser = s;
        }

        public string directory;// => "C:/Users/tyler/Desktop/Charts/";
        public string ext;// => ".chart";
        public string path => directory + file.ToString() + ext;

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
                values.Insert(0, "* " + file.ToString());
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
                    if (Path.Equals(file.ToString(), path))
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


    public class FileSelectorMono : MonoBehaviour
    {

    }

    [System.Serializable]
    public class FileSelector_
    {
        // reads given 

        // simplify to not be generic

        int idx;
        object selected;

        public string directory;// => "C:/Users/tyler/Desktop/Charts/";
        public string ext;// => ".chart";

        public string GeneratePath(object o)
        {
            return directory + o.ToString() + ext;
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
                values.Insert(0, "* " + selected.ToString());
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
                    if (Path.Equals(GeneratePath(selected), path))
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
