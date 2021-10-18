using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Neat.Functions;

namespace Neat.Music
{
	[CustomEditor(typeof(Fretboard))]
	public class FretboardUIEditor : Editor
	{
		private Fretboard fretboard { get { return (Fretboard)target; } }

		private bool isTuningFoldout;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			GUILayout.Space(7);

			// display preferences
			fretboard.borderMode = (Fret.BorderMode)EditorGUILayout.EnumPopup("Border display", fretboard.borderMode);
			fretboard.fretMode = (Fret.PlayableMode)EditorGUILayout.EnumPopup("Playable display", fretboard.fretMode);

			// fret range
			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Fret range");
			float fMin = (float)fretboard.minFret;
			float fMax = (float)fretboard.maxFret;
			fMin = (float)EditorGUILayout.IntField((int)fMin, GUILayout.Width(40));
			EditorGUILayout.MinMaxSlider(ref fMin, ref fMax, 0, 24);
			fMax = (float)EditorGUILayout.IntField((int)fMax, GUILayout.Width(40));
			fretboard.minFret = (int)fMin;
			fretboard.maxFret = (int)fMax;
			GUILayout.EndHorizontal();


			GUILayout.Space(7);

			if (GUILayout.Button("Generate (show scale)"))
			{
				fretboard.Generate();
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