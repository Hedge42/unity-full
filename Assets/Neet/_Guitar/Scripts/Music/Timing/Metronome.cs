using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Audio;

namespace Neat.Music
{
    [RequireComponent(typeof(SoundBank))]
    public class Metronome : MonoBehaviour
    {
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
            if (emphasized)
                sounds.Play(0);
            else
                sounds.Play(1);
        }
    }
}
