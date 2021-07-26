using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Neet.Fighter
{
    [CustomEditor(typeof(InputHandler))]
    public class InputHandlerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            InputHandler _target = (InputHandler)target;

            if (GUILayout.Button("Set default keys"))
                _target.inputSetting = new InputSetting();
        }
    }
}
