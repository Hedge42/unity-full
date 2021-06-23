using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher2 : MonoBehaviour
{
    public static SceneSwitcher2 instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SwitchTo(int scene)
    {
        try
        {
            SceneManager.LoadScene(scene);
        }
        catch
        {
            Debug.LogError("Scene [" + scene.ToString() + "] can't be loaded");
        }
    }

    public void SwitchTo(string name)
    {
        try
        {
            SceneManager.LoadScene(name);
        }
        catch
        {
            Debug.LogError("Scene + \'" + name + "\' can't be loaded");
        }
    }

    public void SwitchTo(Object scene)
    {
        SwitchTo(scene.name);
    }
}
