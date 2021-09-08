using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Music
{
    // musical time?
    [System.Serializable]
    public class Timing
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
            get
            {
                if (beatNum == 0)
                    return BeatType.TimeSignature;
                else if (beatNum % beatDiv == 0)
                    return BeatType.Measure;
                else
                    return BeatType.MajorDivision;

            }
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
                return (measure + 1) + "_" + (localBeatNum + 1) + "/" + signature.numerator;
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
                return beatNum % signature.numerator == 0;
            }
        }
        public int localBeatNum
        {
            get
            {
                return beatNum % signature.numerator;
            }
        }

        public Timing(TimeSignature t, int beatNum)
        {
            this.beatDiv = t.denominator;
            this.signature = t;
            this.beatNum = beatNum;

            //this.measure = beatNum / beatDiv;
            this.measure = beatNum / t.numerator;
            this.localTime = beatNum * t.TimePerDivision(beatDiv);
            this.time = localTime + t.offset;
        }
        public Timing Clone()
        {
            return new Timing(this.signature, this.beatNum);
        }

        public Timing Next()
        {
            var b = new Timing(signature, beatNum + 1);

            // check if new time signature happens before the beat
            if (signature.next != null)
            {
                if (b.time >= signature.next.offset)
                    return signature.next.FirstBeat();
            }

            return b;
        }
        public Timing Prev()
        {
            if (beatNum > 0)
                return new Timing(signature, beatNum - 1);

            else
            {
                // this must be the first beat in the timing map
                if (signature.prev == null)
                    return this;

                // return the last note of the last time signature
                else
                    return signature.timingMap.Earliest(time);
            }
        }

        public bool Equals(Timing b)
        {
            if (b == null)
                return false;

            return beatNum == b.beatNum && beatDiv == b.beatDiv;
        }
        public override string ToString()
        {
            return measureString + "(" + timeString + ")";
        }


        // test
        public Vector3 Position(float scale)
        {
            return Vector3.right * time * scale;
        }
    }
}
