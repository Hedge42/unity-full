using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat.Guitar
{
    [System.Serializable]
    public class Beat
    {
        public float time;

        // Measure # in the timing signature
        public int bar;

        // Beat # (quarter notes) in the measure
        public int beat;

        // SubBeat # (< quarter notes) in the quater-note beat
        public int subDiv;

        public Beat(float time, int bar, int beat, int subDiv)
        {
            this.time = time;
            this.bar = bar;
            this.beat = beat;
            this.subDiv = subDiv;
        }
        public Beat Clone()
        {
            return new Beat(time, bar, beat, subDiv);
        }

        public bool Equals(Beat b)
        {
            return bar == b.bar && beat == b.beat && subDiv == b.subDiv;
        }

        public override string ToString()
        {
            return bar + " - " + beat + "/" + subDiv + " (" + time.ToString("f3") + ")";
        }
    }
}
