using System.Collections.Generic;

namespace Neat.Music
{
    [System.Serializable]
    public class Note
    {
        public float on;
        public float off;
        public int value;
        public int lane;

        public float duration
        {
            get { return off - on; }
        }

        public Note(int value)
        {
            this.value = value;
        }
        public Note Clone()
        {
            var note = new Note(value);
            note.lane = lane;
            note.on = on;
            note.off = off;
            return note;
        }

        public int Octave()
        {
            return value / 12;
        }

        public string Name(bool preferFlats = true)
        {
            var value = this.value % 12;

            if (value == 0)
                return "C";
            else if (value == 1)
                return preferFlats ? "D" + "b" : "C" + "#";
            else if (value == 2)
                return "D";
            else if (value == 3)
                return preferFlats ? "E" + "b" : "D" + "#";
            else if (value == 4)
                return "E";
            else if (value == 5)
                return "F";
            else if (value == 6)
                return preferFlats ? "G" + "b" : "F" + "#";
            else if (value == 7)
                return "G";
            else if (value == 8)
                return preferFlats ? "A" + "b" : "G" + "#";
            else if (value == 9)
                return "A";
            else if (value == 10)
                return preferFlats ? "B" + "b" : "A" + "#";
            else if (value == 11)
                return "B";
            else
                return "(" + this.value + ")";
        }
        public string FullName(bool preferFlats = true)
        {
            return Name(preferFlats) + Octave();
        }
    }
}
