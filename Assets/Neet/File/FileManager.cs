using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Crosstales.FB;
using System.Collections.Generic;
using Sirenix.OdinInspector;

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

        public enum PathType
        {
            Root,
            DataPath,
            PersistentDataPath,
        }

        public ExtensionFilter filterAudio = new ExtensionFilter("Audio [.mp3, .wav]", new string[] { "*MP3", "*WAV" });
        public ExtensionFilter filterAllFiles = new ExtensionFilter("All files", new string[] { "*.*" });

        /// <summary>
        /// Editor: /Assets
        /// Standalone: /[.exe folder]
        /// </summary>
        public string RootPath
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

        // https://docs.unity3d.com/ScriptReference/Application-dataPath.html
        /// <summary>
        /// Editor: /Assets <br/>
        /// Standalone: [.exe folder]/name_Data
        /// </summary>
        public string DataPath { get { return Application.dataPath; } }

        // https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html
        /// <summary>
        /// AppData/Local/Packages/[productname]/LocalState
        /// </summary>
        public string PersistentDataPath { get { return Application.persistentDataPath; } }
        
        /// <summary>
        /// PersistentDataPath/Settings
        /// </summary>
        public string SettingsPath
        {
            get
            {
                string s = PersistentDataPath + "/Settings/";
                if (!Directory.Exists(s))
                    Directory.CreateDirectory(s);
                return s;
            }
        }

        public string PersistentGameDataFolder
        {
            get
            {
                string s = PersistentDataPath + "/Game/";
                if (!Directory.Exists(s))
                    Directory.CreateDirectory(s);
                return s;
            }
        }

        private string GetPath(PathType type)
        {
            switch (type)
            {
                case PathType.DataPath:
                    return Application.dataPath;
                case PathType.PersistentDataPath:
                    return Application.persistentDataPath;
                case PathType.Root:
                    return RootPath;
                default:
                    return "";
            }
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        [Button]
        public void OpenDataPath()
        {
            string path = GetPath(PathType.DataPath);
            FileBrowser.OpenSingleFolder("Example", path);
        }
        [Button]
        public void OpenPersistentDataPath()
        {
            string path = GetPath(PathType.PersistentDataPath);
            FileBrowser.OpenSingleFolder("Example", path);
        }
        [Button]
        public void OpenRootPath()
        {
            string path = GetPath(PathType.Root);
            FileBrowser.OpenSingleFolder("Example", path);
        }
        [Button]
        public void OpenSettingsPath()
        {
            FileBrowser.OpenSingleFolder("Example", SettingsPath);
        }

        public void OpenExplorer(string path)
        {
            UnityEngine.Debug.Log("Opening " + path);
            System.Diagnostics.Process.Start(path);
        }

        public string GetUniqueFilename(string directory, string filename)
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

        public string OpenFolder(string dialog, PathType type)
        {
            string path = GetPath(type);
            return FileBrowser.OpenSingleFolder(dialog, path);
        }
        public string OpenFile(string dialog, PathType type, ExtensionFilter[] filters)
        {
            string path = GetPath(type);
            return FileBrowser.OpenSingleFile(dialog, path, filters);
        }
        public string OpenFile(string dialog, PathType type)
        {
            string path = GetPath(type);
            var filter = new ExtensionFilter[] { filterAllFiles };

            return FileBrowser.OpenSingleFile(dialog, path, filter);
        }
        public string OpenAudio(string dialog)
        {
            // example...
            return OpenFile(dialog, PathType.DataPath, new ExtensionFilter[] { filterAudio });
        }
        public string[] GetAudioFiles(string folder)
        {
            // FIXME
            return null;
        }

        public string[] SanitizeAudio(string[] paths)
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
        public bool IsDirectory(string path)
        {
            // https://stackoverflow.com/questions/1395205/better-way-to-check-if-a-path-is-a-file-or-a-directory
            // determine if the string is a directory
            FileAttributes attr = System.IO.File.GetAttributes(path);
            return attr.HasFlag(FileAttributes.Directory);
        }

        public void SerializeBinary(object file, string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Create);
            bf.Serialize(fs, file);

            fs.Close();
        }
        public void DeserializeBinary<T>(out T file, string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    file = (T)bf.Deserialize(fs);
                }
            }
            catch
            {
                file = default(T); // ???
                Debug.LogWarning(typeof(T).ToString() + " at " + path + " could not be loaded for some reason or another. x_x");
            }
        }
        // Deerialize from bytes (BinaryFormatter)
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