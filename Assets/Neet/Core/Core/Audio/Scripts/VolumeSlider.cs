using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Neat.Audio
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        public int targetMixerGroup;
        private Slider slider;

        private void Start()
        {
            

            slider = GetComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = AudioManager.instance.GetMixerVolumeLinear(GetMixer());
            slider.onValueChanged.AddListener(UpdateVolume);
        }
        public void UpdateVolume(float f)
        {
            AudioManager.instance.SetMixerVolumeLinear(GetMixer(), f);
        }

        // this logic should probably be in AudioManager.cs
        private AudioMixerGroup GetMixer()
        {
            if (targetMixerGroup == 0)
                return AudioManager.instance.masterGroup;
            else if (targetMixerGroup == 1)
                return AudioManager.instance.musicGroup;
            else if (targetMixerGroup == 2)
                return AudioManager.instance.sfxGroup;

            Debug.LogWarning("Invalid mixer group. Selecting master by default");
            return AudioManager.instance.masterGroup;
        }
    }
}