using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.Audio;
using Neet.File;

public class SettingsManager : MonoBehaviour
{
    [System.Serializable]
    public class Setting
    {
        // with default values
        public int FOV = 103;
        public bool fullScreen = true;
        public float cmPer360 = 14.61039f;
        public Resolution[] allRes => Screen.resolutions;
        public Resolution currentRes;
        public int frameLimit = 144;
        public bool showFPS = true;

        public float masterVolume = .5f;
        public float SFXVolume = .5f;
        public float musicVolume = .5f;
    }

    public static SettingsManager instance;
    public static string fileName { get { return "settings.json"; } }
    public static string path { get { return FileManager.SettingsPath + fileName; } }

    public readonly int[] allFps = { 30, 60, 90, 120, 144, 240, 400 };
    public Setting setting;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }
    private void Start()
    {
        LoadSettings();
    }


    public void SaveSettings()
    {
        string path = FileManager.instance.SerializeJson(setting, FileManager.SettingsPath, fileName);
        Debug.Log("Saved current setting to " + path);

    }
    public void LoadSettings()
    {
        setting = FileManager.instance.DeserializeJson<Setting>(path);
        if (setting == null)
            setting = new Setting();
    }
}