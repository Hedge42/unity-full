using System;
using UnityEngine;

namespace Neat.Audio.Synthesis
{
    [Serializable]
    public class WaveGenerator
    {
        [Range(0, 1)]
        public float sinAmp;
        [Range(0, 1)]
        public float squareAmp;
        [Range(0, 1)]
        public float triAmp;

        public float Get(double phase)
        {
            return Sin(phase) + Square(phase) + Triangle(phase);
        }

        // saw???
        public float Sin(double phase)
        {
            return (float)(Mathf.Sin((float)phase)) * sinAmp;
        }
        public float Square(double phase)
        {
            if (Mathf.Sin((float)phase) >= 0)
            {
                return .6f * squareAmp;
            }
            else
            {
                return -.6f * squareAmp;
            }
        }
        public float Triangle(double phase)
        {
            return (float)((double)Mathf.PingPong((float)phase, 1f)) * triAmp;
        }
    }
}
