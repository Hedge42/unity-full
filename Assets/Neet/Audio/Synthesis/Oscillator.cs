using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    // https://newt.phys.unsw.edu.au/jw/notes.html
    // https://www.youtube.com/watch?v=GqHFGMy_51c
    // https://newt.phys.unsw.edu.au/jw/notes.html

    

    public float sinAmp;
    public float squareAmp;
    public float triAmp;

    public double frequency = 440;
    private double increment;
    private double phase;
    private double sampling_frequency;

    public float gain = 0;

    private void Awake()
    {
        sampling_frequency = AudioSettings.outputSampleRate;
    }


    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2f * Mathf.PI / sampling_frequency;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;

            data[i] = SinWave() + SquareWave() + TriangleWave();

            if (channels == 2)
            {
                data[i + 1] = data[i];
            }
            if (phase > Mathf.PI * 2)
            {
                phase = 0;
            }
        }
    }

    public float SinWave()
    {
        return (float)(gain * Mathf.Sin((float)phase)) * sinAmp;
    }
    public float SquareWave()
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
    public float TriangleWave()
    {
        return (float)(gain * (double)Mathf.PingPong((float)phase, 1f)) * triAmp;
    }
}
