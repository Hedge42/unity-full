using System;
using UnityEngine;

namespace Neat.Synthesis
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
