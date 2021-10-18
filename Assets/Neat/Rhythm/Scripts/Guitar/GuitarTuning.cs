using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Music
{
    // made a class to serialize tunings
    [System.Serializable]
    public class GuitarTuning
    {
        public string name;

        public int numStrings { get { return values.Length; } }
        public int[] values;

        public int numFrets = 25; // include open string

        public int this[int index]
        {
            get { return values[index]; }
            set { values[index] = value; }
        }

        public GuitarTuning()
        {
            values = new int[] {
                4 + 12 * 2,
                9 + 12 * 2,
                2 + 12 * 3,
                7 + 12 * 3,
                11 + 12 * 3,
                4 + 12 * 4
            };
            name = "Standard";
        }

        private static GuitarTuning Standard()
        {
            return new GuitarTuning();
        }

        public List<Note> Notes()
        {
            List<Note> _notes = new List<Note>();
            for (int i = 0; i < values.Length; i++)
                _notes.Add(new Note(this, i, 0));
            return _notes;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(name);

            return sb.ToString();
        }
    }
}