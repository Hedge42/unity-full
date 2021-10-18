using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Neat.Music
{
    [CustomEditor(typeof(FretUI))]
    public class FretUIEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            FretUI _target = (FretUI)target;

            if (GUILayout.Button("Fade In"))
            {

            }
            else if (GUILayout.Button("Fade Out"))
            {

            }
        }
    }
}