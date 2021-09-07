using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Neat.Music
{
    [CustomEditor(typeof(ChartSerializer))]
    public class ChartSerializerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ChartSerializer _target = (ChartSerializer)target;

            //if (GUILayout.Button("Load"))
            //{
            //    Chart.Load
            //}



            if (GUILayout.Button("Save"))
            {
                _target.chart.Save();
            }
            if (GUILayout.Button("Open directory"))
            {
                Chart.OpenDirectory();
            }
        }
    }
}
