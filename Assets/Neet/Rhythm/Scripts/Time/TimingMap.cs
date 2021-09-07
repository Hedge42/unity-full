using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Neat.Music
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
        [NonSerialized]
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

        public Timing FirstBeat
        {
            get
            {
                if (timeSignatures.Count > 0)
                    return new Timing(timeSignatures[0], 0);
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
                if (i > 0)
                    timeSignatures[i].prev = timeSignatures[i - 1];
                if (i < timeSignatures.Count - 1)
                    timeSignatures[i].next = timeSignatures[i + 1];
            }

            // set timing map reference
            foreach (TimeSignature t in timeSignatures)
                t.timingMap = this;


            Debug.Log("Processed " + timeSignatures.Count + " time signatures");
        }

        private List<TimeSignature> Sort()
        {
            bool flag = true;

            while (flag)
            {
                flag = false;

                for (int i = 0; i < timeSignatures.Count - 1; i++)
                {
                    var a = timeSignatures[i];
                    var b = timeSignatures[i + 1];
                    // swap?
                    if (a.offset > b.offset)
                    {
                        var temp = a; // ??????
                        timeSignatures[i] = b;
                        timeSignatures[i + 1] = temp;

                        // raise flag if swap occured
                        flag = true;
                    }
                }
            }

            return timeSignatures;
        }
        public void Add(TimeSignature t)
        {
            timeSignatures.Add(t);
            ProcessSignatures();

            onChange?.Invoke();
        }
        public void Overwrite(TimeSignature _prev, TimeSignature _new)
        {
            int i = timeSignatures.IndexOf(_prev);
            timeSignatures[i] = _new;

            ProcessSignatures();
        }
        public void Remove(TimeSignature t)
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
        public List<Timing> TimingsBetween(float startTime, float endTime)
        {
            List<Timing> beats = new List<Timing>();
            var b = Earliest(startTime);
            while (b.time < endTime)
            {
                beats.Add(b);
                b = b.Next();
            }
            return beats;
        }
        public Timing Earliest(float time)
        {
            // has to have a time signature to run
            if (time < 0) time = 0;

            var ts = GetSignatureAtTime(time);
            if (ts != null)
            {
                float beatF = time / ts.TimePerDivision(ts.denominator);
                int beatNum = (int)beatF;
                return new Timing(ts, beatNum);
            }

            // time is before the first time signature
            else if (timeSignatures.Count > 0)
            {
                return timeSignatures[0].FirstBeat();
            }

            else
                return null;
        }
    }
}
