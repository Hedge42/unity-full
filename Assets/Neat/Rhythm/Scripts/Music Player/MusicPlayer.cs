using Neat.Audio;
using Neat.States;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Neat.Audio.Music
{
    [ExecuteAlways]
    // is this more of a music timer?
    public class MusicPlayer : MonoBehaviour, MediaPlayer
    {
        // inspector
        public string musicDirectory; // default music directory

        // events - I feel like these should be in the ChartPlayer logic
        public UnityEvent<AudioClip> onClipReady = new UnityEvent<AudioClip>();
        public UnityEvent onClipFinished = new UnityEvent();
        public UnityEvent<bool> onPlayPause = new UnityEvent<bool>();
        public UnityEvent<bool> onEnable = new UnityEvent<bool>();
        public UnityEvent<float> onTick = new UnityEvent<float>();
        public UnityEvent<float> onSkip = new UnityEvent<float>();

        // playing properties
        private float _time;
        public float time
        {
            get
            {
                return _time;
            }
            set
            {
                source.time = _time = value;
            }
        }
        public bool isPlayable => source != null && source.clip != null;
        public bool isPlaying { get; private set; }

        // audio source properties
        public AudioSource source => AudioManager.instance.musicSource;
        public AudioClip clip => source.clip;
        public float length
        {
            get
            {
                float _length = 0;
                if (source.clip != null)
                    return source.clip.length;
                return _length;
            }
        }

        private Coroutine playRoutine;

        private void Awake()
        {
            // enabled h
            Enable(false);

            onClipReady.AddListener(delegate { Enable(true); });
        }

        public void Enable(bool value)
        {
            onEnable?.Invoke(value);
        }

        // should these be here?
        public void LoadClip(string path)
        {
            AudioLoader.Load(source, path, Ready);
        }
        public void FindClip(string directory)
        {
            AudioLoader.FindAndLoad(directory, source, Ready);
        }
        public void Ready()
        {
            onClipReady?.Invoke(source.clip);
            Enable(true);
        }

        
        // media functions
        public void SkipTo(float newTime)
        {
            _time = newTime;
            source.time = newTime;

            onSkip?.Invoke(newTime);
        }
        public void SkipForward()
        {
            SkipTo(time + 5f);
        }
        public void SkipBack()
        {
            SkipTo(time - 5f);
        }
        public void Pause()
        {
            source.Pause();
            isPlaying = false;

            // doesn't care about audio being played
            if (playRoutine != null)
                StopCoroutine(playRoutine);
            playRoutine = null;

            onPlayPause?.Invoke(false);
        }
        public void Play()
        {
            // use a stopwatch if audio cannot be played
            if (!isPlayable)
                playRoutine = StartCoroutine(_PlayNoMusic());
            else
                playRoutine = StartCoroutine(_Play());

            isPlaying = true;
            onPlayPause?.Invoke(true);
        }
        private IEnumerator _PlayNoMusic()
        {
            // until user interrupt
            while (isPlaying)
            {
                _time += Time.deltaTime;
                onTick?.Invoke(_time);

                yield return new WaitForEndOfFrame();
            }

            isPlaying = false;
        }
        private IEnumerator _Play()
        {
            source.Play();

            // until user interrupt or end of file
            while (source.isPlaying)
            {
                _time = source.time;
                onTick?.Invoke(_time);

                yield return new WaitForEndOfFrame();
            }

            // coroutine may be terminated before this point
            isPlaying = false;
            playRoutine = null;

            onClipFinished?.Invoke();
        }
    }
}