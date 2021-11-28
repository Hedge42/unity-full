using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Neat.Audio.Music
{
    [CustomEditor(typeof(NoteSpawner))]
    public class NoteSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Hello"))
            {

            }
        }
    }
}
