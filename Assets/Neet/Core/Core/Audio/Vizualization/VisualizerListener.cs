using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Audio.Vizualization
{
    // https://www.youtube.com/watch?v=0KqwmcQqg0s
    public class VisualizerListener : MonoBehaviour
    {
        public AudioSource source;
        private float[] samplesLeft = new float[512];
        private float[] samplesRight = new float[512];
        [HideInInspector]
        public float[] freqBand = new float[8];
        [HideInInspector]
        public float[] freqBandBuffer = new float[8];
        [HideInInspector]
        public float[] bufferDecrease = new float[8];
        [HideInInspector]
        public float[] freqBandHighest = new float[8];

        [HideInInspector]
        public float[] freqBand64 = new float[64];
        [HideInInspector]
        public float[] freqBandBuffer64 = new float[64];
        [HideInInspector]
        public float[] bufferDecrease64 = new float[64];
        [HideInInspector]
        public float[] freqBandHighest64 = new float[64];

        [Range(0, .005f)]
        public float bufferValue1;
        [Range(.5f, 2f)]
        public float bufferValue2;
        public float audioProfile;


        [HideInInspector]
        public float[] audioBand, audioBandBuffer;
        [HideInInspector]
        public float[] audioBand64, audioBandBuffer64;

        [HideInInspector]
        public float amplitude, amplitudeBufferred;
        private float amplitudeHighest;

        public enum Channel { Stereo, Left, Right }
        public Channel channel = new Channel();

        private void Start()
        {
            audioBand = new float[8];
            audioBandBuffer = new float[8];
            audioBand64 = new float[64];
            audioBandBuffer64 = new float[64];

            AudioProfile(audioProfile);
        }
        private void Update()
        {
            GetSpecturmAudioSource();

            MakeFrequencyBands();
            MakeFrequencyBands64();

            BandBuffer();
            BandBuffer64();

            CreateAudioBands();
            CreateAudioBands64();

            GetAmplitude();
        }

        private void AudioProfile(float audioProfile)
        {
            for (int i = 0; i < 8; i++)
            {
                freqBandHighest[i] = 0;
            }
        }
        private void GetAmplitude()
        {
            //amplitude = audioBand[0] + audioBand[1]; // ....
            float currentAmplitude = 0;
            float currentAmplitudeBufferred = 0;
            for (int i = 0; i < 8; i++)
            {
                currentAmplitude += audioBand[i];
                currentAmplitudeBufferred += audioBandBuffer[i];

            }

            if (currentAmplitude > amplitudeHighest)
            {
                amplitudeHighest = currentAmplitude;
            }

            amplitude = currentAmplitude / amplitudeHighest;
            amplitudeBufferred = currentAmplitudeBufferred / amplitudeHighest;
        }
        private void CreateAudioBands()
        {
            for (int i = 0; i < 8; i++)
            {
                if (freqBand[i] > freqBandHighest[i])
                {
                    freqBandHighest[i] = freqBand[i];
                }
                audioBand[i] = freqBand[i] / freqBandHighest[i];
                audioBandBuffer[i] = freqBandBuffer[i] / freqBandHighest[i];
            }
        }
        private void CreateAudioBands64()
        {
            for (int i = 0; i < 64; i++)
            {
                if (freqBand64[i] > freqBandHighest64[i])
                {
                    freqBandHighest64[i] = freqBand64[i];
                }
                audioBand64[i] = freqBand64[i] / freqBandHighest64[i];
                audioBandBuffer64[i] = freqBandBuffer64[i] / freqBandHighest64[i];
            }
        }
        private void BandBuffer()
        {
            for (int i = 0; i < 8; i++)
            {
                if (freqBand[i] > freqBandBuffer[i])
                {
                    freqBandBuffer[i] = freqBand[i];
                    bufferDecrease[i] = bufferValue1;
                }
                if (freqBand[i] < freqBandBuffer[i])
                {
                    freqBandBuffer[i] -= bufferDecrease[i];
                    bufferDecrease[i] *= bufferValue2;
                }
            }
        }
        private void BandBuffer64()
        {
            for (int i = 0; i < 64; i++)
            {
                if (freqBand64[i] > freqBandBuffer64[i])
                {
                    freqBandBuffer64[i] = freqBand64[i];
                    bufferDecrease64[i] = bufferValue1;
                }
                if (freqBand64[i] < freqBandBuffer64[i])
                {
                    freqBandBuffer64[i] -= bufferDecrease64[i];
                    bufferDecrease64[i] *= bufferValue2;
                }
            }
        }
        private void GetSpecturmAudioSource()
        {
            // What the fuck is an fftwindow?
            source.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
            source.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
        }
        private void MakeFrequencyBands()
        {
            /*
             * 22050 / 512 = 43hertz per sample
             * 
             * 20 - 60 hertz
             * 60 - 250 hertz
             * 500 - 2000 hertz
             * 2000 - 4000 hertz
             * 4000 - 6000 hertz
             * 6000 - 20000
             * 
             * 0 - 2 = 86 hertz
             * 1 - 4 = 172 hertz - 87-258
             * 2 - 8 = 244 hertz - 259-602
             * 3 - 16 = 688 hertz - 603-1290
             * 4 - 32 = 1376 hertz - 1291-2666
             * 5 - 64 = 5504 hertz to - 5418
             * 6 - 128 = 5504 hertz - 5419 - 10922
             * 7 - 256 = 11008 hertz - 10923-21930
             */

            int count = 0;

            for (int i = 0; i < 8; i++)
            {
                float average = 0;
                int sampleCount = (int)Mathf.Pow(2, i) * 2;

                if (i == 7)
                {
                    sampleCount += 2;
                }
                for (int j = 0; j < sampleCount; j++)
                {
                    if (channel == Channel.Stereo)
                        average += (samplesLeft[count] + samplesRight[count]) * (count + 1);
                    else if (channel == Channel.Left)
                        average += samplesLeft[count] * (count + 1);
                    else if (channel == Channel.Right)
                        average += samplesRight[count] * (count + 1);
                    average += (samplesLeft[count] + samplesRight[count]) * (count + 1);
                    count++;
                }

                average /= count;
                freqBand[i] = average * 10;
            }
        }
        private void MakeFrequencyBands64()
        {
            // TODO
            int count = 0;
            int sampleCount = 1;
            int power = 0;

            for (int i = 0; i < 64; i++)
            {
                float average = 0;

                if (i == 16 || i == 32 || i == 40 || i == 48 || i == 56)
                {
                    power++;
                    sampleCount = (int)Mathf.Pow(2, power);

                    if (power == 3)
                        sampleCount -= 2;
                }

                // Mono / Stereo
                for (int j = 0; j < sampleCount; j++)
                {
                    if (channel == Channel.Stereo)
                        average += (samplesLeft[count] + samplesRight[count]) * (count + 1);
                    else if (channel == Channel.Left)
                        average += samplesLeft[count] * (count + 1);
                    else if (channel == Channel.Right)
                        average += samplesRight[count] * (count + 1);
                    average += (samplesLeft[count] + samplesRight[count]) * (count + 1);
                    count++;
                }

                average /= count;
                freqBand64[i] = average * 80;
            }
        }
    }
}
