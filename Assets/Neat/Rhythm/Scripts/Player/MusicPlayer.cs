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
using Neat.States;

namespace Neat.Music
{
    using Input = UnityEngine.Input;
    using FileBrowser = Neat.FileBrowser.FileBrowser;

    // is this more of a music timer?
    public class MusicPlayer : MonoBehaviour
    {
        // public const string defaultPath = "D:/Music";

        public event Action<AudioClip> onClipReady;
        public event Action onClipFinished;

        public UnityEvent<bool> onPlayAttempt = new UnityEvent<bool>();
        public UnityEvent<bool> onPause = new UnityEvent<bool>();


        public MusicSlider timeSlider;

        // inspector, references
        public string defaultPath;
        string[] filters = new string[] { ".mp3", ".wav" };

        private bool isPaused = true;
        private bool wasPlaying; // for slider drag
        public bool isPlaying
        {
            get
            {
                return source.isPlaying;
            }
        }

        private AudioClock _clock;
        public AudioClock clock
        {
            get
            {
                if (_clock == null)
                    _clock = new AudioClock(source, this);
                return _clock;
            }
        }
        public ClockState NewClock()
        {
            // so events don't get added more than once
            _clock = new AudioClock(source, this);
            return _clock;
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
                source.time = value;
            }
        }
        public bool isPlayable
        {
            get
            {
                return clock.Playable();
            }
        }

        public MediaInput controls;

        private void Start()
        {
            // initialize ui
            //if (timeSlider != null)
            //{
            //    onSourceTick += timeSlider.UpdateTime;
            //    timeSlider.interactable = false;
            //    timeSlider.onDragStart.AddListener(OnDragStart);
            //    timeSlider.onDragEnd.AddListener(OnDragEnd);
            //}
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
                Debug.LogError("Could not load: " + path + "\n" + e.Message);
            }
        }
        private void SongLoaded()
        {
            onClipReady.Invoke(source.clip);
            // successful load
            //if (timeSlider != null)
            //{
            //    timeSlider.interactable = true;
            //    timeSlider.range = source.clip.length;
            //}

        }

        // ui handling - this shouldn't be here
        //public void OnDragStart(float value)
        //{
        //    // isDragging = true;
        //    wasPlaying = source.isPlaying;
        //    Pause();
        //}
        //public void OnDragEnd(float value)
        //{
        //    SkipTo(value);
        //    if (wasPlaying)
        //        Play();
        //    wasPlaying = false;
        //}

        public void PauseToggle()
        {
            if (isPaused)
                Play();
            else
                Pause();
        }
        public void Pause()
        {
            clock.Pause();
        }
        public void Play()
        {
            clock.Start();
        }

        public void Skip(float delta)
        {
            clock.SetTime(clock.GetTime() + delta);
        }
        public void SkipTo(float time)
        {
            clock.SetTime(time);
        }
    }
}