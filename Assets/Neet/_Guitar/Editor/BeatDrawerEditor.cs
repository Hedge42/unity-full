using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Neat.Guitar
{
    [CustomEditor(typeof(BeatDrawer))]
    public class BeatDrawerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var _target = (BeatDrawer)target;

            if (GUILayout.Button("Draw"))
            {
                _target.DrawBars(0f, _target.GetComponent<ChartPlayer>().chart.duration);
            }
        }
    }
}
