using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Audio.Music
{
    [System.Serializable]
    public class TimeSignature
    {
        [HideInInspector]
        [NonSerialized]
        public TimeSignature next;
        [NonSerialized]
        [HideInInspector]
        public TimeSignature prev;

        // StartTime in the song
        [SerializeField]
        public float offset = 0f;

        // quarter notes per minute
        [SerializeField]
        public float beatsPerMinute = 120f;

        // quarter notes per second
        public float beatsPerSecond => beatsPerMinute / 60f;

        [NonSerialized] // fixes serialization loop
        public TimingMap _timingMap;
        public TimingMap timingMap
        {
            get { return _timingMap; }
            set { _timingMap = value; }
        }

        // 4/4 common time
        [SerializeField]
        [Range(1, 16)]
        public int numerator = 4;
        [SerializeField]
        [Range(1, 16)]
        public int denominator = 4;

        public TimeSignature Clone()
        {
            var ts = new TimeSignature();
            ts.offset = offset;
            ts.numerator = numerator;
            ts.denominator = denominator;
            ts.timingMap = timingMap;
            ts.beatsPerMinute = beatsPerMinute;
            ts.prev = prev;
            ts.next = next;

            return ts;
        }

        public TimeSignature() { }
        public TimeSignature(float offset)
        {
            this.offset = offset;
        }

        public float TimePerDivision(int div)
        {
            // time per beat = 1 / beats per time
            return (4f / div) / beatsPerSecond;
        }
        public float TimePerMeasure()
        {
            return numerator * TimePerDivision(denominator);
        }
        public float DivisionsPerMeasureF(int div)
        {
            return (float)numerator * (float)div / (float)denominator;
        }
        public int DivisionsPerMeasure(int div)
        {
            return numerator * div / denominator;
        }

        public Timing FirstBeat()
        {
            return new Timing(this, 0);
        }

        private void ProcessBeat(Timing b)
        {
            // has beatNum
        }

        public override string ToString()
        {
            var _time = "@" + offset.ToString("f3");
            var _bpm = beatsPerMinute.ToString("f3") + "bpm";
            var _sig = numerator + "/" + denominator;
            return _bpm + " " + _sig + " " + _time;
        }
    }
}
