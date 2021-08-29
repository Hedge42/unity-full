using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Neat.FileManagement;

namespace Neat.Guitar
{
    [CustomEditor(typeof(ChartPlayer))]
    public class SongScrollerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var _target = (ChartPlayer)target;

            if (GUILayout.Button("Open chart directory"))
            {
            }
            if (GUILayout.Button("Fix width"))
            {
                _target.UpdateWidth();
            }

        }
    }
}
