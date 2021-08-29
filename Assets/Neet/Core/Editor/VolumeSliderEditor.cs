using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

namespace Neat.Audio
{
    [CustomEditor(typeof(MixerSlider))]
    public class VolumeSliderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();

            // create popup of all mixers

            var _target = (MixerSlider)target;

            string[] names = new string[3];
            names[0] = "Master";
            names[1] = "Music";
            names[2] = "SFX";

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Mixer channel");
            _target.targetMixerGroup = EditorGUILayout.Popup(_target.targetMixerGroup, names);
            GUILayout.EndHorizontal();
        }
    }
}
