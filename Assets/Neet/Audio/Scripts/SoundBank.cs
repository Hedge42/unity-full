using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Audio;

namespace Neet.Audio
{
    [Serializable]
    public class Sound
    {
        public AudioClip clip;
        public string name;

        [Range(0f, 1f)]
        public float volume;

        [HideInInspector]
        public AudioSource source;

        [Button]
        private void Preview()
        {
            // get to outer class?
            source.clip = clip;
            source.volume = volume;
            source.Play();
        }
    }

    public class SoundBank : MonoBehaviour
    {

        [Required]
        public AudioSource source;

        [OnInspectorGUI("SetSoundSources")]
        public Sound[] sounds;

        private void SetSoundSources()
        {
            if (sounds != null)
            {
                foreach (Sound s in sounds)
                {
                    s.source = source;
                }
            }
        }


        // Methods
        public void Play(int index)
        {
            Sound sound = null;
            if (index >= 0 && index < sounds.Length)
            {
                sound = sounds[index];

                if (sound.source != null && sound.clip != null)
                {
                    sound.source.Play();
                }

                else
                    Debug.LogWarning("Clip or source null.");
            }
            else
                Debug.LogWarning("Index out of range.");
        }
        public void Play(string name)
        {

            Sound sound = Array.Find(sounds, s => s.name == name);
            if (sound != null)
            {
                if (sound.clip != null)
                {
                    source.clip = sound.clip;
                    source.volume = sound.volume;
                    sound.source.Play();
                }

                else
                    Debug.LogWarning("No clip on sound " + name);
            }
            else
                Debug.LogWarning("No sound found with name " + name);
        }
    }
}
