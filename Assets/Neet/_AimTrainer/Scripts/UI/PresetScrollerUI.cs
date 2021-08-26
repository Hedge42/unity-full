using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;
using Neat.UI;

public class PresetScrollerUI : MonoBehaviour
{
    public TextMeshProUGUI presetsTitle;
    public RectTransform presetsContainer;
    private List<Button> presetButtons;

    private GameObject prefab;
    private ContextPrompt prompt;
    private UnityAction LoadAction;

    private void Start()
    {
        presetButtons = new List<Button>();

        // get existing "prefab" transforms and hide
        prefab = presetsContainer.GetChild(0).gameObject;
        prefab.SetActive(false);
    }

    public void SetClickPrompt(ContextPrompt prompt, UnityAction loadAction)
    {
        this.LoadAction = loadAction;
        this.prompt = prompt;
    }

    public void LoadCollection()
    {
        ClearButtons();

        presetsTitle.text =
            "Presets (" + PresetCollection.loaded.items.Count.ToString() + ")";

        // create button objects with preset name and events
        foreach (var item in PresetCollection.loaded.items)
            CreateButton(item);
    }
    public void CreateButton(PresetProfile item)
    {
        // create and show instance of prefab
        var obj = Instantiate(prefab, presetsContainer);
        obj.gameObject.SetActive(true);

        // set text to preset name
        obj.GetComponentInChildren<TextMeshProUGUI>().text = item.name;

        // set onclick event
        var btn = obj.GetComponent<Button>();
        btn.onClick.AddListener(delegate
        {
            prompt.onYes = delegate { Select(btn); };
            Neat.UI.ContextMenu.instance.Process(prompt);
        });

        // add to list
        presetButtons.Add(btn);
    }
    public void ClearButtons()
    {
        foreach (Button b in presetButtons)
            Destroy(b.gameObject);

        presetButtons = new List<Button>();
    }
    public void Select(int index)
    {
        Select(presetButtons[index]);
    }
    public void Select(Button btn)
    {
        ToggleButtonInteractability(btn);

        int i = presetButtons.IndexOf(btn);
        PresetCollection.SelectedIndex = i;
        PresetProfile.current = PresetCollection.currentItem;

        LoadAction.Invoke();
    }

    private void ToggleButtonInteractability(Button b)
    {
        foreach (Button _b in presetButtons)
        {
            _b.interactable = b != _b;
        }
    }
}
