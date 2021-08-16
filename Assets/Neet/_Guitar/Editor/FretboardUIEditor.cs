using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FretboardUI))]
public class FretboardUIEditor : Editor
{
    private FretboardUI fretboard { get { return (FretboardUI)target; } }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // key slider
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Key");
        fretboard.key = EditorGUILayout.IntSlider(fretboard.key, 0, 11);
        EditorGUILayout.LabelField(Scale.NoteValueToName(fretboard.key, fretboard.preferFlats, false), GUILayout.Width(40));
        GUILayout.EndHorizontal();

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

        fretboard.scale = new Scale(fretboard.key, fretboard.mode, fretboard.preferFlats);
        GUILayout.FlexibleSpace();
        GUILayout.Label(fretboard.scale.ToString());

        GUILayout.Space(5);

        if (GUILayout.Button("Generate"))
        {
            fretboard.Display();
        }
    }
}
