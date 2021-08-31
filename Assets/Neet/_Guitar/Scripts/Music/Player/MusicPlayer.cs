using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neat.FileManagement;
using Neat.FileBrowser;
using Neat.Audio;
using System;
using System.IO;
using System.Linq;
using UnityEngine.EventSystems;
using Neat.UI;
using UnityEngine.Events;

namespace Neat.Music
{
    using Input = UnityEngine.Input;
    using FileBrowser = Neat.FileBrowser.FileBrowser;
    public class MusicPlayer : MonoBehaviour
    {
        // public const string defaultPath = "D:/Music";

        public event Action<AudioClip> onClipLoaded;
        public event Action onPlay;
        public event Action onClipFinished;
        public event Action<float> onSourceTick;
        public event Action<float> onSkip;


        public MusicSlider timeSlider;

        // inspector, references
        public string defaultPath;
        string[] filters = new string[] { ".mp3", ".wav" };

        private bool isPaused = true;
        private bool wasPlaying; // for slider drag

        private bool _isPlaying;
        public bool isPlaying
        {
            get
            {
                if (hasClip)
                    _isPlaying = source.isPlaying;
                return _isPlaying;
            }
        }
        public AudioSource source
        {
            get
            {
                return AudioManager.instance.musicSource;
            }
        }
        public AudioClip clip
        {
            get
            {
                return source.clip;
            }
        }

        public float time
        {
            get
            {
                return source.time;
            }
            set
            {
                // force time=0 when no clip is loaded
                if (hasClip)
                    source.time = Mathf.Clamp(value, 0f, source.clip.length);
                else
                    source.time = 0f;
            }
        }

        public bool hasClip
        {
            get
            {
                return source != null && source.clip != null;
            }
        }

        private MediaControls _controls;
        public MediaControls controls
        {
            get
            {
                if (_controls == null)
                    _controls = GetComponent<MediaControls>();
                return _controls;
            }
        }

        private void Start()
        {
            // initialize ui
            if (timeSlider != null)
            {
                onSourceTick += timeSlider.UpdateTime;
                timeSlider.interactable = false;
                timeSlider.onDragStart.AddListener(OnDragStart);
                timeSlider.onDragEnd.AddListener(OnDragEnd);
            }

            if (controls != null)
            {
                controls.SetTarget(PauseToggle, Skip, SkipTo);
            }
        }

        // class for this?
        public void LoadFile()
        {
            FileBrowser.instance.Show(defaultPath, OnFileSelect, filters);

        }
        private void OnFileSelect(string path)
        {
            try
            {
                AudioManager.LoadClip(path, source, SongLoaded);
            }
            catch (Exception e)
            {
                Debug.LogError("Not a valid audio file: " + path + "\n" + e.Message);
            }
        }
        private void SongLoaded()
        {
            // successful load
            if (timeSlider != null)
            {
                timeSlider.interactable = true;
                timeSlider.range = source.clip.length;
            }

            onClipLoaded.Invoke(source.clip);
        }

        // ui handling
        public void OnDragStart(float value)
        {
            // isDragging = true;
            wasPlaying = source.isPlaying;
            Pause(true);
        }
        public void OnDragEnd(float value)
        {
            SkipTo(value);
            if (wasPlaying)
                Pause(false);
            wasPlaying = false;
        }

        // media functions
        private IEnumerator _Play()
        {
            source.Play();
            onPlay?.Invoke();

            while (!isPaused)
            {
                if (source.isPlaying)
                {
                    onSourceTick?.Invoke(time);
                }
                else
                {
                    Pause(true);
                    onClipFinished?.Invoke();
                }

                yield return new WaitForEndOfFrame();
            }
        }
        public void Pause(bool isPaused)
        {
            this.isPaused = isPaused;

            // stay paused if there is no clip
            if (hasClip)
            {
                if (isPaused)
                {
                    source.Pause();
                    StopAllCoroutines();
                }
                else
                    StartCoroutine(_Play());
            }
        }
        public void PauseToggle()
        {
            Pause(!isPaused);
        }
        public void Play()
        {
            Pause(false);
        }

        public void Skip(float delta)
        {
            time += delta;
            onSkip?.Invoke(time);
        }
        public void SkipTo(float time)
        {
            this.time = time;
            onSkip?.Invoke(time);
        }
    }
}