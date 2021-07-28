using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace Neet.File
{
    public class FileManager : MonoBehaviour
    {
        private static FileManager _instance;
        public static FileManager instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<FileManager>();
                return _instance;
            }
            private set { _instance = value; }
        }

        // pathing docs
        // https://docs.unity3d.com/ScriptReference/Application-dataPath.html
        // https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html

        /// <summary>
        /// Editor: /Assets
        /// Standalone: /[.exe folder]
        /// </summary>
        public static string RootPath
        {
            get
            {
#if UNITY_EDITOR
                return Application.dataPath;
#elif UNITY_STANDALONE_WIN
        return Directory.GetParent(Application.dataPath).ToString();
#endif
            }
        }

        /// <summary>
        /// Editor: /Assets <br/>
        /// Standalone: [.exe folder]/name_Data
        /// </summary>
        public static string DataPath { get { return Application.dataPath; } }

        /// <summary>
        /// AppData/Local/Packages/[productname]/LocalState
        /// </summary>
        public static string PersistentDataPath { get { return Application.persistentDataPath; } }
        
        /// <summary>
        /// PersistentDataPath/Settings
        /// </summary>
        public static string SettingsPath
        {
            get
            {
                string s = PersistentDataPath + "/Settings/";
                if (!Directory.Exists(s))
                    Directory.CreateDirectory(s);
                return s;
            }
        }

        /// <summary>
        /// PersistentDataPath/Game/
        /// </summary>
        public static string PersistentGameDataFolder
        {
            get
            {
                string s = PersistentDataPath + "/Game/";
                if (!Directory.Exists(s))
                    Directory.CreateDirectory(s);
                return s;
            }
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public static void OpenExplorer(string path)
        {
            UnityEngine.Debug.Log("Opening " + path);
            System.Diagnostics.Process.Start(path);
        }

        // helper methods
        public static string GetUniqueFilename(string directory, string filename)
        {
            var files = Directory.GetFiles(directory);

            bool isContained = false;
            var split = filename.Split('.');
            var filenameNoExt = split[0];
            var ext = "";
            if (split.Length >= 1) ext = "." + split[1];

            string newfileName = filenameNoExt + ext;

            int count = 0;
            do
            {
                foreach (string file in files)
                {
                    if (file.Equals(newfileName))
                    {
                        newfileName = filename + "_" + count + ext;
                        isContained = true;
                        break;
                    }
                }
            }
            while (isContained);

            return newfileName;
        }
        public static string OpenAudio(string dialog)
        {
            throw new System.NotImplementedException();
        }
        public static string[] GetAudioFiles(string folder)
        {
            throw new System.NotImplementedException();
        }
        public static string[] SanitizeAudio(string[] paths)
        {
            List<string> list = new List<string>();
            foreach (string s in paths)
            {
                string ext = Path.GetExtension(s).ToLower();
                if (ext == ".mp3" || ext == ".wav")
                    list.Add(s);
            }
            return list.ToArray();
        }
        public static bool IsDirectory(string path)
        {
            // https://stackoverflow.com/questions/1395205/better-way-to-check-if-a-path-is-a-file-or-a-directory
            // determine if the string is a directory
            FileAttributes attr = System.IO.File.GetAttributes(path);
            return attr.HasFlag(FileAttributes.Directory);
        }

        // saving and loading
        public static void SerializeBinary(object file, string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Create);
            bf.Serialize(fs, file);
            fs.Close();
        }
        public static void DeserializeBinary<T>(out T file, string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    file = (T)bf.Deserialize(fs);
                }
            }
            catch (Exception e)
            {
                file = default(T); // ???
                Debug.LogWarning(typeof(T).ToString() + " at " + path + " could not be loaded.\n" + e.Message);
            }
        }

        public static T DeserializeFromBytes<T>(byte[] source)
        {
            using (var stream = new MemoryStream(source))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
        public string SerializeJson(object o, string directory, string fileName)
        {
            if (!fileName.EndsWith(".json"))
                fileName += ".json";

            string path = directory + fileName;
            string j = JsonUtility.ToJson(o, true);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(j);
                }
            }
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif

            return path;
        }
        public T DeserializeJson<T>(string path)
        {

            T file = default(T); // ??
            try
            {
                string json = System.IO.File.ReadAllText(path);
                file = (T)JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }

            return file;
        }
        public bool DeserializeJson<T>(string path, out T t)
        {
            t = default(T);
            try
            {
                string json = System.IO.File.ReadAllText(path);
                t = (T)JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                return false;
            }

            return true;
        }
        public T LoadGameObjectJSON<T>(string fileName)
        {
            DeserializeJson<T>(PersistentGameDataFolder + fileName, out T obj);

            return obj;
        }
        public void SaveGameObjectJSON<T>(T obj, string fileName)
        {
            SerializeJson(obj, PersistentGameDataFolder, fileName);
        }
        public T LoadGameObjectBinary<T>(string fileName)
        {
            DeserializeBinary<T>(out T obj, PersistentGameDataFolder + fileName);
            return obj;
        }
        public void SavePersistentGameObjectBinary<T>(T obj, string fileName)
        {
            SerializeBinary(obj, PersistentGameDataFolder + fileName);
        }
    }
}