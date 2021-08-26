using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neet.File;
using Neat.FileBrowser;
using Neet.Audio;
using System;
using System.IO;
using System.Linq;
using UnityEngine.EventSystems;

public class MusicPlayer : MonoBehaviour
{
    public event Action<float> onTick;
    public event Action<AudioClip> onClipLoaded;

    // inspector, references
    public Button btnLoad;
    public Slider timeSlider;
    public Slider volumeSlider;
    public TMPro.TextMeshProUGUI tmpTime;
    public string defaultPath;

    // data
    private bool isPaused = true;
    private bool wasPlaying; // for slider drag
    private AudioSource source => AudioManager.instance.musicSource;
    string[] filters = new string[] { ".mp3", ".wav" };

    // mono
    private void Start()
    {
        // initialize ui
        btnLoad.onClick.AddListener(OpenFileBrowser);
        volumeSlider.value = AudioManager.instance.musicSource.volume;
        timeSlider.onValueChanged.AddListener(UpdateTimeText);
        UpdateTimeText(0f);
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

    // file select
    private void OpenFileBrowser()
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
        InitializeSlider();
    }

    // ui handling
    private void InitializeSlider()
    {
        timeSlider.minValue = 0f;
        timeSlider.value = 0f;

        if (source.clip != null)
        {
            timeSlider.maxValue = source.clip.length;
        }
        else
        {
            timeSlider.maxValue = 1f;
        }
    }
    public void SliderDragStart()
    {
        // isDragging = true;
        wasPlaying = source.isPlaying;
        Pause(true);
        SkipTo(timeSlider.value);
    }
    public void SliderDragEnd()
    {
        SkipTo(timeSlider.value);
        print(wasPlaying);
        if (wasPlaying)
            Pause(false);
        wasPlaying = false;
    }
    public void VolumeSliderDrag()
    {
        AudioManager.instance.UpdateMusicVolume(volumeSlider.value);
    }

    private void UpdateTimeText(float value)
    {
        if (source.clip != null)
        {
            int minutes = ((int)(value / 60));
            int seconds = ((int)(value - minutes));

            // idk how this works lol
            // https://stackoverflow.com/questions/40867158/how-can-i-format-a-float-number-so-that-it-looks-like-real-time
            tmpTime.text = $"{(int)value / 60}:{value % 60:00.000}";
            // tmpTime.text = value.ToString("f3");

        }
        else
            tmpTime.text = "--";
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
            timeSlider.value = source.time;
            onTick?.Invoke(source.time);
            yield return new WaitForEndOfFrame();
        }
    }
}
