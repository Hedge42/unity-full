using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameSettingUI : MonoBehaviour
{
    public GameObject warningPrefab;
    public Transform container;

    private ControlSetting controlSetting;
    private AudioSetting audioSetting;

    private ControlSettingUI controlUI;
    private AudioSettingUI audioUI;

    private bool hasChanges = false;

    private void Start()
    {
        controlUI = GetComponent<ControlSettingUI>();
        audioUI = GetComponent<AudioSettingUI>();
        controlSetting = ControlSetting.Load();
        audioSetting = AudioSetting.Load();

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
        if (hasChanges)
            print("Set me up");

        controlUI.Apply(ref controlSetting);
        controlSetting.SaveJSON();

        audioUI.Apply(ref audioSetting);
        audioSetting.SaveJSON();
    }
    public void LoadFields()
    {
        controlUI.LoadFields(controlSetting);
        audioUI.LoadFields(audioSetting);
    }
}