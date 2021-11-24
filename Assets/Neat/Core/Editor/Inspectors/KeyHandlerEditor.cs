using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(KeyHandler))]
public class KeyHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        KeyHandler _target = (KeyHandler)target;

        if (_target.keyDic == null)
            _target.keyDic = new Dictionary<string, List<KeyCode>>();
        if (_target.keys == null)
            _target.keys = new List<string>();
        if (_target.values == null)
            _target.values = new List<List<KeyCode>>();

        if (_target.keys.Count == 0 && GUILayout.Button("+"))
        {
            var newList = new List<KeyCode>();
            newList.Add(default);
            _target.keys.Add("keyEvent");
            _target.values.Add(newList);
        }


        for (int i = 0; i < _target.keys.Count; i++)
        {
            GUILayout.BeginHorizontal();
            if (i >= 1 && GUILayout.Button("-", GUILayout.Width(30)))
            {
                // remove entry
                --i;
                _target.keys.Remove(_target.keys[i]);
                _target.values.Remove(_target.values[i]);
                continue;
            }
            if (i < _target.keys.Count - 1 && GUILayout.Button("\\/", GUILayout.Width(30)))
            {
                var tempKey = _target.keys[i];
                _target.keys[i] = _target.keys[i + 1];
                _target.keys[i + 1] = tempKey;

                var tempValue = _target.values[i];
                _target.values[i] = _target.values[i + 1];
                _target.values[i + 1] = tempValue;
            }

            _target.keys[i] = GUILayout.TextField(_target.keys[i]);


            if (i != 0 && GUILayout.Button("/\\", GUILayout.Width(30)))
            {
                // swap with previous
                var tempKey = _target.keys[i];
                _target.keys[i] = _target.keys[i - 1];
                _target.keys[i - 1] = tempKey;

                var tempValue = _target.values[i];
                _target.values[i] = _target.values[i - 1];
                _target.values[i - 1] = tempValue;
            }
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                // add entry
                var newList = new List<KeyCode>();
                newList.Add(default);
                string newName = "keyEvent";
                string temp = newName;
                int count = 0;
                while (_target.keys.Contains(temp))
                {
                    temp = newName + count;
                    count++;
                }
                newName = temp;
                _target.keys.Add(newName);
                _target.values.Add(newList);
            }
            GUILayout.EndHorizontal();

            _target.RebuildDic();

            // KEYCODE LIST
            var keycodes = _target.keyDic[_target.keyDic.Keys.ToList()[i]];
            for (int j = 0; j < keycodes.Count; j++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(40);
                if (j >= 1 && GUILayout.Button("-", GUILayout.Width(30)))
                {
                    // remove entry
                    keycodes.RemoveAt(j);
                }
                if (j < keycodes.Count - 1 && GUILayout.Button("\\/", GUILayout.Width(30)))
                {
                    // swap with next
                    KeyCode temp = keycodes[j];
                    keycodes[j] = keycodes[j + 1];
                    keycodes[j + 1] = temp;
                }
                keycodes[j] = (KeyCode)EditorGUILayout.EnumPopup(keycodes[j]);
                if (j != 0 && GUILayout.Button("/\\", GUILayout.Width(30)))
                {
                    // swap with previous
                    KeyCode temp = keycodes[j];
                    keycodes[j] = keycodes[j - 1];
                    keycodes[j - 1] = temp;
                }
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    // add entry
                    keycodes.Insert(j + 1, default(KeyCode));
                }
                GUILayout.Space(10);
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(10);
        }
    }
}