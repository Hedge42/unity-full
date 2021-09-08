using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Neat.Audio.MusicPlayer
{
    public class SliderTimeline : MonoBehaviour
    {
        public TextMeshProUGUI value;

        private Slider slider;
        private AudioSource source;

        private bool isDragging = false;
        private bool wasPlaying = false;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void Start()
        {
            source = AudioManager.instance.musicSource;
            AudioManager.onClipLoaded += AudioManager_onSongLoaded;
            slider.onValueChanged.AddListener(UpdateText);
        }

        private void Update()
        {
            if (!isDragging)
                slider.value = source.time;
            else
                source.time = slider.value;
        }

        private void AudioManager_onSongLoaded(AudioManager am)
        {
            if (source?.clip != null)
                slider.maxValue = source.clip.length;
        }

        public void ClickDown()
        {
            if (source.isPlaying)
            {
                source.Pause();
                wasPlaying = true;
            }

            isDragging = true;
        }
        public void ClickUp()
        {
            if (isDragging && wasPlaying)
                source.UnPause();

            wasPlaying = false;
            isDragging = false;
        }

        private void UpdateText(float ignoreMe)
        {
            value.text = slider.value.ToString("f0");
        }
    }
}
