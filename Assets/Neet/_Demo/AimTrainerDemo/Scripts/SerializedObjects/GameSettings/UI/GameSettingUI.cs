using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameSettingUI : MonoBehaviour
{
    public GameObject warningPrefab;
    public Transform container;

    private ControlSetting control;
    private AudioSetting audio;

    private ControlSettingUI controlUI;
    private AudioSettingUI audioUI;

    private bool hasChanges = false;

    private void Start()
    {
        controlUI = GetComponent<ControlSettingUI>();
        audioUI = GetComponent<AudioSettingUI>();
        control = ControlSetting.Load();
        audio = AudioSetting.Load();

        controlUI.CreateWarnings(warningPrefab, container);
        audioUI.CreateWarnings(warningPrefab, container);

        UnityAction changes = delegate
        {
            hasChanges = true;
        };

        controlUI.SetUIValidation(changes);
        audioUI.SetUIValidation(changes);

        LoadFields();
    }

    public void Apply()
    {
        controlUI.Apply(ref control);
        control.SaveBinary();

        audioUI.Apply(ref audio);
        audio.SaveBinary();
    }
    public void LoadFields()
    {
        controlUI.LoadFields(control);
        audioUI.LoadFields(audio);
    }
}