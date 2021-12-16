using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Neat.Audio
{
    [CreateAssetMenu(menuName = "")]
    public class SoundObject : ScriptableObject
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

        public static SoundObject CreateInstance()
        {
            return ScriptableObject.CreateInstance<SoundObject>();
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


        // this should go somewhere else...
        [RuntimeInitializeOnLoadMethod]
        public static void InitializeMe()
        {

        }
    }
}
