using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Neat.Audio
{
    public class MicManager : MonoBehaviour
    {
        public AudioMixerGroup microphoneMixer;
        public bool printMicrophonesOnStart;

        private void Start()
        {
            if (printMicrophonesOnStart)
            {
                foreach (string mic in Microphone.devices)
                {
                    AudioSource source = gameObject.AddComponent<AudioSource>();
                    source.clip = Microphone.Start(mic, true, 10, AudioSettings.outputSampleRate);
                    source.outputAudioMixerGroup = microphoneMixer;
                    source.Play();
                }
            }
        }
    }
}

