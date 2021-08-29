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
        // StartTime in the song
        [SerializeField] public float offset = 0f;

        // quarter notes per minute
        [SerializeField] public float beatsPerMinute = 120f;

        // quarter notes per second
        public float beatsPerSecond => beatsPerMinute / 60f;

        // 4/4 common time
        public int numerator;
        public int denominator;

        public int SubDivisionsPerBar(int div)
        {
            // EXAMPLES

            // divs = num * (div) / denom
            // 6/8 and 1/2 divs → 6 * 2 / 8 = 1.5 divs
            // 2/2 →

            // num * denom / div
            // 6/8 2s → 6 * 8 / 2 = 24

            // div * 4 / denom
            // 6 * 2 * 4 / 8

            // (num / denom) * div
            // (6 / 8) * (4 * 2) = 6
            // (2 / 2) * (4 * 3) = 12

            return (int)((float)(numerator / denominator) * (4 * div));
        }
        public float TimePerBeatDivision(float div)
        {
            // beatsPerSecond is technically quarter notes (1 div) per scond
            return beatsPerSecond * div;
        }

        public float TimePerBar()
        {
            // EXAMPLES
            // num/dem → (num * (4 / denom)) * quarterNotesPerSecond
            // 4/4 -> (4 * (4 / 4)) = 4 seconds
            // 3/8 -> (3 * (4 / 8)) = 1.5
            // 2/2 -> (2 * (4 / 2)) = 4
            return (numerator * (4f / (float)denominator)) / beatsPerSecond;
        }
        public float TimeOfBar(int barNum)
        {
            return offset + LocalTimeOfBar(barNum);
        }
        public float LocalTimeOfBar(int barNum)
        {
            return barNum + TimePerBar();
        }

        public float TimePerBeat()
        {
            return beatsPerMinute / 60f;
        }
        public float TimeOfBeat(Beat b, int beatDivs)
        {
            return offset + LocalTimeOfBeat(b, beatDivs);
        }
        public float LocalTimeOfBeat(Beat b, int beatDivs)
        {
            return b.bar * TimePerBar()
                + b.beat * TimePerBeat()
                + b.subDiv * TimePerBeatDivision(beatDivs);
        }

        public float DivisionsPerSecond(int div)
        {
            return beatsPerSecond / div;
        }

        public List<Beat> BeatsBetween(float startTime, float endTime, int div)
        {
            var beats = new List<Beat>();

            // item1 or item2?
            var beat = GetAdjacentBeats(startTime, div).Item1;
            var divsBefore = (int)(DivisionsPerSecond(div) * startTime);
            var numDivs = endTime - startTime;

            // next → add ?? add → next
            for (int i = 0; i < numDivs; i++)
            {
                beats.Add(beat);
                beat = NextBeat(beat, div);
            }

            return beats;

        }
        public List<Beat> BarsBetween(float startTime, float endTime)
        {
            var bars = new List<Beat>();
            throw new System.NotImplementedException();
        }
        public (Beat, Beat) GetAdjacentBeats(float time, int subDivs)
        {
            int bar = (int)(time / TimePerBar());
            float remaining = (float)(time - bar);
            int beat = (int)(remaining / TimePerBeat());
            remaining -= beat;
            int subBeat = (int)(remaining / TimePerBeatDivision(subDivs));

            Beat a = new Beat(0f, bar, beat, subBeat);
            a.time = TimeOfBeat(a, subDivs);
            Beat b = a.Clone();

            b.subDiv += 1;
            bool newBeat = b.subDiv / subDivs >= 1;
            if (newBeat)
            {
                b.subDiv %= subDivs;
                b.beat += 1;

                bool newBar = b.beat > SubDivisionsPerBar(1);
                if (newBar)
                {
                    b.beat -= SubDivisionsPerBar(1);
                    b.bar += 1;
                }
            }

            (Beat, Beat) t = (a, b);

            return t;
        }
        public Beat NextBeat(Beat a, int subDivs)
        {
            Beat b = a.Clone();

            b.subDiv += 1;
            bool newBeat = b.subDiv / subDivs >= 1;
            if (newBeat)
            {
                b.subDiv %= subDivs;
                b.beat += 1;

                bool newBar = b.beat > SubDivisionsPerBar(1);
                if (newBar)
                {
                    b.beat -= SubDivisionsPerBar(1);
                    b.bar += 1;
                }
            }

            return b;
        }

        public (Beat, Beat) GetAdjacentBars(float time)
        {
            time = time - offset;

            int barNum = (int)(time / TimePerBar());
            var a = new Beat(time, barNum, 0, 0);
            var b = new Beat(time + TimePerBar(), barNum + 1, 0, 0);
            return (a, b);
        }
        public List<Beat> GetBarsBetween(float startTime, float endTime)
        {
            float localStart = startTime - offset;
            float localEnd = endTime - offset;

            float timePerBar = TimePerBar();

            // the first bar found after startTime
            int barNum = (int)(localStart / timePerBar);



            var bars = new List<Beat>();
            for (; ; barNum++)
            {
                // add bar to list if in range
                float barTime = barNum * timePerBar;
                if (barTime < localEnd)
                    bars.Add(new Beat(barTime + offset, barNum, 0, 0));
                else
                    break;
            }
            return bars;
        }

        public float NextBeatTime(float time, float div)
        {
            throw new System.NotImplementedException();
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
