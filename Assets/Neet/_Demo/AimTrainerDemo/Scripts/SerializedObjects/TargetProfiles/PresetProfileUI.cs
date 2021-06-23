using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

// delegates to other profileUIs
public class PresetProfileUI : MonoBehaviour, ISettingUI<PresetProfile>
{
    public TMP_InputField presetName;
    public Dropdown room;

    // other profile UIs - automatically obtained
    private AimProfileUI aimProfileUI;
    private ColorProfileUI colorProfileUI;
    private TimingProfileUI timingProfileUI;
    private ChallengeProfileUI challengeProfileUI;
    private TrackingProfileUI trackingProfileUI;
    private MovementProfileUI movementProfileUI;

    private GameObject nameWarning;

    private void Awake()
    {
        aimProfileUI = GetComponent<AimProfileUI>();
        colorProfileUI = GetComponent<ColorProfileUI>();
        timingProfileUI = GetComponent<TimingProfileUI>();
        challengeProfileUI = GetComponent<ChallengeProfileUI>();
        trackingProfileUI = GetComponent<TrackingProfileUI>();
        movementProfileUI = GetComponent<MovementProfileUI>();
    }

    public void Apply(ref PresetProfile profile)
    {
        profile.name = presetName.text;

        aimProfileUI.Apply(ref profile.aimProfile);
        timingProfileUI.Apply(ref profile.timingProfile);
        colorProfileUI.Apply(ref profile.colorProfile);
        challengeProfileUI.Apply(ref profile.challengeProfile);
        trackingProfileUI.Apply(ref profile.trackingProfile);
        movementProfileUI.Apply(ref profile.movementProfile);
    }

    public void CreateWarnings(GameObject warningPrefab, Transform container)
    {
        if (presetName != null)
            nameWarning = UIHelpers.CreateWarning(warningPrefab,
                presetName.gameObject, container, "Name must be unique");

        aimProfileUI.CreateWarnings(warningPrefab, container);
        timingProfileUI.CreateWarnings(warningPrefab, container);
        colorProfileUI.CreateWarnings(warningPrefab, container);
        challengeProfileUI.CreateWarnings(warningPrefab, container);
        movementProfileUI.CreateWarnings(warningPrefab, container);
        trackingProfileUI.CreateWarnings(warningPrefab, container);
    }

    public void LoadCurrentProfile()
    {
        // print("null profile: " + (PresetProfile.current == null).ToString());
        LoadFields(PresetProfile.current);
    }
    public void LoadFields(PresetProfile profile)
    {
        // print("null input: " + (presetName == null).ToString());
        // print("null name: " + (profile.name == null).ToString());

        // print("null profile: " + (PresetProfile.current == null).ToString());
        presetName.text = profile.name;

        aimProfileUI.LoadFields(profile.aimProfile);
        timingProfileUI.LoadFields(profile.timingProfile);
        colorProfileUI.LoadFields(profile.colorProfile);
        challengeProfileUI.LoadFields(profile.challengeProfile);
        trackingProfileUI.LoadFields(profile.trackingProfile);
        movementProfileUI.LoadFields(profile.movementProfile);
    }

    public void SetUIValidation(UnityAction endAction = null)
    {
        ValidateUniqueName(presetName, nameWarning, endAction);

        aimProfileUI.SetUIValidation(endAction);
        colorProfileUI.SetUIValidation(endAction);
        timingProfileUI.SetUIValidation(endAction);
        challengeProfileUI.SetUIValidation(endAction);
        movementProfileUI.SetUIValidation(endAction);
        trackingProfileUI.SetUIValidation(endAction);
    }

    // move to UI helpers?
    void ValidateUniqueName(TMP_InputField f, GameObject warning, 
        UnityAction endAction = null)
    {
        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/local-functions
        // reusable function for both change and end events
        // create a local function which accepts string and returns bool

        // warn if not unique
        UnityAction<string> change = delegate (string s)
        {
            warning.SetActive(!IsUnique(s, PresetCollection.SelectedIndex));
        };

        // set to unique name
        UnityAction<string> end = delegate (string s)
        {
            f.text = GetUniqueName(s, PresetCollection.SelectedIndex);

            endAction.Invoke();
        };

        f.onValueChanged.AddListener(change);

        f.onDeselect.AddListener(end);
        f.onEndEdit.AddListener(end);
        f.onSubmit.AddListener(end);
    }

    private bool IsUnique(string s, int filter)
    {
        bool unique = true;
        var list = PresetCollection.loaded.names;

        // for each name in the list
        for (int i = 0; i < list.Count; i++)
        {
            // don't check against this item
            if (i == filter)
                continue;

            // if this item's text matches the one in the list
            else if (s == list[i])
            {
                unique = false;
                break;
            }
        }

        return unique;
    }
    public string GetUniqueName(string name, int filter)
    {
        int count = 0;
        string newName = name;

        while (!IsUnique(newName, filter))
            newName = name + "(" + (++count).ToString() + ")";

        return newName;
    }
}
