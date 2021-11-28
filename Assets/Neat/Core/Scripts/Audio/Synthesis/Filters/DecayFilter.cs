using System;
using UnityEngine;

namespace Neat.Audio.Synthesis
{
    public class DecayFilter : IFilter
    {
        private Synth synth;
        public DecayFilter(Synth s)
        {
            synth = s;
        }
        public float Filter(float f)
        {
            throw new NotImplementedException();
        }
    }
}
