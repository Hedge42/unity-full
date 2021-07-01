using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LineGraph))]
public class LineGraphEditor : Editor
{
    private LineGraph _target => (LineGraph)target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate random"))
        {
            _target.GenerateRandom();
        }
    }
}
