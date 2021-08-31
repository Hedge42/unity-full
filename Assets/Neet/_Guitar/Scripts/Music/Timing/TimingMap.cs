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

        public float startTime
        {
            get
            {
                if (timeSignatures.Count > 0)
                    return timeSignatures[0].offset;
                else
                    return 0f;
            }
        }

        public Beat FirstBeat
        {
            get
            {
                if (timeSignatures.Count > 0)
                    return new Beat(timeSignatures[0], 0);
                else
                    return null;
            }
        }

        private void ProcessSignatures()
        {
            Sort();

            // set prev, next
            for (int i = 0; i < timeSignatures.Count; i++)
            {
                if (i != 0)
                    timeSignatures[i].prev = timeSignatures[i - 1];
                if (i < timeSignatures.Count - 1)
                    timeSignatures[i].next = timeSignatures[i + 1];
            }
        }

        private List<TimeSignature> Sort()
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
            ProcessSignatures();

            onChange?.Invoke();
        }
        public void RemoveTimeSignature(TimeSignature t)
        {
            timeSignatures.Remove(t);
            ProcessSignatures();

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

        public List<Beat> _BeatsUntil(float endTime, int beatDiv)
        {
            var beats = new List<Beat>();
            for (int i = 0; i < timeSignatures.Count; i++)
            {
                var t = timeSignatures[i];

                // until time, or until next signature start
                float end = endTime;
                if (i < timeSignatures.Count - 1)
                    end = timeSignatures[i + 1].offset;

                float duration = end - t.offset;
                beats.AddRange(t._BeatsUntil(duration, beatDiv));
            }
            return beats;
        }
        public List<Beat> _BeatsUntil(float endTime)
        {
            var beats = new List<Beat>();
            for (int i = 0; i < timeSignatures.Count; i++)
            {
                var t = timeSignatures[i];

                // until time, or until next signature start
                float end = endTime;
                if (i < timeSignatures.Count - 1)
                    end = timeSignatures[i + 1].offset;

                float duration = end - t.offset;
                beats.AddRange(t._BeatsUntil(duration, t.denominator));
            }
            return beats;
        }
        public List<Beat> _BeatsFrom(float startTime, float duration, int beatDiv)
        {
            var beats = new List<Beat>();
            var t = GetSignatureAtTime(startTime, out int i);
            float endTime = startTime + duration;

            for (; i >= 0 && i < timeSignatures.Count; i++)
            {
                t = timeSignatures[i];

                // until time, or until next signature start
                float end = endTime;
                if (i < timeSignatures.Count - 1)
                    end = timeSignatures[i + 1].offset;

                float _duration = end - t.offset;
                beats.AddRange(t._BeatsUntil(_duration, beatDiv));
            }

            return beats;
        }

        public List<Beat> NextBeatsUntil(float endTime)
        {
            List<Beat> beats = new List<Beat>();

            Beat b = FirstBeat;
            while (b.time < endTime)
                beats.Add(b.next);

            return beats;
        }
        public List<Beat> NextBeatsBetween(float startTime, float endTime)
        {
            List<Beat> beats = new List<Beat>();
            var b = Earliest(startTime);
            while (b.time < endTime)
            {
                beats.Add(b);
                b = b.next;
            }
            return beats;
        }

        public Beat Earliest(float time)
        {
            // has to have a time signature to run
            if (time < 0) time = 0;

            var ts = GetSignatureAtTime(time);
            if (ts != null)
            {
                float beatF = time / ts.TimePerDivision(ts.denominator);
                int beatNum = (int)beatF;
                return new Beat(ts, beatNum);
            }

            // time is before the first time signature
            else if (timeSignatures.Count > 0)
            {
                return timeSignatures[0].FirstBeat();
            }

            else
                return null;
        }


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
