using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Tools;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Devices;
using System.Threading;
using System.Collections;
using Note = Melanchall.DryWetMidi.Interaction.Note;
using Input = UnityEngine.Input;
using Melanchall.DryWetMidi.Composing;

namespace Neet.Guitar
{
	public class MidiWriter : MonoBehaviour
	{
		private MidiFile _file;

		public MidiFile file
		{
			get { return _file; }
			set { _file = value; }
		}

		public static MidiFile Create()
		{
			return new MidiFile();
		}

		public static void AddNote(MidiFile m, Note n)
		{
			m.GetNotes().Add(n);

			// PatternBuilder p = new PatternBuilder().;
		}
	}
}
