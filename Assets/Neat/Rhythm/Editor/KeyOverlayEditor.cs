﻿using UnityEngine;
using UnityEditor;

namespace Neat.Music
{

    [CustomEditor(typeof(KeyOverlay))]
    public class KeyOverlayEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var _target = (KeyOverlay)target;

            if (GUILayout.Button("Destroy"))
            {
                _target.DestroyUI();
            }
        }
    }
}
