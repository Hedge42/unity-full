using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShaderComponent))]
public class ShaderComponentEditor : Editor
{
    private ShaderComponent __target;
    private ShaderComponent _target
    {
        get
        {
            if (__target == null)
                __target = (ShaderComponent)target;
            return __target;
        }
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        _target.width = EditorGUILayout.Slider(_target.width, 0, 800);

        //float maxRadius = Mathf.Min(_target.rt.rect.size.x, _target.rt.rect.size.y) / 2f;
        //_target.radius = EditorGUILayout.Slider(_target.radius, 0, maxRadius);

        if (EditorGUI.EndChangeCheck())
        {

        }
    }
}
