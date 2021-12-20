using Neat.Tools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI controller class
namespace Neat.Demos.AimTrainer
{
    public class PlayMenuUI : MonoBehaviour
    {
        // editor references
        public PresetScrollerUI presetScroller;
        public ScoreScrollerUI scoreScroller;
        public PresetProfileUI profileUI;

        public UISwitcher switcher;

        public GameObject warningPrefab;
        public Transform settingsContainer;

        public GameObject tooltipPrefab;

        public Button saveButton;
        public Button revertButton;

        // data & properties
        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                _hasChanges = value;

                saveButton.interactable = value;
                revertButton.interactable = value;
            }
        }
        private PresetCollection presets => PresetCollection.loaded;
        private bool _hasChanges;

        private ContextPrompt cpOverwriteSettings;
        private ContextPrompt cpDeleteScore;
        private ContextPrompt cpDeletePreset;
        private ContextPrompt cpCantDelete;
        private ContextPrompt cpPlay;
        private ContextPrompt cpReturn;
        private ContextPrompt cpRevert;

        private void Start()
        {
            switcher.SwitchTo(0);
            PauseListener.isListeningForKey = false;
            PauseListener.isPaused = true;
            PresetCollection.Load();

            // prompts handle delete actions
            cpDeleteScore = CreateDeleteScorePrompt();
            cpDeletePreset = CreateDeletePresetPrompt();
            cpCantDelete = CreateCantDeletePrompt();
            cpOverwriteSettings = CreateOverwritePrompt();
            cpPlay = CreatePlayPrompt();
            cpReturn = CreateReturnPrompt();
            cpRevert = CreateRevertPrompt();

            presetScroller.SetClickPrompt(CreatePresetSwitchPrompt(), LoadCurrentProfile);


            SetupUI();
            presetScroller.LoadCollection();
            SelectCurrentProfile();
        }

        // initialization
        private void SetupUI()
        {
            profileUI.CreateWarnings(warningPrefab, settingsContainer.transform);
            profileUI.SetUIValidation(delegate { HasChanges = true; });
            profileUI.AddAllTooltips(settingsContainer.transform, tooltipPrefab);
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
            HasChanges = false;
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
        private ContextPrompt CreateDeletePresetPrompt()
        {
            var prompt = new ContextPrompt();

            prompt.infoText = "Delete this preset? This action cannot be undone.";
            prompt.yesText = "Yes, delete preset";
            prompt.noText = "No, take me back";

            prompt.onYes = DeleteSelectedPreset;

            return prompt;
        }
        private ContextPrompt CreateCantDeletePrompt()
        {
            var prompt = new ContextPrompt();

            prompt.infoText = "Cannot delete last remaining preset.";
            prompt.yesText = "OK";

            return prompt;
        }
        private ContextPrompt CreateOverwritePrompt()
        {
            var prompt = new ContextPrompt();

            prompt.infoText = "Overwrite settings? This will clear all scores";
            prompt.yesText = "Yes, overwrite and remove scores";
            prompt.noText = "No, take me back";
            prompt.shouldShow = delegate
            {
                return HasChanges && PresetProfile.current.scores.Count > 0;
            };
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
            PresetProfile.current.scores = new List<ScoreProfile>();
                presets.Save();
                presetScroller.LoadCollection();

                SelectCurrentProfile();
            };
            prompt.shouldShow = delegate
            {
                return HasChanges && PresetProfile.current.scores.Count > 0;
            };

            return prompt;
        }
        private ContextPrompt CreateDeleteScorePrompt()
        {
            var prompt = new ContextPrompt();

            prompt.infoText = "Delete this score? This action cannot be undone.";
            prompt.yesText = "Yes, delete score";
            prompt.noText = "No, take me back";

            prompt.onYes = DeleteScore;

            return prompt;
        }
        private ContextPrompt CreatePresetSwitchPrompt()
        {
            var prompt = new ContextPrompt();

            prompt.infoText = "You have unsaved changed. Discard changes and switch preset?";
            prompt.yesText = "Yes, discard and switch";
            prompt.noText = "No, take me back";
            prompt.shouldShow = delegate { return HasChanges; };

            return prompt;
        }
        private ContextPrompt CreatePlayPrompt()
        {
            var prompt = new ContextPrompt();

            prompt.infoText = "You have unsaved changes. Would you like to save "
                + "changes and continue to play?";

            prompt.yesText = "Yes, save changes and play";
            prompt.noText = "No, continue editing";
            prompt.shouldShow = delegate { return HasChanges; };

            prompt.onYes = delegate
            {
                PresetCollection.loaded.Save(); // preserves last loaded
                SceneSwitcher2.instance.SwitchTo(2);
            };

            return prompt;
        }
        private ContextPrompt CreateReturnPrompt()
        {
            ContextPrompt c = new ContextPrompt();

            c.infoText = "You have unsaved changes. Discard and return to main?";
            c.yesText = "Yes, discard and return";
            c.noText = "No, stay here";
            c.shouldShow = delegate { return HasChanges; };

            c.onYes = delegate
            {
                SceneSwitcher2.instance.SwitchTo(0);
            };

            return c;
        }
        private ContextPrompt CreateRevertPrompt()
        {
            ContextPrompt c = new ContextPrompt();
            c.infoText = "Discard changes? This cannot be undone.";
            c.yesText = "Yes, revert to last saved state";
            c.noText = "No, continue editing";
            c.shouldShow = delegate { return HasChanges; };
            c.onYes = LoadCurrentProfile;

            return c;
        }

        // button events
        public void SaveAndApply()
        {
            cpOverwriteSettings.Process();
        }
        public void CreatePreset()
        {
            PresetProfile.current = new PresetProfile();
            LoadCurrentProfile();
            HasChanges = true;
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

            HasChanges = true;
        }
        public void Play()
        {
            cpPlay.Process();
        }
        public void ReturnToMain()
        {
            cpReturn.Process();
        }
        public void DeletePresetPressed()
        {
            // prevent deleting the last preset
            if (PresetCollection.loaded.items.Count >= 2)
                cpDeletePreset.Process();
            else
                cpCantDelete.Process();
        }
        public void DeleteScorePressed()
        {
            cpDeleteScore.Process();
        }
        public void RevertChanges()
        {
            cpRevert.Process();
        }
    }
}