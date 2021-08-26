using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Neat.File;

namespace Neat.FileBrowser
{
    [RequireComponent(typeof(FileBrowser))]
    public class FileBrowserToolbar : MonoBehaviour
    {
        private FileBrowser browser;


        public Button btnBack;
        public Button btnForward;
        public Button btnFolderUp;
        public Button btnRefresh;
        public Button btnExplorer;
        public Button btnQuit;

        public TMP_InputField ipfPath;

        private void Start()
        {
            browser = GetComponent<FileBrowser>();
            SetUIEvents();
        }
        private void SetUIEvents()
        {
            // back
            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(Back);

            // forward
            btnForward.onClick.RemoveAllListeners();
            btnForward.onClick.AddListener(Forward);

            // refresh
            btnRefresh.onClick.RemoveAllListeners();
            btnRefresh.onClick.AddListener(Refresh);

            // folder up
            btnFolderUp.onClick.RemoveAllListeners();
            btnFolderUp.onClick.AddListener(FolderUp);

            // explorer
            btnExplorer.onClick.RemoveAllListeners();
            btnExplorer.onClick.AddListener(Explorer);

            // quit
            btnQuit.onClick.RemoveAllListeners();
            btnQuit.onClick.AddListener(Quit);

            // path inputfield
            ipfPath.onSubmit.RemoveAllListeners();
            ipfPath.onSubmit.AddListener(Submit);
        }

        void Submit(string s)
        {
            browser.NavigateTo(s);
        }
        void Quit()
        {

        }
        void Refresh()
        {
            browser.LoadItems();
        }
        void Forward()
        {

        }
        void Back()
        {
        }
        void FolderUp()
        {
            var parent = Directory.GetParent(browser.path);
            if (parent != null && parent.Exists)
            {
                browser.NavigateTo(Directory.GetParent(browser.path).FullName);
            }
            else
            {
                browser.NavigateTo("");
            }
        }
        void Explorer()
        {
            FileManager.OpenExplorer(browser.path);
        }
    }
}
