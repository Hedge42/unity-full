using UnityEngine;

namespace Neat.Synthesis
{
    public class SynthNote
    {
        public int value;
        public double freq;
        public double increment;
        public double phase;
        public float volume;

        public SynthNote(int value)
        {
            this.value = value;

            // update frequency and increment
            freq = Mathf.Pow(2, (float)(value - 69) / 12f) * 440f;
            increment = freq * 2f * Mathf.PI / AudioSettings.outputSampleRate;
        }

        public void UpdatePhase()
        {
            phase += increment;

            if (phase > Mathf.PI * 2)
                phase = 0;
        }
    }
}
