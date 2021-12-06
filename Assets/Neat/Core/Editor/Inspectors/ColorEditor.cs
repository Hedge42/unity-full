using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Neat.Tools.Functions;

[CustomEditor(typeof(ColorOverTime))]
public class ColorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var _target = (ColorOverTime)target;

        // GUIListItem<Color>("Combo Colors", ref _target.comboColors);
    }
}
