using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Neat.Tools;

namespace Neat.Tutorials
{
    [CustomEditor(typeof(GameDataObject))]
    public class GameDataObjectCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                ExtendedEditorWindow.Open(target as GameDataObject);
            }
            base.OnInspectorGUI();
        }
    }
}