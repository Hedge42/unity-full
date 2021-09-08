using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TMP_Dropdown))]
public class SceneDropdown : MonoBehaviour
{
    [System.Serializable]
    public class SceneItem
    {
        public Object sceneAsset;
        public string label;
    }

    public SceneItem[] sceneItems;

    private TMP_Dropdown dropdown;

    private void Start()
    {
        LoadDropdown();
    }

    public void LoadSelectedScene()
    {
        SceneSwitcher2.instance.SwitchTo(GetSelectedSceneName());
    }
    public string GetSelectedSceneName()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        SceneItem selected = sceneItems[dropdown.value];
        return selected.sceneAsset.name;
    }

    private void LoadDropdown()
    {
        // get list of labels with valid scenes
        List<string> dropdownLabels = new List<string>();
        foreach (SceneItem item in sceneItems)
        {
            if (Application.CanStreamedLevelBeLoaded(item.sceneAsset.name))
                dropdownLabels.Add(item.label);
            else
                Debug.LogError("Scene \'" + item.sceneAsset.name + "\' can't be loaded");
        }


        // apply to dropdown
        dropdown.ClearOptions();
        dropdown.AddOptions(dropdownLabels);
    }
}
