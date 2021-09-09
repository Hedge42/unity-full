using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Music
{
    // made a class to serialize tunings
    public class GuitarTuning
    {
        public string name;

        public int numStrings { get { return stringValues.Length; } }

        public int[] stringValues;

        public GuitarTuning()
        {
            this.stringValues = new int[] {
                4 + 12 * 2,
                9 + 12 * 2,
                2 + 12 * 3,
                7 + 12 * 3,
                11 + 12 * 3,
                4 + 12 * 4
            };
            this.name = "Standard";
        }

        private static GuitarTuning Standard()
        {
            return new GuitarTuning();
        }

        public List<Note> Notes()
        {
            List<Note> _notes = new List<Note>();
            for (int i = 0; i < stringValues.Length; i++)
                _notes.Add(new Note(stringValues[i], i, 0));
            return _notes;
        }
    }
}