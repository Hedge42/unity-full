using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Neat.Music
{
    [CustomEditor(typeof(ScaleSerializer))]
    public class ScaleSerializerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();

            ScaleSerializer _target = (ScaleSerializer)target;

			// flat preference
			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Prefer Flats");
			_target.scale.preferFlats = EditorGUILayout.Toggle(_target.scale.preferFlats);
			GUILayout.EndHorizontal();

			// key
			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Key");
			_target.scale.key = EditorGUILayout.Popup(_target.scale.key,
				MusicScale.AllNoteNames(_target.scale.preferFlats));
			GUILayout.EndHorizontal();

			// mode
			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Mode");
			_target.scale.mode = EditorGUILayout.Popup(_target.scale.mode,
				_target.scale.AllModeNames());
			GUILayout.EndHorizontal();

			// show scale
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label(_target.scale.ToString());
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
    }
}
