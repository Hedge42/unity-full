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
        [SerializeField] private List<TimeSignature> _signatures;
        public List<TimeSignature> signatures
        {
            get
            {
                if (_signatures == null)
                    _signatures = new List<TimeSignature>();
                return _signatures;
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
                if (signatures.Count > 0)
                    return signatures[0].offset;
                else
                    return 0f;
            }
        }

        public Timing FirstBeat
        {
            get
            {
                if (signatures.Count > 0)
                    return new Timing(signatures[0], 0);
                else
                    return null;
            }
        }

        private void ProcessSignatures()
        {
            Sort();

            // set prev, next
            for (int i = 0; i < signatures.Count; i++)
            {
                if (i > 0)
                    signatures[i].prev = signatures[i - 1];
                if (i < signatures.Count - 1)
                    signatures[i].next = signatures[i + 1];
            }

            // set timing map reference
            foreach (TimeSignature t in signatures)
                t.timingMap = this;


            // Debug.Log("Processed " + signatures.Count + " time signatures");
        }

        private List<TimeSignature> Sort()
        {
            bool flag = true;

            while (flag)
            {
                flag = false;

                for (int i = 0; i < signatures.Count - 1; i++)
                {
                    var a = signatures[i];
                    var b = signatures[i + 1];
                    // swap?
                    if (a.offset > b.offset)
                    {
                        var temp = a; // ??????
                        signatures[i] = b;
                        signatures[i + 1] = temp;

                        // raise flag if swap occured
                        flag = true;
                    }
                }
            }

            return signatures;
        }
        public void Add(TimeSignature t)
        {
            signatures.Add(t);
            ProcessSignatures();

            onChange?.Invoke();
        }
        public void Overwrite(TimeSignature _prev, TimeSignature _new)
        {
            int i = signatures.IndexOf(_prev);
            signatures[i] = _new;

            ProcessSignatures();
        }
        public void Remove(TimeSignature t)
        {
            signatures.Remove(t);
            ProcessSignatures();

            onChange?.Invoke();
        }

        public void SetSnapping(Snapping s)
        {
            var temp = s.CreateSignatures(this);
        }

        public TimeSignature GetSignatureAtTime(float time)
        {
            return GetSignatureAtTime(signatures, time);
        }
        public static TimeSignature GetSignatureAtTime(List<TimeSignature> list, float time)
        {
            if (list != null)
            {
                foreach (var ts in list)
                {
                    if (time >= ts.offset)
                        return ts;
                }
            }

            return null;
        }

        public TimeSignature GetSignatureAtTime(float time, out int index)
        {
            for (int i = 0; i < signatures.Count; i++)
            {
                if (time >= signatures[i].offset)
                {
                    index = i;
                    return signatures[i];
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

        public List<Timing> TimingsIn(TimeSpan span)
        {
            return TimingsBetween(span.on, span.off);
        }
        public static List<Timing> TimingsIn(List<TimeSignature> signatures, TimeSpan span)
        {
            List<Timing> beats = new List<Timing>();
            var b = Earliest(signatures, span.on);
            while (b.time < span.off)
            {
                beats.Add(b);
                b = b.Next();
            }
            return beats;
        }

        public Timing Earliest(float time)
        {
            return Earliest(signatures, time);
        }
        public Timing Next(float time)
        {
            return Next(signatures, time);
        }
        public static Timing Next(List<TimeSignature> signatures, float time)
        {
            // same as earliest, but excludes current timing

            var next = Earliest(signatures, time);
            if (Mathf.Approximately(next.time, time))
                next = next.Next();
            return next;
        }
        public static Timing Earliest(List<TimeSignature> signatures, float time)
        {
            // returns current or next timing
            
            // has to have a time signature to run
            if (time < 0) time = 0; // is this relevant?

            var ts = GetSignatureAtTime(signatures, time);
            if (ts != null)
            {
                var localTime = time - ts.offset;
                float beatF = localTime / ts.TimePerDivision(ts.denominator);
                int beatNum = (int)beatF;
                return new Timing(ts, beatNum);
            }

            // time is before the first time signature
            else if (signatures.Count > 0)
            {
                return signatures[0].FirstBeat();
            }

            else
                return null;
        }
    }
}
