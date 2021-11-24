using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Neat.Audio
{
    [Serializable]
    public class Sound
    {
        public AudioClip clip;
        public string name;

        [Range(0f, 1f)]
        public float volume;

        [Range(-3f, 3f)]
        public float pitch;

        public bool loop;

        [HideInInspector]
        public bool isLooping;

        [HideInInspector]
        public AudioSource source;

        public Sound()
        {
            pitch = 1f;
            volume = 1f;

        }

        // [Button("Preview"), ShowIf("@!this.isLooping")]
        public void Play()
        {
            // get to outer class?
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = loop;
            source.Play();

            if (loop)
                isLooping = true;
        }

        // [Button("Stop"), ShowIf("@this.isLooping")]
        public void Stop()
        {
            source.Stop();
            isLooping = false;
        }
    }

    public class SoundBank : MonoBehaviour
    {
        // [Required]
        public AudioSource source;

        public Sound[] sounds;

        // [OnInspectorGUI]
        public void SetSoundSources()
        {
            if (sounds != null)
            {
                for (int i = 0; i < sounds.Length; i++)
                {
                    sounds[i].source = source;
                }
            }
        }

        // Methods
        public void Play(int index)
        {
            try
            {
                sounds[index].Play();
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to play sound with index "
                    + index + "\n" + e.Message);
            }
        }
        public void Play(string name)
        {
            try
            {
                Sound sound = Array.Find(sounds, s => s.name == name);
                sound.Play();
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to play sound with name " + name
                    + "\n" + e.Message);
            }
        }
    }
}
