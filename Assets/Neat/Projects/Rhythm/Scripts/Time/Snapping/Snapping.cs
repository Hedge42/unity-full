using System;
using System.Collections.Generic;
using UnityEngine;
using Neat.Tools;

namespace Neat.Audio.Music
{
    [System.Serializable]
    public class Snapping
    {
        public static void Set(string count, string type)
        {
            var timingBar = ChartPlayer.instance.ui.timingBar;
            var timingMap = ChartPlayer.instance.chart.timingMap;
            if (int.TryParse(count, out int result) && result >= 1 && result <= 16)
            {
                if (type.Equals("b"))
                {
                    var snap = new Snapping(timingMap, Setting.Beat, result);
                    timingBar.ApplySnapping(snap);
                }
                else if (type.Equals("m"))
                {
                    var snap = new Snapping(timingMap, Setting.Bar, result);
                    timingBar.ApplySnapping(snap);
                }
                else
                {
                    // invalid
                }
            }
        }

        public enum Setting
        {
            Beat, // draw #div timings every bar
            Bar, // draw #div timings every bar
        }

        public Setting per;
        [Range(1, 16)] public int count = 1;

        private TimingMap map;

        private List<TimeSignature> signatures;

        public Snapping(TimingMap map, Setting s, int count)
        {
            this.per = s;
            this.count = count;
            this.map = map;

            this.signatures = CreateSignatures(map);
        }

        public List<Timing> GetTimings(TimeSpan span)
        {
            return TimingMap.TimingsIn(signatures, span);
        }

        private TimeSignature SetSnapping(TimeSignature ts)
        {
            bool validCount = count > 0 && count <= 32;

            if (!validCount)
                return ts;

            TimeSignature cloned = ts.Clone();

            if (per == Setting.Bar)
            {
                // set count per bar? 
                // 1 per bar = 1/1
                // 2 per bar = 2/2
                // etc...

                cloned.numerator = count;
                cloned.denominator = count;
            }
            else if (per == Setting.Beat)
            {
                // set count per beat?
                // already 1 per beat
                // multiply numerator and denominator

                cloned.numerator *= count;
                cloned.denominator *= count;
            }

            return cloned;
        }
        public List<TimeSignature> CreateSignatures(TimingMap map)
        {
            if (per == Setting.Beat && count == 1)
                return map.signatures;

            var list = new List<TimeSignature>();
            foreach (var t in map.signatures)
                list.Add(SetSnapping(t));
            return list;
        }

        public Timing Earliest(float time)
        {
            return TimingMap.Earliest(signatures, time);
        }
    }
}
