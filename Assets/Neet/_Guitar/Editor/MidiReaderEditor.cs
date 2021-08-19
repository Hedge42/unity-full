using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Melanchall.DryWetMidi.Devices;
using System.Linq;

namespace Neet.Guitar
{
    [CustomEditor(typeof(MidiReader))]
    public class MidiReaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MidiReader _reader = (MidiReader)target;

            if (GUILayout.Button("Read file"))
            {
                _reader.ReadFile();
            }

            if (GUILayout.Button("Open path"))
            {
                _reader.OpenPath();
            }

            if (GUILayout.Button("Playback"))
            {
                //_reader.Play();
                //MidiReader.Playback(_reader.path);
                //_reader.Play();
            }
            if (GUILayout.Button("stop"))
            {
                //_reader.Stop();
            }

            if (GUILayout.Button("Show output devices"))
            {
                var outputs = OutputDevice.GetAll().ToArray();
                Debug.Log("Devices found: " + outputs.Length);
                foreach (var o in outputs)
                    Debug.Log(o.Name);
            }

        }
    }
}
