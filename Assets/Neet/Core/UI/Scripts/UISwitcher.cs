using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UISwitcher : MonoBehaviour
{
    public bool useCanvas;
    public GameObject[] options;

    public int activeOption = 0;

    [ReadOnly]
    public string targetUI;

    private void OnValidate()
    {
        // validate active option
        if (options.Length == 0 || activeOption < 0)
            activeOption = 0;
        else if (activeOption >= options.Length)
            activeOption = options.Length - 1;

        // update text
        if (options.Length == 0)
            targetUI = "[NONE]";
        else
            targetUI = options[activeOption].name;

        // activate
        SwitchTo(activeOption);
    }

    public void SwitchTo(int selected)
    {
        // for each option
        for (int i = 0; i < options.Length; i++)
        {
            GameObject g = options[i];
            Canvas c = g.GetComponent<Canvas>();

            // ignore if null
            if (g == null)
                continue;

            if (useCanvas)
            {
                // enable/disable the canvas 
                if (c != null)
                {
                    // re-enable gameObject if it got switched off
                    g.SetActive(true);

                    c.enabled = i == selected;
                    continue;
                }
            }

            // will not run if canvas is found
            g.SetActive(i == selected);

            // re-enable the canvas if it got switched off
            if (c != null)
                c.enabled = true;
        }
    }
}
