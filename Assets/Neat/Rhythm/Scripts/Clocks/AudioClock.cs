using UnityEngine;
using UnityEngine.Audio;
using Neat.Music;
using System.Collections;
using System;

namespace Neat.States
{
    public class AudioClock : ClockState
    {
        private AudioSource source;
        private MonoBehaviour mono;

        public event Action<float> onTick;
        public event Action onClipFinished;

        private Coroutine playRoutine;

        public bool isActive
        {
            get { return source.isPlaying; }
        }

        public AudioClock(AudioSource source, MonoBehaviour mono)
        {
            this.source = source;
            this.mono = mono;
        }
        public float GetTime()
        {
            return source.time;
        }
        public void SetTime(float t)
        {
            source.time = t;
        }

        public void Start()
        {
            if (Playable())
            {
                source.Play();
                ResetRoutine();
                playRoutine = mono.StartCoroutine(_Start());
            }
            else
            {
                Debug.Log("Clip wasn't ready to play...");
            }
        }

        public bool Playable()
        {
            return source != null && source.clip != null &&
                    source.clip.loadState == AudioDataLoadState.Loaded;
        }

        public IEnumerator _Start()
        {
            while (source.isPlaying)
            {
                onTick?.Invoke(source.time);
                yield return new WaitForEndOfFrame();
            }

            // coroutine would be stopped by a pause
            onClipFinished?.Invoke();
            playRoutine = null;
        }

        private void ResetRoutine()
        {
            if (playRoutine != null)
            {
                mono.StopCoroutine(playRoutine);
                playRoutine = null;
            }
        }

        public void Pause()
        {
            source.Pause();
            ResetRoutine();
        }
        public void Stop()
        {
            source.Stop();
            ResetRoutine();
            SetTime(0f);
        }
    }
}