﻿using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using UnityEngine.Networking;
using System;

namespace Neet.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        public static event Action<AudioManager> onClipLoaded;

        public AudioSource masterAudioSource;
        public AudioSource musicSource;

        public AudioMixer master;
        public AudioMixerGroup masterGroup;
        public AudioMixerGroup sfxGroup;
        public AudioMixerGroup musicGroup;
        public AudioMixerGroup microphoneGroup;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            masterAudioSource = GetComponent<AudioSource>();
        }

        public void LoadSetting(AudioSetting setting)
        {
            UpdateMasterVolume(setting.masterVolume);
        }

        /// <summary>
        /// Accepts [0,1] range
        /// </summary>
        public void UpdateMasterVolume(float linear)
        {
            // https://www.youtube.com/watch?v=9tqi1aXlcpE
            master.SetFloat("myMasterVolume", LinearToDb(linear));
        }
        /// <summary>
        /// Accepts [0,1] range
        /// </summary>
        public void UpdateSfxVolume(float linear)
        {
            // https://www.youtube.com/watch?v=9tqi1aXlcpE
            master.SetFloat("mySfxVolume", LinearToDb(linear));
        }
        /// <summary>
        /// Accepts [0,1] range
        /// </summary>
        public void UpdateMusicVolume(float linear)
        {
            // https://www.youtube.com/watch?v=9tqi1aXlcpE
            master.SetFloat("myMusicVolume", LinearToDb(linear));
        }

        private float LinearToDb(float linear)
        {
            // https://answers.unity.com/questions/283192/how-to-convert-decibel-number-to-audio-source-volu.html
            float db;
            if (linear != 0)
                db = 20.0f * Mathf.Log10(linear);
            else
                db = -144f;
            return db;
        }
        private float DbToLinear(float db)
        {
            // https://answers.unity.com/questions/283192/how-to-convert-decibel-number-to-audio-source-volu.html
            float linear = Mathf.Pow(10.0f, db / 20.0f);
            return linear;
        }

        // https://forum.unity.com/threads/www-is-obsolete-use-unitywebrequest-how-to-get-a-byte-array.556564/
        /// <summary> Accepts the path to an audio file, converts it into an audioClip, and attaches it to the source. </summary>
        public IEnumerator GetClip(string path, AudioSource source, Action onLoad = null)
        {
            AudioClip clip = null;
            float startTime = Time.time;

            // Handle supported extensions
            string _ext = Path.GetExtension(path).ToLower();
            UnityWebRequest _www;
            if (_ext == ".mp3")
                _www = UnityWebRequestMultimedia.GetAudioClip("file:///" + path, AudioType.MPEG);
            else if (_ext == ".wav")
                _www = UnityWebRequestMultimedia.GetAudioClip("file:///" + path, AudioType.WAV);
            else
            {
                Debug.Log("File type " + _ext + " not supported.");
                yield break;
            }

            yield return _www.SendWebRequest();

            if (_www.isNetworkError)
            {
                Debug.Log("Oops, something went wrong :/");
                yield break;
            }
            else
            {
                if (_ext == ".mp3")
                    clip = NAudioPlayer.FromMp3Data(_www.downloadHandler.data);
                else
                    clip = DownloadHandlerAudioClip.GetContent(_www);
            }


            clip.name = Path.GetFileName(path);
            source.clip = clip;

            float millisecondsToLoad = (Time.time - startTime) * 1000;
            Debug.Log(clip.name + " loaded in " + millisecondsToLoad + " milliseconds.");

            if (onClipLoaded != null)
                onClipLoaded(instance);

            if (onLoad != null)
                onLoad.Invoke();
        }
    }
}
