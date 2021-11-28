using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

namespace Neat.GameManager.FileBrowser
{


    public class FileBrowserItem : MonoBehaviour
    {
        [HideInInspector] public FileBrowser browser;
        [HideInInspector] public string path;
        [HideInInspector] public string text;


        private TextMeshProUGUI tmp;
        private Button btn;
        private Color startColor;

        public void Initialize(string path, FileBrowser browser, bool isDirectory = false)
        {
            gameObject.SetActive(true);
            this.browser = browser;

            tmp = GetComponentInChildren<TextMeshProUGUI>();
            this.path = path;
            SetText(new DirectoryInfo(path).Name);

            btn = GetComponent<Button>();
            this.startColor = btn.image.color;
            btn.onClick.AddListener(ButtonClick);
        }

        public void ResetColor()
        {
            btn.image.color = startColor;
        }
        public void SetColor(Color c)
        {
            btn.image.color = c;
        }
        public void SetText(string s)
        {
            text = s;
            tmp.text = s;
        }

        private void ButtonClick()
        {
            browser.ItemClicked(this);
        }

    }
}