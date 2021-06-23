using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using Neet.UI;

// UI controller class
public class PlayMenuUI : MonoBehaviour
{
    // editor references
    public PresetScrollerUI presetScroller;
    public ScoreScrollerUI scoreScroller;
    public PresetProfileUI profileUI;

    public AreYouSure areYouSure;
    public GameObject warningPrefab;
    public Transform settingsContainer;

    // data & properties
    public bool HasChanges { get { return hasChanges; } }
    private PresetCollection presets => PresetCollection.loaded;
    private bool hasChanges;

    private ConfirmationPrompt overwriteSettingsPrompt;
    private ConfirmationPrompt deleteScorePrompt;
    private ConfirmationPrompt deletePresetPrompt;
    private ConfirmationPrompt cantDeletePrompt;

    private void Start()
    {
        PauseListener.isListeningForKey = false;
        PauseListener.isPaused = true;
        PresetCollection.Load();

        // prompts handle delete actions
        deleteScorePrompt = CreateDeleteScorePrompt();
        deletePresetPrompt = CreateDeletePresetPrompt();
        cantDeletePrompt = CreateCantDeletePrompt();
        overwriteSettingsPrompt = CreateOverwritePrompt();
        presetScroller.SetClickPrompt(CreatePresetSwitchPrompt(), LoadCurrentProfile);

        SetupUI();
        presetScroller.LoadCollection();
        SelectCurrentProfile();
    }

    // initialization
    private void SetupUI()
    {
        profileUI.CreateWarnings(warningPrefab, settingsContainer.transform);
        profileUI.SetUIValidation(delegate { hasChanges = true; });
    }
    private void SelectCurrentProfile()
    {
        // calls LoadCurrentProfile through the presetScroller
        // to interact with buttons
        presetScroller.Select(PresetCollection.SelectedIndex);
    }
    private void LoadCurrentProfile()
    {
        // loads data without interacting with preset scroller
        profileUI.LoadCurrentProfile();
        scoreScroller.LoadCurrentProfile();
        scoreScroller.ShowSettingsMenu();
        hasChanges = false;
    }

    // deleting
    private void DeleteScore()
    {
        // might need testing
        PresetProfile.current.scores.RemoveAt(scoreScroller.selectedIndex);

        scoreScroller.LoadCurrentProfile();
        if (PresetProfile.current.scores.Count == 0)
            scoreScroller.ShowSettingsMenu();

        PresetCollection.loaded.Save();
    }
    private void DeleteSelectedPreset()
    {
        int presetIndex = PresetCollection.SelectedIndex;
        PresetCollection.loaded.RemovePreset(presetIndex);
        PresetCollection.loaded.Save();
        presetScroller.LoadCollection();
        SelectCurrentProfile();
    }

    // confirmation prompts
    private ConfirmationPrompt CreateDeletePresetPrompt()
    {
        var prompt = new ConfirmationPrompt(areYouSure);

        prompt.infoText = "Delete this preset? This action cannot be undone.";
        prompt.yesText = "Yes, delete preset";
        prompt.noText = "No, take me back";

        prompt.onYes = DeleteSelectedPreset;

        return prompt;
    }
    private ConfirmationPrompt CreateCantDeletePrompt()
    {
        var prompt = new ConfirmationPrompt(areYouSure);

        prompt.infoText = "Cannot delete last remaining preset.";
        prompt.yesText = "OK";

        return prompt;
    }
    private ConfirmationPrompt CreateOverwritePrompt()
    {
        var prompt = new ConfirmationPrompt(areYouSure);

        prompt.infoText = "Overwrite settings? This will clear all scores";
        prompt.yesText = "Yes, overwrite and remove scores";
        prompt.noText = "No, take me back";
        prompt.onYes = delegate
        {
            profileUI.Apply(ref PresetProfile.current);

            // add new preset to collection
            if (PresetCollection.SelectedIndex == -1)
            {
                presets.items.Add(PresetProfile.current);

                // set selected item as this new profile
                PresetCollection.SelectedIndex = presets.items.Count - 1;
            }

            // reload the preset scroller
            presets.Save();
            presetScroller.LoadCollection();

            SelectCurrentProfile();
        };
        prompt.shouldShow = delegate
        {
            return hasChanges && PresetProfile.current.scores.Count > 0;
        };

        return prompt;
    }
    private ConfirmationPrompt CreateDeleteScorePrompt()
    {
        var prompt = new ConfirmationPrompt(areYouSure);

        prompt.infoText = "Delete this score? This action cannot be undone.";
        prompt.yesText = "Yes, delete score";
        prompt.noText = "No, take me back";

        prompt.onYes = DeleteScore;

        return prompt;
    }
    private ConfirmationPrompt CreatePresetSwitchPrompt()
    {
        var prompt = new ConfirmationPrompt(areYouSure);

        prompt.infoText = "You have unsaved changed. Discard changes and switch preset?";
        prompt.yesText = "Yes, discard and switch";
        prompt.noText = "No, take me back";
        prompt.shouldShow = delegate { return HasChanges; };

        return prompt;
    }

    

    // button events
    public void SaveAndApply()
    {
        overwriteSettingsPrompt.Ask();
    }
    public void CreatePreset()
    {
        PresetProfile.current = new PresetProfile();
        LoadCurrentProfile();
        hasChanges = true;
    }
    public void DuplicatePreset()
    {
        // Clone the current profile, delete scores, save the index to return to
        PresetProfile.current = PresetProfile.current.Clone();
        string name = PresetProfile.current.name;
        PresetProfile.current.name = profileUI.GetUniqueName(name, -1);
        PresetProfile.current.scores.Clear();

        // load the new profile
        PresetCollection.SelectedIndex = -1;
        LoadCurrentProfile();

        // TODO create a button

        hasChanges = true;
    }
    public void Play()
    {
        PresetCollection.loaded.Save(); // to preserve last loaded index
        SceneSwitcher2.instance.SwitchTo(2);
    }
    public void ReturnToMain()
    {
        SceneSwitcher2.instance.SwitchTo(0);
    }
    public void DeletePresetPressed()
    {
        // prevent deleting the last preset
        if (PresetCollection.loaded.items.Count >= 2)
            deletePresetPrompt.Ask();
        else
            cantDeletePrompt.Inform();
    }
    public void DeleteScorePressed()
    {
        deleteScorePrompt.Ask();
    }
    public void RevertChanges()
    {
        Debug.Log("This has not been set up yet");
    }
}
