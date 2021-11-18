using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat.Music
{
    // NOT IMPLEMENTED
    [System.Serializable]
    public class NoteBase
    {
        // literally just a pitch with a name
        public int value { get; private set; }
        public int name { get; private set; }

        public NoteBase(int value)
        {
            this.value %= 12;
        }
        public static NoteBase operator +(NoteBase note, int arg)
        {
            return new NoteBase(note.value + arg);
        }

        private string GetName(bool flat = false)
        {
            return MusicScale.Name(value, flat);
        }
    }

    [System.Serializable]
    public class Note
    {
        // things that are different
        public int fret { get; set; } // offset?
        public int lane { get; set; }
        public int value { get; set; }
        public TimeSpan timeSpan { get; set; }
        public int id;


        public GuitarTuning tuning; // should be a reference
        public Note(GuitarTuning t, int lane, int fret)
        {
            this.tuning = t;
            this.value = tuning.values[lane] + fret;
            this.lane = lane;
            this.fret = fret;
        }
        public Note Clone()
        {
            var gn = new Note(tuning, lane, fret); ;
            gn.timeSpan = timeSpan;

            return gn;
        }

        public bool Overlaps(Note other)
        {
            // are these notes in the same lane?
            bool laneOverlap = other.lane == lane;

            // does the on or off time of A exist in the range of B?
            bool timeOverlap = other.timeSpan.Overlaps(timeSpan);

            return laneOverlap && timeOverlap;
        }
        public bool Overlaps(Note[] other)
        {
            foreach (var n in other)
            {
                if (Overlaps(n))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            // type mismatch
            Note other = (Note)obj;
            if (other == null)
                return base.Equals(obj);

            // equal timespans
            // & fret, lane, value
            return timeSpan.Equals(other.timeSpan)
                && fret == other.fret
                && lane == other.lane
                && value == other.value;
        }
        public override int GetHashCode()
        {
            // VS complained this wasn't here
            return base.GetHashCode();
        }

        public void SetDefaultFretAndTuningIndex()
        {
            int nps = 8; // notes per second - configurable

            // foreach guitar string
            int startNote = tuning.values[0];
            for (int i = 0; i < tuning.values.Length; i++)
            {
                int openString = tuning.values[i];
                int endNote = startNote + nps;

                // if value is in fret range
                if (value >= startNote && value < endNote)
                {
                    // return??
                    fret = value - openString;
                    lane = i;
                    return;
                }
                else
                {
                    // update startNote
                    startNote = endNote;
                }
            }

            // if you get here, ignore the fret range limit
            lane = tuning.numStrings - 1;
            fret = value - tuning.values[lane];
        }
        public void UpdateNote()
        {
            value = tuning[lane] + fret;
        }
        public void UpdateFret()
        {
            // can update octave to prevent frets out of range
            int fret = value - tuning[lane];

            while (fret < 0)
                fret += 12;
            while (fret > 24)
                fret -= 12;

            this.fret = fret;
        }

        // naming and text
        public string Name()
        {
            return MusicScale.Name(value, false);
        }
        public string FullName()
        {
            // octave
            return Name() + (value / 12);
        }
        public string FullTimedName()
        {
            return FullName() + " @ " + timeSpan.label;
        }

        public string FretName()
        {
            return fret + "(" + Name() + ")";
        }
        public string FretFullName()
        {
            return fret + "(" + FullName() + ")";
        }
        public string FullFullName()
        {
            return FretFullName() + " @ " + timeSpan.label;
        }
    }
}
