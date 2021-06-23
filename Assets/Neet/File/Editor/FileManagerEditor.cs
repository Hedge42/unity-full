using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEditor;

namespace Neet.File
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
                FileManager.instance.OpenExplorer(FileManager.instance.PersistentDataPath);
            }
            if (GUILayout.Button("DataPath"))
            {
                FileManager.instance.OpenExplorer(FileManager.instance.DataPath);
            }
            if (GUILayout.Button("GameDataFolder"))
            {
                FileManager.instance.OpenExplorer(FileManager.instance.GameDataFolder);
            }
        }
    }
}
