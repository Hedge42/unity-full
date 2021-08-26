﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat.Guitar
{
    [System.Serializable]
    public class TimeSignature
    {
        public float startTime;
        public float bpm;
        public int numerator;
        public int denominator;

        public TimeSignature()
        {
            startTime = 0f;
            bpm = 120f;
            numerator = 4;
            denominator = 4;
        }
    }
}
