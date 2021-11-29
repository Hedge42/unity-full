using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Neat.Tools.Extensions;

namespace Neat.Audio.Music
{
	[CustomEditor(typeof(Fretboard))]
	public class FretboardEditor : Editor
	{
		private Fretboard fretboard { get { return (Fretboard)target; } }

		private bool isTuningFoldout;

		public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //GUILayout.Space(7);
            //FretboardDisplaySetting setting = DrawDisplayPreferences();
            //DrawFretRange(setting);
            //GUILayout.Space(7);

            if (GUILayout.Button("Generate (show scale)"))
            {
                fretboard.Generate();
            }
            else if (GUILayout.Button("Preview Spacing"))
            {
                fretboard.PreviewSpacing();
            }
            else if (GUILayout.Button("Dynamic Spacing"))
            {
                fretboard.DynamicSpacing();
            }
            else if (GUILayout.Button("Show all"))
            {
                fretboard.ShowAll();
            }
            else if (GUILayout.Button("Hide all"))
            {
                fretboard.HideAll();
            }


        }

        

        
    }
}