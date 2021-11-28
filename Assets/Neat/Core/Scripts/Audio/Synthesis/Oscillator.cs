using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Audio.Synthesis
{
    public class Oscillator : MonoBehaviour
    {
        public const float A4_freq = 440;
        public const int A4_midi = 69; // nice

        // https://newt.phys.unsw.edu.au/jw/notes.html
        // https://www.youtube.com/watch?v=GqHFGMy_51c
        // https://newt.phys.unsw.edu.au/jw/notes.html


        public float sinAmp;
        public float squareAmp;
        public float triAmp;

        private double sampling_frequency;

        [Range(0, 1)]
        public float volume;

        [HideInInspector]
        public float gain = 0;

        public int[] notes;
        private double[] frequencies;
        private double[] increments;
        private double[] phases;

        private void Awake()
        {
            frequencies = new double[notes.Length];
            phases = new double[notes.Length];
            increments = new double[notes.Length];
            UpdateFrequencies();
            UpdateIncrements();

            sampling_frequency = AudioSettings.outputSampleRate;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKey(KeyCode.Space))
                gain = volume;
            else
                gain = 0;
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
                ReadAudioFilter(data, channels, i);
            }
        }

        private void ReadAudioFilter(float[] data, int channels, int i)
        {
            UpdatePhases();

            data[i] = 0;
            foreach (var p in phases)
                data[i] += SinWave(p);

            if (channels == 2)
                data[i + 1] = data[i];
        }

        // onaudiofilterread
        private void UpdateFrequencies()
        {
            for (int i = 0; i < frequencies.Length; i++)
                frequencies[i] = Freq(notes[i]);
        }
        private void UpdateIncrements()
        {
            for (int i = 0; i < increments.Length; i++)
                increments[i] = frequencies[i] * 2f * Mathf.PI / sampling_frequency;
        }
        private void UpdatePhases()
        {
            for (int i = 0; i < phases.Length; i++)
            {
                phases[i] += increments[i];
                if (phases[i] > Mathf.PI * 2)
                    phases[i] = 0;
            }
        }

        // MOVE ME
        private float Freq(int note)
        {
            return Mathf.Pow(2, (float)(note - 69) / 12f) * 440f;
        }

        // MOVE ME
        public float SinWave(double phase)
        {
            return (float)(gain * Mathf.Sin((float)phase)) * sinAmp;
        }
        public float SquareWave(double phase)
        {
            if (gain * Mathf.Sin((float)phase) >= 0 * gain)
            {
                return (float)gain * .6f * squareAmp;
            }
            else
            {
                return (-(float)gain) * .6f * squareAmp;
            }
        }
        public float TriangleWave(double phase)
        {
            return (float)(gain * (double)Mathf.PingPong((float)phase, 1f)) * triAmp;
        }
    }
}