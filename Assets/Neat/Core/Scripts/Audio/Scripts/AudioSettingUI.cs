using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Neat.Audio;

public class AudioSettingUI : MonoBehaviour, ISettingUI<AudioSetting>
{
    // public Toggle masterVolumeToggle;
    public Slider masterVolumeSlider;
    public TextMeshProUGUI masterVolumeLabel;

    public void AddAllTooltips(Transform container, GameObject prefab)
    {
        throw new System.NotImplementedException();
    }

    public void AddTooltip(Transform obj, Transform container, string text, GameObject prefab)
    {
        throw new System.NotImplementedException();
    }

    public void Apply(ref AudioSetting profile)
    {
        profile.masterVolume = masterVolumeSlider.value;

        AudioManager.instance.LoadSetting(profile);
        profile.SaveJSON();
    }

    public void CreateWarnings(GameObject warningPrefab, Transform container)
    {
    }

    public void LoadFields(AudioSetting profile)
    {
        masterVolumeSlider.value = profile.masterVolume;
    }

    public void SetContextTexts()
    {
        throw new System.NotImplementedException();
    }

    public void SetUIValidation(UnityAction endAction = null)
    {
        // masterVolumeToggle.onValueChanged.AddListener(delegate { });

        masterVolumeSlider.onValueChanged.AddListener(delegate(float value)
        {
            UpdateVolumeGUI(value, masterVolumeLabel);
        });
    }

    private void UpdateVolumeGUI(float rawValue, TextMeshProUGUI text)
    {
        text.text = (rawValue * 100).ToString("f0");
    }
}
