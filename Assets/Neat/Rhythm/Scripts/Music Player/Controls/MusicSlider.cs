using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Neat.Music
{
    // instead: audio slider
    [RequireComponent(typeof(Slider))]
    public class MusicSlider : MonoBehaviour, IMusicPlayerControl
    // IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        // references
        public MusicPlayer player;
        public Slider slider;
        public TextMeshProUGUI tmp;
        private bool wasPlaying;

        public void Enable(bool value)
        {
            slider.interactable = value;
            slider.maxValue = player.length;
        }

        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(UpdateText);
            slider.interactable = false;

            player.onEnable.AddListener(Enable);
            player.onTick.AddListener(UpdateTime);
            player.onSkip.AddListener(UpdateTime);
        }

        public void UpdateTime(float value)
        {
            value = Mathf.Clamp(value, 0f, player.length);
            slider.value = value;

            UpdateText(value);
        }

        private void UpdateText(float value)
        {
            value = Mathf.Clamp(value, 0f, player.length);

            // idk how this works lol MM:ss.mmm
            // https://stackoverflow.com/questions/40867158/how-can-i-format-a-float-number-so-that-it-looks-like-real-time
            tmp.text = $"{(int)value / 60}:{value % 60:00.000}";
        }

        // using EventTrigger in scene to access these
        // interface solution wasn't calling the methods
        public void OnPointerUp()
        {
            player.SkipTo(slider.value);

            // restart playback?
            if (wasPlaying)
                player.Play();
        }
        public void OnPointerDown()
        {
            print("pointer down");

            // pause if playing
            wasPlaying = player.isPlaying;
            if (wasPlaying)
                player.Pause();
        }
        public void OnDrag()
        {
            player.SkipTo(slider.value);
        }
    }
}