using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Neat.FileManagement;

namespace Neat.Music
{
    [CustomEditor(typeof(ChartController))]
    public class ChartControllerEditor : Editor
    {
        private float skip;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var _target = (ChartController)target;

            if (GUILayout.Button("Load Chart"))
            {
                _target.LoadChart(_target.chart);
            }
            if (GUILayout.Button("Stop"))
            {
                _target.Stop();
            }

            GUILayout.BeginHorizontal();
            skip = EditorGUILayout.Slider(skip, -10f, 10f);
            if (GUILayout.Button("Add time"))
            {
                _target.SetTime(_target.time + skip);
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Skip forward"))
            {
                _target.SkipForward();
            }
            if (GUILayout.Button("Skip back"))
            {
                _target.SkipBack();
            }
            if (GUILayout.Button("Remove All"))
            {
                _target.ui.timingBar.DiscardAll();
            }
        }
    }
}
