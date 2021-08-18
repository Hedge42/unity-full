using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PixelSizeAdjuster))]
[CanEditMultipleObjects]
public class PixelSizeAdjusterEditor : Editor
{
    private PixelSizeAdjuster _target
    {
        get
        {
            return (PixelSizeAdjuster)target;
        }
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();

        // width toggle and field
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Width");
        _target.controlWidth = GUILayout.Toggle(_target.controlWidth, "");

        EditorGUI.BeginDisabledGroup(!_target.controlWidth);
        _target.width = EditorGUILayout.IntField(_target.width);
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

        // height toggle and field
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Height");
        _target.controlHeight = GUILayout.Toggle(_target.controlHeight, "");
        EditorGUI.BeginDisabledGroup(!_target.controlHeight);
        _target.height = EditorGUILayout.IntField(_target.height);
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Update"))
            _target.UpdateSize();
    }

    private void OnSceneGUI()
    {
        _target.UpdateSize();
    }
}
