using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Neet.Functions.EditorFunctions;

[CustomEditor(typeof(TargetSpawner))]
public class TargetSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TargetSpawner _target = (TargetSpawner)target;


        var refList = new List<UnityEngine.Object>()
        {
            _target.targetPrefab,
            _target.connectorPrefab
        };


        DrawDefaultInspector();

        GUIMinMaxSlider("Distance", ref _target.minDistance, ref _target.maxDistance, .1f, 20f);

        GUIMinMaxSlider("Angle", ref _target.minAngle, ref _target.maxAngle, 0f, Mathf.PI);

        GUIColorLerp("Gizmo Colors", ref _target.gizmoStartColor, ref _target.gizmoEndColor);

        GUIListItem<Color>("Combo Colors", ref _target.comboColors);

        if (GUILayout.Button("Generate"))
        {
            _target.ResetTargets();

            RepaintScene();
        }
    }
}