using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Guitar
{
    [System.Serializable]
    public class TimeSignature
    {
        [HideInInspector]
        public TimeSignature next;
        [HideInInspector]
        public TimeSignature prev;

        // StartTime in the song
        [SerializeField] public float offset = 0f;

        // quarter notes per minute
        [SerializeField] public float beatsPerMinute = 120f;

        // quarter notes per second
        public float beatsPerSecond => beatsPerMinute / 60f;

        // 4/4 common time
        [SerializeField]
        [Range(1, 16)]
        public int numerator = 4;
        [SerializeField]
        [Range(1,16)]
        public int denominator = 4;

        public float TimePerDivision(int div)
        {
            // div is the denominator...
            // 4 is a quarter note, 8 is an eigth note, 1 is a whole note

            return beatsPerSecond * div / 4f;
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

        public List<Beat> _BeatsUntil(float duration, int beatDiv)
        {
            List<Beat> beats = new List<Beat>();

            // calculate how many divs can exist during this time
            float timePerDiv = TimePerDivision(beatDiv);
            /*int divsPerMeasure = DivisionsPerMeasure(beatDiv);*/
            int divsPerMeasure = DivisionsPerMeasure(beatDiv);
            int numDivs = (int)(duration / timePerDiv);

            for (int i = 0; i < numDivs; i++)
            {
                float localTime = timePerDiv * i;
                if (localTime > duration)
                    break;

                int measureNum =  i / divsPerMeasure;
                int beatNum = i;

                Beat b = new Beat(localTime, localTime + offset, measureNum, beatNum, beatDiv);
                beats.Add(b);
            }

            return beats;
        }
        public List<Beat> _BeatsFrom(float startTime, float duration, int beatDiv)
        {
            List<Beat> beats = new List<Beat>();

            float timePerDiv = TimePerDivision(beatDiv);
            int divsPerMeasure = DivisionsPerMeasure(beatDiv);
            float endTime = startTime + duration;

            // first div after the startTime
            int i = (int)(startTime / timePerDiv) + 1;
            float nextTime = i * timePerDiv;
            while (nextTime < endTime)
            {
                int beatNum = i;
                int measure = i / divsPerMeasure;

                beats.Add(new Beat(nextTime, nextTime + offset, measure, beatNum, beatDiv));

                i += 1;
                nextTime = i * timePerDiv;
            }

            return beats;
        }

        public Beat FirstBeat()
        {
            return new Beat(this, 0);
        }

        private void ProcessBeat(Beat b)
        {
            // has beatNum
        }

        public override string ToString()
        {
            var _time = "(" + offset.ToString("f3") + ")";
            var _bpm = beatsPerMinute.ToString("f3") + "bpm";
            var _sig = numerator + "/" + denominator;
            return _bpm + " " + _sig + " " + _time;
        }
    }
}
