using System;
using UnityEngine;

namespace Neat.Audio.Synthesis
{
    public class AttackFilter : IFilter
    {
        private Synth synth;
        public AttackFilter(Synth s)
        {
            synth = s;
        }

        public float Filter(float f)
        {
            throw new NotImplementedException();
        }
    }
}
