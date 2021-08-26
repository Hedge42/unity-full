using UnityEngine;
using System.IO;
using TMPro;
using Neat.File;
using UnityEngine.UI;

namespace Neat.Scroller
{
    public class PresetScrollerItem : MonoBehaviour
    {
        public TextMeshProUGUI tmpFileName;
        public TextMeshProUGUI tmpFullPath;

        public Image arrowIcon;
        public Image musicFileIcon;
        public Image folderIcon;

        private DirectoryItem item;

#pragma warning disable CS0649
        private ClickStrategy onClick;
        public void ClickButton()
        {
            if (onClick != null)
                onClick.Click();
        }
        public void SetData(DirectoryItem item, PresetScroller manager)
        {
            this.item = item;

            if (item.isOpen)
                arrowIcon.color = Color.white;
            else
                arrowIcon.color = Color.clear;

            if (FileManager.IsDirectory(item.path))
            {
                folderIcon.color = Color.white;
                musicFileIcon.color = Color.clear;
            }
            else
            {
                musicFileIcon.color = Color.white;
                folderIcon.color = Color.clear; ;
            }


            tmpFullPath.text = item.path;

            // https://stackoverflow.com/questions/3736462/getting-the-folder-name-from-a-path
            tmpFileName.text = Path.GetFileName(item.path);
        }
    }
}
