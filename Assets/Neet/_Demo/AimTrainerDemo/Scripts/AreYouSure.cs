using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

public class ContextMenu : MonoBehaviour
{
    

    public TextMeshProUGUI text;
    public Button btnYes;
    public Button btnNo;

    private Canvas canvas;
    private UnityAction onYes = null;
    private UnityAction onNo = null;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        // button clicks should always hide this panel
        UnityAction hide = delegate
        {
            canvas.enabled = false;
        };

        btnYes.onClick.AddListener(hide);
        btnNo.onClick.AddListener(hide);

        // allows changing yes/no behavior
        // without changing onClick listeners
        UnityAction _onYes = delegate
        {
            onYes?.Invoke();
        };
        UnityAction _onNo = delegate
        {
            onNo?.Invoke();
        };

        btnYes.onClick.AddListener(_onYes);
        btnNo.onClick.AddListener(_onNo);
    }

    public void Ask(string prompt, string yesText, string noText,
        UnityAction yesEvent, UnityAction noEvent = null)
    {
        // show canvas and set text
        canvas.enabled = true;
        text.text = prompt;
        btnYes.GetComponentInChildren<TextMeshProUGUI>().text = yesText;
        btnNo.gameObject.SetActive(true);
        btnNo.GetComponentInChildren<TextMeshProUGUI>().text = noText;

        // set internal events, referenced by buttons
        onYes = yesEvent;
        onNo = noEvent;
    }

    public void Inform(string prompt, string okText)
    {
        // only prompts with "OK" button

        // show canvas and set text
        canvas.enabled = true;
        text.text = prompt;
        btnYes.GetComponentInChildren<TextMeshProUGUI>().text = okText;
        btnNo.gameObject.SetActive(false);

        // buttons will still hide canvas
        onYes = null;
        onNo = null;
    }
}

public class ConfirmationPrompt
{
    public ContextMenu ui;
    public string infoText = "Are you sure?";
    public string yesText = "Yes";
    public string noText = "No";

    public Func<bool> shouldShow = delegate { return true; };
    public UnityAction onYes = null;
    public UnityAction onNo = null;

    public ConfirmationPrompt(ContextMenu ui)
    {
        this.ui = ui;
    }

    public void Ask()
    {
        if (shouldShow())
        {
            ui.Ask(infoText, yesText, noText, onYes, onNo);
        }
        else
        {
            onYes?.Invoke();
        }
    }

    public void Inform()
    {
        if (shouldShow())
        {
            ui.Inform(infoText, yesText);
        }
        else
        {
            onYes?.Invoke();
        }
    }
}