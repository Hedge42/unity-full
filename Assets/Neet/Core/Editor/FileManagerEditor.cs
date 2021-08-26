using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEditor;

namespace Neat.File
{
    [CustomEditor(typeof(FileManager))]
    public class FileManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            FileManager _target = (FileManager)target;

            if (GUILayout.Button("PersistentDataPath"))
            {
                FileManager.OpenExplorer(FileManager.PersistentDataPath);
            }
            if (GUILayout.Button("DataPath"))
            {
                FileManager.OpenExplorer(FileManager.DataPath);
            }
            if (GUILayout.Button("GameDataFolder"))
            {
                FileManager.OpenExplorer(FileManager.PersistentGameDataFolder);
            }
        }
    }
}
