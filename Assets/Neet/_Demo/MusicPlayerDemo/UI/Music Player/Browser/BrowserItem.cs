using UnityEngine;
using System.IO;
using TMPro;
using Neet.File;
using UnityEngine.UI;

namespace Neet.Scroller
{
    public class BrowserItem : MonoBehaviour
    {
        public TextMeshProUGUI tmpFileName;
        public TextMeshProUGUI tmpFullPath;

        public Image arrowIcon;
        public Image musicFileIcon;
        public Image folderIcon;

        private DirectoryItem item;

        private ClickStrategy onClick;
        public void ClickButton()
        {
            if (onClick != null)
                onClick.Click();
        }
        public void SetData(DirectoryItem item, BrowserManager manager)
        {
            this.item = item;

            if (item.isOpen)
                arrowIcon.color = Color.white;
            else
                arrowIcon.color = Color.clear;

            if (FileManager.instance.IsDirectory(item.path))
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

            if (FileManager.instance.IsDirectory(item.path))
                onClick = new FolderClickStrategy(item, manager);
            else
                onClick = new FileClickStrategy(item);
        }
    }
}
