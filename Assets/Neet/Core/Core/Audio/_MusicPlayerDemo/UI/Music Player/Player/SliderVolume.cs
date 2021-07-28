using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

namespace Neet.Audio.MusicPlayer
{
    public class SliderVolume : MonoBehaviour
    {
        public TextMeshProUGUI value;
        private Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(UpdateVolume);

            UpdateVolume(0f);
        }

        private void UpdateVolume(float arg0)
        {
            AudioManager.instance.UpdateMusicVolume(slider.value);
            value.text = (slider.value * 100).ToString("f0") + "%";
        }
    }
}
