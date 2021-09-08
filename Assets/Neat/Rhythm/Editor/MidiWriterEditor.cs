using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Note = Melanchall.DryWetMidi.Interaction.Note;
using Melanchall.DryWetMidi.Common;

namespace Neat.Midi
{
	[CustomEditor(typeof(MidiWriter))]
	public class MidiWriterEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			MidiWriter writer = (MidiWriter)target;

			if (GUILayout.Button("Add notes"))
			{
				var m = MidiWriter.Create();

				Note a = new Note((SevenBitNumber)1, 1, 1);
				Note b = new Note((SevenBitNumber)2, 1, 2);
				Note c = new Note((SevenBitNumber)3, 1, 3);

				MidiWriter.AddNote(m, a);
				MidiWriter.AddNote(m, c);
				MidiWriter.AddNote(m, b);

				MidiReader.PrintNotes(m);
			}
		}
	}
}
