using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Neat.FileManagement;

namespace Neat.Guitar
{
    [CustomEditor(typeof(ChartController))]
    public class ChartPlayerEditor : Editor
    {
        private float judgementPosNormalized;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var _target = (ChartController)target;


            // scrollspeed
            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Distance per second");
            _target.distancePerSecond = EditorGUILayout.Slider(_target.distancePerSecond, 10f, 1000f);
            GUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
                _target.LoadChart(_target.chart);

            if (GUILayout.Button("Load Chart"))
            {
                _target.LoadChart(_target.chart);
            }
            if (GUILayout.Button("Draw beats"))
            {
                _target.OnSkip(0f);
            }

            if (GUILayout.Button("Tick .5"))
            {
                _target.OnTimeTick(0f);
            }
        }
    }
}
