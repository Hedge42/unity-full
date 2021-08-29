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

namespace Neat.Music
{
    using Input = UnityEngine.Input;
    using FileBrowser = Neat.FileBrowser.FileBrowser;
    public class MusicPlayer : MonoBehaviour
    {
        // public const string defaultPath = "D:/Music";

        public event Action<float> onTick;
        public event Action<AudioClip> onClipLoaded;

        public MusicSlider timeSlider;

        // inspector, references
        public string defaultPath;

        // data
        private bool isPaused = true;
        private bool wasPlaying; // for slider drag

        private AudioSource source => AudioManager.instance.musicSource;
        string[] filters = new string[] { ".mp3", ".wav" };
        public float time => source.time;
        public bool hasClip => source?.clip != null;

        // mono
        private void Start()
        {
            // initialize ui
            if (timeSlider != null)
            {
                onTick += timeSlider.UpdateTime;
                timeSlider.interactable = false;
                timeSlider.onDragStart.AddListener(OnDragStart);
                timeSlider.onDragEnd.AddListener(OnDragEnd);
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                SkipTo(0f);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SkipTo(source.time + 5f);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SkipTo(source.time - 5f);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Pause(!isPaused);
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

        // successful load
        private void SongLoaded()
        {
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
        public void Pause(bool isPaused)
        {
            // stay paused if there is no clip
            if (source.clip != null)
            {
                this.isPaused = isPaused;
                if (isPaused)
                {
                    source.Pause();
                    StopAllCoroutines();
                }
                else
                    StartCoroutine(_Play());
            }
            else
                this.isPaused = true;
        }
        public void Play()
        {
            Pause(false);
        }
        public void SkipTo(float time)
        {
            if (source.clip != null)
            {
                time = Mathf.Clamp(time, 0, source.clip.length);
                source.time = time;
                onTick?.Invoke(source.time);
            }
        }
        private IEnumerator _Play()
        {
            source.Play();
            // don't break on chart missing for now
            while (!isPaused && source.time < source.clip.length)
            {
                onTick?.Invoke(source.time);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}