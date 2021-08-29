using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Neat.Guitar
{
    [System.Serializable]
    public class TimingMap
    {
        [SerializeField] private List<TimeSignature> _timeSignatures;
        public List<TimeSignature> timeSignatures
        {
            get
            {
                if (_timeSignatures == null)
                    _timeSignatures = new List<TimeSignature>();
                return _timeSignatures;
            }
        }

        //public event Action onChange;
        private UnityEvent _onChange;
        public UnityEvent onChange
        {
            get
            {
                if (_onChange == null)
                    _onChange = new UnityEvent();
                return _onChange;
            }
        }

        public List<TimeSignature> Sort()
        {
            bool flag = true;

            while (flag)
            {
                flag = false;

                for (int i = 0; i < timeSignatures.Count - 1; i++)
                {
                    // swap?
                    var a = timeSignatures[i];
                    var b = timeSignatures[i + 1];
                    if (a.offset > b.offset)
                    {
                        // swap
                        var temp = a;
                        a = b;
                        b = temp;

                        // raise flag if swap occured
                        flag = true;
                    }
                }
            }

            return timeSignatures;
        }
        public void AddTimeSignature(TimeSignature t)
        {
            timeSignatures.Add(t);
            Sort();

            onChange?.Invoke();
        }
        public void RemoveTimeSignature(TimeSignature t)
        {
            timeSignatures.Remove(t);
            onChange?.Invoke();
        }

        public TimeSignature GetSignatureAtTime(float time)
        {
            foreach (var ts in timeSignatures)
            {
                if (time >= ts.offset)
                    return ts;
            }

            return null;
        }
        public TimeSignature GetSignatureAtTime(float time, out int index)
        {
            for (int i = 0; i < timeSignatures.Count; i++)
            {
                if (time >= timeSignatures[i].offset)
                {
                    index = i;
                    return timeSignatures[i];

                }
            }

            index = -1;
            return null;
        }
        public TimeSignature GetSignatureAtIndex(int i)
        {
            if (i >= 0 && i < timeSignatures.Count)
                return timeSignatures[i];
            else
                return null;
        }
        public (TimeSignature, TimeSignature) GetAdjacentSignatures(float time)
        {
            int idx = GetSignatureAtTimeIndex(time);
            var currentSignature = GetSignatureAtIndex(idx);
            var nextSignature = GetSignatureAtIndex(idx + 1);

            return (currentSignature, nextSignature);
        }
        public List<TimeSignature> GetSignaturesBetween(float startTime, float endTime)
        {
            var list = new List<TimeSignature>();

            // loop backward through signatures
            // to ensure previous time signature is included

            GetSignatureAtTime(startTime, out int index);
            for (int i = index; i >= 0 && i < timeSignatures.Count; i++)
            {
                var t = timeSignatures[i];
                list.Add(t);

                // valid range
                if (!(t.offset >= startTime && t.offset < endTime))
                    break;
            }

            // instead
            foreach (var t in timeSignatures)
            {
            }
            return list;
        }
        public List<Beat> GetBarsBetween(float startTime, float endTime)
        {
            var bars = new List<Beat>();

            var ts = GetSignaturesBetween(startTime, endTime);

            for (int i = 0; i < ts.Count; i++)
            {
                float end = endTime;
                float start = startTime;

                // start is startTime or signature offset
                if (i > 0)
                    start = ts[i].offset;

                // end is next time signature offset or duration
                if (i < ts.Count - 1)
                    end = ts[i + 1].offset;


                bars.AddRange(ts[i].GetBarsBetween(start, end));
            }

            return bars;
        }
        public int GetSignatureAtTimeIndex(float time)
        {
            int toReturn = -1;
            for (int i = 0; i < timeSignatures.Count; i++)
            {
                if (time >= timeSignatures[i].offset)
                    toReturn = i;

                else
                    return toReturn;
            }

            return toReturn;
        }
        

        // metronome?
        public float GetNextBeatTime(float time, int div)
        {
            var sigs = GetAdjacentSignatures(time);
            var a = sigs.Item1;
            var b = sigs.Item2;

            if (a == null)
            {
                // no beats found
                if (b == null)
                    return -1f;
                else
                    return sigs.Item2.offset;
            }
            else
            {
                float beatTime = a.NextBeatTime(time, div);

                // there is no next time signature
                // could also return the end of the chart
                if (b == null)
                    return beatTime;
                else
                    return Mathf.Min(beatTime, b.offset);
            }
        }



        List<Beat> AllBars()
        {
            throw new System.NotImplementedException();
            //foreach (var t in timeSignatures)
            //{
            //	var time = t.GetTimeOfBeat()
            //}
        }
        List<Beat> AllBeats(int div)
        {
            throw new System.NotImplementedException();
        }
        void GetBarsBetween(float a, float b, int division) { }





        // class Timing
        // has local & global time
        // has TimeSignature, measure, beat references
        // other data?

        private class Timing
        {
            public float localTime;
            public float time;

            public int measureNum;
            public int beatNum;
            public int beatDivision;
        }

        private List<Timing> timings;

        [System.Serializable]
        private class Tick
        {
            long precision; // per second. 1000 → milliseconds
            long tick;
            float time => tick / precision;
        }
    }
}
