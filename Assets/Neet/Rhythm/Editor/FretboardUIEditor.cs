using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Neat.Functions;

namespace Neat.Music
{
	[CustomEditor(typeof(FretboardUI))]
	public class FretboardUIEditor : Editor
	{
		private FretboardUI fretboard { get { return (FretboardUI)target; } }

		private bool isTuningFoldout;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			GUILayout.Space(7);

			// flat preference
			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Prefer Flats");
			fretboard.scale.preferFlats = EditorGUILayout.Toggle(fretboard.scale.preferFlats);
			GUILayout.EndHorizontal();

			// key
			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Key");
			fretboard.key = EditorGUILayout.Popup(fretboard.key,
				Scale.AllNoteNames(fretboard.preferFlats));
			GUILayout.EndHorizontal();

			// mode
			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Mode");
			fretboard.mode = EditorGUILayout.Popup(fretboard.mode,
				fretboard.scale.AllModeNames());
			GUILayout.EndHorizontal();

			// update scale
			fretboard.scale = new Scale(fretboard.key, fretboard.mode, fretboard.preferFlats);

			// show scale
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label(fretboard.scale.ToString());
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
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

			EditTuning();

			GUILayout.Space(7);

			if (GUILayout.Button("Generate"))
			{
				fretboard.Load();
			}
		}

		private void EditTuning()
		{
			isTuningFoldout = EditorGUILayout.Foldout(isTuningFoldout, "Tuning");
			string[] noteNames = Scale.AllNoteNames(fretboard.preferFlats);

			if (isTuningFoldout)
			{
				// go backward

				int max = fretboard.tuning.numStrings - 1;
				//for (int i = 0; i < fretboard.tuning.numStrings; i++)
				for (int i = max; i >= 0; i--)
				{
					// first 2 buttons
					GUILayout.BeginHorizontal();
					GUILayout.Space(20);
					HandleStringMoveUp(i);

					// to prevent index out of range with changed array
					if (HandleStringRemove(i))
					{
						i -= 1;
						continue;
					}

					// showing dropdown
					int stringValue = fretboard.tuning.stringValues[i];
					int selected = EditorGUILayout.Popup(stringValue, noteNames);
					fretboard.tuning.stringValues[i] = selected;

					// last 2 buttons
					HandleStringAdd(i);
					HandleStringMoveDown(i);
					GUILayout.EndHorizontal();
				}

				GUILayout.Space(10);
			}
		}

		private bool HandleStringRemove(int index)
		{
			if (GUILayout.Button("-", GUILayout.Width(30)))
			{
				List<int> list = new List<int>(fretboard.tuning.stringValues);
				list.RemoveAt(index);
				fretboard.tuning.stringValues = list.ToArray();
				return true;
			}
			return false;
		}
		private void HandleStringAdd(int index)
		{
			if (GUILayout.Button("+", GUILayout.Width(30)))
			{
				List<int> list = new List<int>(fretboard.tuning.stringValues);
				list.Insert(index, fretboard.tuning.stringValues[index]);
				fretboard.tuning.stringValues = list.ToArray();
			}
		}
		private void HandleStringMoveUp(int index)
		{
			EditorGUI.BeginDisabledGroup(index <= 0 || fretboard.tuning.stringValues.Length <= 1);

			if (GUILayout.Button("↑", GUILayout.Width(30)))
			{
				var clicked = fretboard.tuning.stringValues[index];
				var prev = fretboard.tuning.stringValues[index - 1];

				fretboard.tuning.stringValues[index] = prev;
				fretboard.tuning.stringValues[index - 1] = clicked;
			}

			EditorGUI.EndDisabledGroup();
		}

		private void HandleStringMoveDown(int index)
		{
			EditorGUI.BeginDisabledGroup(index >= fretboard.tuning.numStrings - 1 || fretboard.tuning.stringValues.Length <= 1);

			if (GUILayout.Button("↓", GUILayout.Width(30)))
			{
				var clicked = fretboard.tuning.stringValues[index];
				var next = fretboard.tuning.stringValues[index + 1];

				fretboard.tuning.stringValues[index] = next;
				fretboard.tuning.stringValues[index + 1] = clicked;
			}

			EditorGUI.EndDisabledGroup();
		}
	}
}