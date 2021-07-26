using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tooltip))]
public class ToolTipEditor : Editor
{
    Tooltip _target => (Tooltip)target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Fix Rect"))
            _target.FixRect();
    }
}
