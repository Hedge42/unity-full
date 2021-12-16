using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Melanchall.DryWetMidi.Multimedia;
using System.Linq;
using Melanchall.DryWetMidi.Interaction;
using System;

namespace Neat.Audio.Midi
{
	[CustomEditor(typeof(MidiReader))]
	public class MidiReaderEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			MidiReader _reader = (MidiReader)target;

			var color = GUI.backgroundColor;
			if (_reader.midiFile == null)
				GUI.backgroundColor = new Color(80, 30, 30);
			if (GUILayout.Button("Read file", GUI.skin.button))
			{
				_reader.ReadFile();
			}
			GUI.backgroundColor = color;

			if (GUILayout.Button("Open path"))
			{
				_reader.OpenPath();
			}

			if (GUILayout.Button("Show output devices"))
			{
				var outputs = OutputDevice.GetAll().ToArray();
				Debug.Log("Devices found: " + outputs.Length);
				foreach (var o in outputs)
					Debug.Log(o.Name);
			}


			if (GUILayout.Button("Test note equals"))
			{
				_reader.ReadFile();
				_reader.midiFile.GetNotes().ElementAt(10).TimeChanged += MidiReaderEditor_TimeChanged; ;

				MidiReader.Quantize(_reader.midiFile, _reader.midiFile.GetNotes(), MusicalTimeSpan.Eighth);
			}

			if (GUILayout.Button("Read chunks"))
			{
				_reader.ReadFile();
				MidiReader.ReadChunks(_reader.midiFile);
			}
		}

		private void MidiReaderEditor_TimeChanged(object sender, TimeChangedEventArgs e)
		{
			Note n = (Note) sender;
			Debug.Log(n.NoteNumber + " changed");
		}
	}
}
