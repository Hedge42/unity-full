using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.File;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using System.Linq;

namespace Neat.FileBrowser
{
    [RequireComponent(typeof(FileBrowserToolbar))]
    public class FileBrowser : MonoBehaviour
    {
        // singleton
        private static FileBrowser _instance;
        public static FileBrowser instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<FileBrowser>();
                return _instance;
            }
        }

        // references + inspector
        public Transform tfContainer;
        public RectTransform rtPrefab;
        public Color clrClicked;
        private FileBrowserToolbar toolbar;

        // data
        private FileBrowserItem fbiClicked; // support for multiple items?
        public string path;
        public string[] filters;

        private Action<string> actSelect;

        private Canvas _canvas;
        private Canvas canvas
        {
            get
            {
                if (_canvas == null)
                    _canvas = GetComponent<Canvas>();
                return _canvas;
            }
        }

        public bool isActive
        {
            get { return canvas.enabled; }
            set { canvas.enabled = value; }
        }

        // startup
        private void Start()
        {
            toolbar = GetComponent<FileBrowserToolbar>();
            isActive = false;
        }
        public void Show(string path, Action<string> onSelect, params string[] filters)
        {
            isActive = true;
            this.filters = filters;
            this.actSelect = onSelect;
            NavigateTo(path);
        }

        // handling data
        public void LoadItems()
        {
            DestroyItems();

            // clear selected
            fbiClicked = null;

            // list files
            if (path == "" || Directory.Exists(path))
            {
                foreach(var p in GetPaths(path))
                {
                    SpawnItem(p);
                }
            }
            else
            {
                Debug.LogError("Not a valid directory");
                FileSelected(null);
            }
        }
        private string[] GetPaths(string path)
        {
            // assume valid directory

            // returns drive as default
            if (path == "")
            {
                return Directory.GetLogicalDrives();
            }
            else
            {
                var list = new List<string>();

                list.AddRange(Directory.GetDirectories(path));
                list.AddRange(FilterFiles(Directory.GetFiles(path)));

                return list.ToArray();
            }
        }
        private string[] FilterFiles(string[] files)
        {
            return files.Where(s => filters.Contains(Path.GetExtension(s))).ToArray();
        }
        private void DestroyItems()
        {
            var childList = new List<GameObject>();
            foreach (Transform child in tfContainer)
                childList.Add(child.gameObject);
            foreach (GameObject g in childList)
            {
                if (Application.isPlaying)
                    Destroy(g);
                else
                    DestroyImmediate(g);
            }
        }
        private void SpawnItem(string path)
        {
            var itemRect = Instantiate(rtPrefab, tfContainer);
            var fbi = itemRect.GetComponent<FileBrowserItem>();
            fbi.Initialize(path, this);

            // or add an icon
            //if (FileManager.IsDirectory(path))
            if (Directory.Exists(path))
            {
                fbi.SetText(fbi.text + "/");
            }
        }
        public void NavigateTo(string path)
        {
            path = path.Trim();
            if (Directory.Exists(path))
            {
                this.path = path;
                toolbar.ipfPath.text = path;
                LoadItems();
            }
            else if (path == "")
            {
                this.path = path;
                toolbar.ipfPath.text = path;
                LoadItems();
            }
            else
            {
                Debug.LogWarning("Invalid path");
            }
        }

        // click
        public void ItemClicked(FileBrowserItem item)
        {
            if (fbiClicked == item)
            {
                ItemSelected(item);
            }
            else
            {
                UpdateClickedItem(item);
            }
        }
        private void UpdateClickedItem(FileBrowserItem item)
        {
            if (fbiClicked != null)
                fbiClicked.ResetColor();

            fbiClicked = item;
            fbiClicked.SetColor(clrClicked);
        }

        // select
        private void ItemSelected(FileBrowserItem item)
        {
            // if it's a directory
            if (FileManager.IsDirectory(item.path))
            {
                fbiClicked = null;
                NavigateTo(item.path);
            }

            else
            {
                FileSelected(item.path);
            }
        }
        private void FileSelected(string text)
        {
            actSelect?.Invoke(text);

            // reset this component
            actSelect = null;
            fbiClicked = null;
            isActive = false;
        }
    }
}