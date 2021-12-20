using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Neat.Audio.Music
{
    [CustomEditor(typeof(TimingSpawner))]
    public class TimingSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var _target = (TimingSpawner)target;

            if (GUILayout.Button("Apply snapping"))
            {
                _target.ApplySnapping();
            }
        }
    }
}