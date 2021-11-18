using System.Collections;
using System.Collections.Generic;
using Neat.Experimental;
using UnityEngine;
using UnityEditor;

// move to core
namespace Neat.Music
{
    [CustomEditor(typeof(ColorPaletteUI))]
    public class ColorPaletteEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ColorPaletteUI _target = (ColorPaletteUI)target;

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

    [CustomEditor(typeof(ColorPaletteSerializer))]
    public class ColorPaletteSerializerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var _target = ((ColorPaletteSerializer)target).obj;

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
