using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Neat.Audio
{
    [CustomEditor(typeof(AudioLoaderSerializer))]
    public class AudioLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            AudioLoaderSerializer _target = (AudioLoaderSerializer)target;

            if (GUILayout.Button("Find"))
            {
                AudioLoader.FindAndLoad(_target.dir, _target.source, SayHello);
            }
        }

        private void SayHello()
        {
            Debug.Log("Clip ready to play!");
        }
    }
}