using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat.Music
{
    public class TrackNote
    {
        // things that are different
        public int fret { get; set; }
        public int lane { get; set; }
        public int value { get; set; }
        public TimeSpan timeSpan { get; set; }


        public GuitarTuning tuning; // should be a reference
        public TrackNote(GuitarTuning t, int lane, int fret)
        {
            this.tuning = t;
            this.value = tuning.values[lane] + fret;
            this.lane = lane;
            this.fret = fret;
        }

        public TrackNote Clone()
        {
            var gn = new TrackNote(tuning, lane, fret); ;
            gn.timeSpan = timeSpan;

            return gn;
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
            value = tuning.values[lane] + fret;
        }

        // generic naming
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

        // fret naming
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
