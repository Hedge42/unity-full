using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Neat.AimTrainer
{
    public class ScoreScrollerUI : MonoBehaviour
    {
        public TextMeshProUGUI scoreTitle;
        public RectTransform scoreContainer;
        public ScoreResultsUI resultsUI;
        public UISwitcher switcher;

        private List<Button> scoreButtons;
        private GameObject prefab;

        public int selectedIndex;

        private void Start()
        {
            scoreButtons = new List<Button>();

            // get existing "prefab" transforms and hide
            prefab = scoreContainer.GetChild(0).gameObject;
            prefab.SetActive(false);
        }

        public void LoadCurrentProfile()
        {
            LoadItems(PresetProfile.current);
        }
        public void LoadItems(PresetProfile preset)
        {
            ClearItems();
            scoreTitle.text = "Scores (" + preset.scores.Count.ToString() + ")";

            for (int i = 0; i < preset.scores.Count; i++)
            {
                Button b = Instantiate(prefab, scoreContainer).GetComponent<Button>();
                b.gameObject.SetActive(true);
                scoreButtons.Add(b);
                var tmp = b.GetComponentInChildren<TextMeshProUGUI>();
                tmp.text = preset.scores[i].datePlayed;

                b.onClick.AddListener(delegate
                {
                    // need to use button index because i keeps changing
                    int btnIndex = scoreButtons.IndexOf(b);
                    selectedIndex = btnIndex;
                    resultsUI.UpdateGUI(preset, preset.scores[btnIndex]);
                    ShowResultsMenu();
                    ToggleButtonInteractability(b);
                });
            }
        }

        public void Select(int index)
        {
            scoreButtons[index].onClick.Invoke();
        }

        private void ClearItems()
        {
            foreach (Button b in scoreButtons)
                Destroy(b.gameObject);

            scoreButtons = new List<Button>();
        }

        public void ShowResultsMenu()
        {
            switcher.SwitchTo(1);
        }
        public void ShowSettingsMenu()
        {
            // resets interactability
            ToggleButtonInteractability(null);
            switcher.SwitchTo(0);
        }

        private void ToggleButtonInteractability(Button b)
        {
            foreach (Button _b in scoreButtons)
            {
                _b.interactable = b != _b;
            }
        }
    }
}