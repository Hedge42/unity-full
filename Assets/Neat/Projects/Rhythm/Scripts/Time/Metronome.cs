using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neat.Audio;

namespace Neat.Audio.Music
{
    [RequireComponent(typeof(SoundBank))]
    public class Metronome : MonoBehaviour
    {
        public bool mute;

        private SoundBank _sounds;
        public SoundBank sounds
        {
            get
            {
                if (_sounds == null)
                    _sounds = GetComponent<SoundBank>();
                return _sounds;
            }
        }

        public void Play(bool emphasized)
        {
            if (mute) return;

            if (emphasized)
                sounds.Play(0);
            else
                sounds.Play(1);
        }
    }
}
