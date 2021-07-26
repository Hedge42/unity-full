using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;
using Neet.Extensions;

public class SceneSwitcher : MonoBehaviour
{
    public Transform elementContainer;
    public GameObject elementPrefab;

    private void Awake()
    {
        elementContainer.DestroyChildren();

        var sceneNames = GetSceneNames();
        for (int i = 0; i < sceneNames.Length; i++)
        {
            var go = Instantiate(elementPrefab, elementContainer);
            Button button = null;
            if (go != null) button = go.GetComponent<Button>();
            int sceneIndex = i; // needed because referring to just 'i' acts as a pointer
            UnityAction buttonEvent = delegate { SceneManager.LoadScene(sceneIndex); };
            //UnityAction buttonEvent = delegate { print(sceneNames[sceneIndex]); };

            var tmp = go.GetComponentInChildren<TextMeshProUGUI>();

            if (button != null)
            {
                button.onClick.AddListener(buttonEvent);
                button.interactable = sceneNames[i] != SceneManager.GetActiveScene().name;
            }
            if (tmp != null)
                tmp.text = sceneNames[i];
        }
    }

    private static string[] GetSceneNames()
    {
        int numScenes = SceneManager.sceneCountInBuildSettings;
        var sceneNames = new string[numScenes];
        for (int i = 0; i < numScenes; i++)
            sceneNames[i] = NameFromIndex(i);
        return sceneNames;
    }
    private static string NameFromIndex(int BuildIndex)
    {
        // https://answers.unity.com/questions/1262342/how-to-get-scene-name-at-certain-buildindex.html
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }
}
