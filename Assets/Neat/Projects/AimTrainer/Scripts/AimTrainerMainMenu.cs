using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimTrainerMainMenu : MonoBehaviour
{
    public Button btnPlay;
    public Button btnReticleEditor;

    private void Start()
    {
        btnPlay.onClick.AddListener(delegate
        {
            SceneSwitcher2.instance.SwitchTo(1);
        });
        btnReticleEditor.onClick.AddListener(delegate
        {
            SceneSwitcher2.instance.SwitchTo(3);
        });
    }
}
