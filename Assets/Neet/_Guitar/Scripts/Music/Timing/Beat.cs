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
        public enum BeatType
        {
            TimeSignature = 0,
            Measure = 1,
            MajorDivision = 2,
            MinorSubdivision = 3,
        }

        public BeatType beatType
        {
            get;
            private set;
        }

        public TimeSignature signature;

        public float time;
        public float localTime;

        // Measure # in the timing signature
        public int measure;
        public int beatNum;

        // SubBeat # (< quarter notes) in the quater-note beat
        public int beatDiv;

        // derived
        public string measureString
        {
            get
            {
                return (measure + 1) + "_" + (localBeatNum + 1) + "/" + beatDiv;
            }
        }
        public string timeString
        {
            get
            {
                return time.ToString("f3");
            }
        }
        public bool isSignatureStart
        {
            get
            {
                return measure == 0 && beatNum == 0;
            }
        }
        public bool isMeasureStart
        {
            get
            {
                return beatNum % beatDiv == 0;
            }
        }
        public int localBeatNum
        {
            get
            {
                return beatNum % beatDiv;
            }
        }

        public Beat next
        {
            get
            {
                return Next();
            }
        }

        public Beat(TimeSignature t, int beatNum)
        {
            this.beatDiv = t.denominator;
            this.signature = t;
            this.beatNum = beatNum;

            this.measure = beatNum / beatDiv;
            this.localTime = beatNum * t.TimePerDivision(beatDiv);
            this.time = localTime + t.offset;
        }

        public Beat(float localTime, float time, int measure, int beatNum, int beatDiv)
        {
            this.localTime = localTime;
            this.time = time;
            this.measure = measure;
            this.beatNum = beatNum;
            this.beatDiv = beatDiv;
        }
        public Beat Clone()
        {
            return new Beat(localTime, time, measure, beatNum, beatDiv);
        }

        public Beat Next()
        {
            var b = new Beat(signature, beatNum + 1);

            // return the first beat of the next time signature
            // if the next beat's time is greater than it's offset

            if (signature.next != null)
            {
                if (b.time >= signature.next.offset)
                    return signature.next.FirstBeat();
            }

            return b;
        }

        public bool Equals(Beat b)
        {
            if (b == null)
                return false;

            return beatNum == b.beatNum && beatDiv == b.beatDiv;
        }

        public override string ToString()
        {
            return measureString + "(" + timeString + ")";
        }
    }
}
