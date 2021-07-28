using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

/// <summary>
/// This component maintains a constant real-size for an image
/// while scaling the border and rounded corners of an image
/// by re-scaling and adjusting the sizeDelta
/// </summary>

[CustomEditor(typeof(ImageScaler))]
public class ImageScalerEditor : Editor
{
    private ImageScaler _target => (ImageScaler)target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UpdateMe();
    }

    private void ParentUpdate()
    {
        Transform t = _target.transform;
        while (t != null)
        {
            if (t.hasChanged)
            {
                UpdateMe();
                return;
            }
            else
                t = t.parent;
        }
    }

    private void UpdateMe()
    {
        _target.scale = EditorGUILayout.Slider("Scale", 
            _target.scale, ImageScaler.MIN_SCALE, ImageScaler.MAX_SCALE);
        _target.UpdateImage();
    }
}
