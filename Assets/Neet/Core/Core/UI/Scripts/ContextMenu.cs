using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

namespace Neet.UI
{
    public class ContextMenu : MonoBehaviour
    {
        public static ContextMenu instance { get; private set; }

        public TextMeshProUGUI text;
        public Button btnYes;
        public Button btnNo;

        private Canvas canvas;
        private UnityAction onYes = null;
        private UnityAction onNo = null;

        private void Awake()
        {
            if (instance == null)
                instance = this;

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

        /// <summary>
        /// Equivalent to ContextMenu.Show, redundant for semantics
        /// </summary>
        public void Process(ContextPrompt c)
        {
            Show(c.infoText, c.shouldShow, c.yesText, c.noText, c.onYes, c.onNo);
        }
        /// <summary>
        /// Equivalent to ContextMenu.Process, redundant for semantics
        /// </summary>
        public void Show(ContextPrompt c)
        {
            Show(c.infoText, c.shouldShow, c.yesText, c.noText, c.onYes, c.onNo);
        }

        public void Show(string text, Func<bool> shouldshow = null,
        string yesText = "Okay", string noText = "Nevermind",
        UnityAction yesAction = null, UnityAction noAction = null)
        {
            // user actions called by buttons
            onYes = yesAction;
            onNo = noAction;

            // show unless manually filtered out
            if (shouldshow == null || shouldshow())
            {
                // show canvas and set text
                canvas.enabled = true;
                this.text.text = text;
                btnYes.GetComponentInChildren<TextMeshProUGUI>().text = yesText;
                btnNo.GetComponentInChildren<TextMeshProUGUI>().text = noText;

                // only show no button if there is a confirmation action
                btnNo.gameObject.SetActive(yesAction != null);

            }

            // shouldShow is false, just run the method
            else
            {
                onYes.Invoke();
            }
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

    public class ContextPrompt
    {
        public ContextMenu ui;
        public string infoText = "Are you sure?";
        public string yesText = "Yes";
        public string noText = "No";

        public Func<bool> shouldShow = delegate { return true; };
        public UnityAction onYes = null;
        public UnityAction onNo = null;

        /// <summary>
        /// Calls ContextMenu singleton instance
        /// </summary>
        public void Process()
        {
            Neet.UI.ContextMenu.instance.Process(this);
        }
    }
}