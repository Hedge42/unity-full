﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// exp
namespace Neat.Audio.Music
{
    [Serializable]
    public class TimeSpan
    {
        public float on;
        public float off;

        public float duration => off - on;

        public string label => on.ToString("f3") + "-" + off.ToString("f3");

        public TimeSpan(float on, float off)
        {
            this.on = on;
            this.off = off;
        }
        public TimeSpan(TimeSpan from)
        {
            this.on = from.on;
            this.off = from.off;
        }
        public TimeSpan(Timing from)
        {
            this.on = from.time;
            this.off = from.Next().time;
            // this.off = from.signature.timingMap.Next(from.time).time;
        }

        public bool Overlaps(TimeSpan other)
        {
            return other.Contains(on) || other.Contains(off);
        }
        public bool Contains(float time)
        {
            return time >= on && time < off;
        }
        public override bool Equals(object obj)
        {
            // timespans are equal if their on and off times
            // are within Epsilon of each other

            // type mismatch
            TimeSpan other = (TimeSpan)obj;
            if (other == null)
                return base.Equals(obj);

            // simpler way to write this?
            bool isOn = Mathf.Abs(on - other.on) < Mathf.Epsilon;
            bool isOff = Mathf.Abs(off - other.off) < Mathf.Epsilon;

            return isOn && isOff;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
