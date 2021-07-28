using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Reticle))]
public class ReticleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Reticle _target = (Reticle)target;

        if (GUILayout.Button("Save profile"))
            _target.profile.Save();
        if (GUILayout.Button("Load profile"))
            _target.profile = ReticleProfile.Load();

        if (GUI.changed)
            _target.UpdateReticle();
    }
}
