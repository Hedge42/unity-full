using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Neat.FileBrowser
{
    public class FileBrowserTest : MonoBehaviour
    {
        public Button btn;
        public TextMeshProUGUI tmp;

        public void Click()
        {
            FileBrowser.instance.Show(Application.persistentDataPath, UpdateText);
        }

        private void UpdateText(string text)
        {
            tmp.text = text;
        }
    }
}
