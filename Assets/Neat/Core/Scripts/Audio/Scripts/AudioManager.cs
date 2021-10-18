using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using UnityEngine.Networking;
using System;

namespace Neat.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        public static AudioManager instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<AudioManager>();
                return _instance;
            }
        }
        // public static event Action<AudioClip> onClipLoaded;

        public AudioSource masterAudioSource;
        public AudioSource musicSource;

        public AudioMixer masterMixer;
        public AudioMixerGroup masterGroup;
        public AudioMixerGroup sfxGroup;
        public AudioMixerGroup musicGroup;
        public AudioMixerGroup microphoneGroup;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);

            masterAudioSource = GetComponent<AudioSource>();


            foreach (var a in masterMixer.FindMatchingGroups(""))
            {
                // print("found one");
            }
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
            masterMixer.SetFloat("myMasterVolume", LinearToDb(linear));
        }
        /// <summary>
        /// Accepts [0,1] range
        /// </summary>Z
        public void UpdateSfxVolume(float linear)
        {
            // https://www.youtube.com/watch?v=9tqi1aXlcpE
            masterMixer.SetFloat("mySfxVolume", LinearToDb(linear));
        }
        /// <summary>
        /// Accepts [0,1] range
        /// </summary>
        public void UpdateMusicVolume(float linear)
        {
            // https://www.youtube.com/watch?v=9tqi1aXlcpE
            masterMixer.SetFloat("myMusicVolume", LinearToDb(linear));
        }

        public float GetMixerVolumeLinear(AudioMixerGroup group)
        {
            string param = GetVolumeParameter(group);
            masterMixer.GetFloat(param, out float f);
            return DbToLinear(f);
        }
        public void SetMixerVolumeLinear(AudioMixerGroup group, float linear)
        {
            string param = GetVolumeParameter(group);
            masterMixer.SetFloat(param, LinearToDb(linear));
        }

        private string GetVolumeParameter(AudioMixerGroup group)
        {
            if (group == masterGroup)
                return "myMasterVolume";
            else if (group == musicGroup)
                return "myMusicVolume";
            else if (group == sfxGroup)
                return "mySfxVolume";

            Debug.LogError("Group parameter not set up");
            return "?";
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
        private IEnumerator _LoadClip(string path, Action<AudioClip> onLoad = null)
        {
            // Debug.Log("Loading clip at: " + path);

            AudioClip clip = null;
            float startTime = Time.time;

            var startDate = System.DateTime.Now;

            if (!File.Exists(path))
            {
                Debug.Log("AudioClip path \"" + path + "\" does not exist");
                yield break;
            }

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

            var endDate = System.DateTime.Now;
            var ms = endDate.Subtract(startDate).TotalMilliseconds;
            Debug.Log(clip.name + " loaded in " + ms.ToString() + " milliseconds.");

            //float millisecondsToLoad = (Time.time - startTime) * 1000;
            //Debug.Log(clip.name + " loaded in " + millisecondsToLoad + " milliseconds.");
            onLoad?.Invoke(clip);
        }
        public static void LoadClip(string path, Action<AudioClip> onLoad = null)
        {
            instance.StartCoroutine(instance._LoadClip(path, onLoad));
        }

        public static void SetClip(AudioSource source, AudioClip clip, Action onClipReady = null)
        {
            instance.StartCoroutine(instance._SetClip(source, clip, onClipReady));
        }
        private IEnumerator _SetClip(AudioSource source, AudioClip clip, Action onClipReady = null)
        {
            var s = new System.Diagnostics.Stopwatch();
            s.Start();
            source.clip = clip;

            while (source.clip.loadState != AudioDataLoadState.Loaded)
                yield return null;

            s.Stop();
            // UnityEngine.Debug.Log("Clip \"" + clip.name + "\" set in " + s.ElapsedMilliseconds + "ms");

            onClipReady?.Invoke();
        }
    }
}
