using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Neat.Audio.Music
{
    [CustomEditor(typeof(TuningSerializer))]
    public class TuningSerializerEditor : Editor
    {
        private TuningSerializer _target => (TuningSerializer)target;
        private GuitarTuning tuning => _target.tuning;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            HandleDropdown();
            SaveClick();
            NewClick();
            InfoClick();

            // EditTuning();
        }

        private int selected;
        private void HandleDropdown()
        {
            var names = _target.GetFileNames();
            if (names == null || names.Length == 0)
                return;

            EditorGUI.BeginChangeCheck();
            selected = EditorGUILayout.Popup(selected, names);
            if (EditorGUI.EndChangeCheck())
            {
                _target.Load(names[selected]);
            }
        }
        private void SaveClick()
        {
            if (GUILayout.Button("Save"))
            {
                _target.Save();
            }
        }
        private void InfoClick()
        {
            if (GUILayout.Button("Info?"))
            {
                foreach (var s in _target.GetFileNames())
                {
                    Debug.Log(s);
                }
            }
        }
        private void NewClick()
        {
            if (GUILayout.Button("New"))
            {
                _target.Load(new GuitarTuning());
            }
        }

        private void EditTuning()
        {
            string[] noteNames = MusicScale.AllNoteNames(true);

            int max = tuning.numStrings - 1;
            //for (int i = 0; i < tuning.numStrings; i++)
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
                int stringValue = tuning.values[i];
                int selected = EditorGUILayout.Popup(stringValue, noteNames);
                tuning.values[i] = selected;

                // last 2 buttons
                HandleStringAdd(i);
                HandleStringMoveDown(i);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
        }
        private bool HandleStringRemove(int index)
        {
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                List<int> list = new List<int>(tuning.values);
                list.RemoveAt(index);
                tuning.values = list.ToArray();
                return true;
            }
            return false;
        }
        private void HandleStringAdd(int index)
        {
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                List<int> list = new List<int>(tuning.values);
                list.Insert(index, tuning.values[index]);
                tuning.values = list.ToArray();
            }
        }
        private void HandleStringMoveUp(int index)
        {
            EditorGUI.BeginDisabledGroup(index <= 0 || tuning.values.Length <= 1);

            if (GUILayout.Button("↑", GUILayout.Width(30)))
            {
                var clicked = tuning.values[index];
                var prev = tuning.values[index - 1];

                tuning.values[index] = prev;
                tuning.values[index - 1] = clicked;
            }

            EditorGUI.EndDisabledGroup();
        }
        private void HandleStringMoveDown(int index)
        {
            EditorGUI.BeginDisabledGroup(index >= tuning.numStrings - 1 || tuning.values.Length <= 1);

            if (GUILayout.Button("↓", GUILayout.Width(30)))
            {
                var clicked = tuning.values[index];
                var next = tuning.values[index + 1];

                tuning.values[index] = next;
                tuning.values[index + 1] = clicked;
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}
