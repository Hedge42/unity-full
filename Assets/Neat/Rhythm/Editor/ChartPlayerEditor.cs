using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Neat.FileManagement;

namespace Neat.Music
{
    [CustomEditor(typeof(ChartPlayer))]
    public class ChartPlayerEditor : Editor
    {
        private float skip;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var _target = (ChartPlayer)target;

            if (GUILayout.Button("Load"))
            {
                _target.LoadChart(_target.chart);
            }
            if (GUILayout.Button("Remove All"))
            {
                _target.ui.timingBar.DiscardAll();
            }
        }
    }
}
