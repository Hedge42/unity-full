using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Neat.Music
{
    [CustomEditor(typeof(LaneUI))]
    public class LaneUIEditor : Editor
    {


        public override void OnInspectorGUI()
        {
            LaneUI ui = (LaneUI)target;

            base.OnInspectorGUI();

            //EditorGUILayout.IntSlider(ui.)

            if (GUILayout.Button("Initialize guitar strings"))
            {
                ui.TrimGuitarStrings(ui.numStrings);
            }

            if (GUILayout.Button("Scroll to beginning"))
            {
                ui.SkipTo(0f);
            }

            if (GUILayout.Button("Skip to next timing point"))
            {
                ui.SkipTo(ui.beatIndex);
            }

            if (GUILayout.Button("Skip to previous timing point"))
            {
                ui.SkipTo(ui.beatIndex - 1);
            }

            if (GUILayout.Button("Make Timing Points"))
            {
                ui.MakeTimeMarkers();
            }
        }
    }
}