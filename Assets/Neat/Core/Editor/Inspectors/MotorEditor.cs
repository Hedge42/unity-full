using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Neat.Tools.Functions;

[CustomEditor(typeof(Motor))]
public class MotorEditor : Editor
{
    private bool ddKeys;

    public override void OnInspectorGUI()
    {
        var _target = (Motor)target;

        base.OnInspectorGUI();

        if (!_target.IsInputActive)
            return;

        ddKeys = EditorGUILayout.Foldout(ddKeys, "Keys");
        if (ddKeys)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("X");
            GUILayout.Label("+", GUILayout.Width(10));
            _target.xpos = (KeyCode)EditorGUILayout.EnumPopup(_target.xpos);
            GUILayout.Label("-", GUILayout.Width(10));
            _target.xneg = (KeyCode)EditorGUILayout.EnumPopup(_target.xneg);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Y");
            GUILayout.Label("+", GUILayout.Width(10));
            _target.ypos = (KeyCode)EditorGUILayout.EnumPopup(_target.ypos);
            GUILayout.Label("-", GUILayout.Width(10));
            _target.yneg = (KeyCode)EditorGUILayout.EnumPopup(_target.yneg);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Z");
            GUILayout.Label("+", GUILayout.Width(10));
            _target.zpos = (KeyCode)EditorGUILayout.EnumPopup(_target.zpos);
            GUILayout.Label("-", GUILayout.Width(10));
            _target.zneg = (KeyCode)EditorGUILayout.EnumPopup(_target.zneg);
            GUILayout.EndHorizontal();
        }
    }
}
