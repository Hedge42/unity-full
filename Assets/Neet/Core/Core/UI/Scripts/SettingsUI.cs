using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Neat.Audio;


public class SettingsUI : MonoBehaviour
{
    public TMP_InputField InchesPer360Input;
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Start()
    {
        SettingsManager.instance.LoadSettings();
        SetEvents();
        ReadSettings();
    }

    void SetEvents()
    {
        InchesPer360Input?.onValueChanged.AddListener(CMPer360Changed);
        masterVolumeSlider?.onValueChanged.AddListener(MasterVolumeChanged);
        sfxVolumeSlider?.onValueChanged.AddListener(SFXVolumeChanged);
    }
    void CMPer360Changed(string s)
    {
        if (float.TryParse(InchesPer360Input.text, out float cmPer360))
        {
            SettingsManager.instance.setting.cmPer360 = cmPer360;
        }
    }
    void MasterVolumeChanged(float f)
    {
        SettingsManager.instance.setting.masterVolume = f;

        AudioManager.instance.UpdateMasterVolume(f);
    }
    void SFXVolumeChanged(float f)
    {
        SettingsManager.instance.setting.SFXVolume = f;

        AudioManager.instance.UpdateSfxVolume(f);
    }

    internal void ReadSettings()
    {
        if (InchesPer360Input != null)
            InchesPer360Input.text = SettingsManager.instance.setting.cmPer360.ToString();
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = SettingsManager.instance.setting.masterVolume;
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = SettingsManager.instance.setting.SFXVolume;
    }

    public void SaveSettings()
    {
        SettingsManager.instance.SaveSettings();
    }
}
