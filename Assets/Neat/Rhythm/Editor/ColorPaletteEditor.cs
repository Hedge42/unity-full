using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// move to core
namespace Neat.Music
{
    [CustomEditor(typeof(ColorPalette))]
    public class ColorPaletteEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ColorPalette _target = (ColorPalette)target;

            if (GUILayout.Button("Generate"))
            {
                _target.colors = _target.Spectrum(_target.startColor, 7);
            }
            if (GUILayout.Button("Alternate"))
            {
                _target.colors = _target.Alternate2(_target.colors);
            }
        }
    }
}
