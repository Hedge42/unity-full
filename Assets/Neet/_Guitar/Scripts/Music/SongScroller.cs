using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Neat.Audio;
using Neat.File;
using UnityEngine.UI;
using TMPro;
using Neat.FileBrowser;
using System.IO;

namespace Neat.Guitar
{
    using Input = UnityEngine.Input;
    public class SongScroller : MonoBehaviour
    {
        private SongScrollerToolbar _toolbar;
        public SongScrollerToolbar toolbar
        {
            get
            {
                if (_toolbar == null)
                    _toolbar = GetComponent<SongScrollerToolbar>();
                return _toolbar;
            }
        }

        public MusicPlayer player;
        public Chart chart;

        public RectTransform movingContainer;
        public float distancePerSecond;

        public TextMeshProUGUI tmpTitle;
        public TextMeshProUGUI tmpInfo;

        [Range(1, 16)]
        public int snap = 1;

        private void Start()
        {
            player.onTick += UpdateTime;

            chart = null;
        }

        private void Update()
        {
            UpdateInfoText();
        }

        private void UpdateTime(float f)
        {
            movingContainer.anchoredPosition = Vector2.left * f * distancePerSecond;
        }

        void UpdateInfoText()
        {
            if (chart == null)
            {
                tmpInfo.text = "No chart loaded";
            }
            else if (chart.path == null || !System.IO.File.Exists(chart.path))
            {
                tmpInfo.text = "No music found";
            }
            else
            {
                tmpInfo.text = "";
            }
            // path is null, blank, or invalid
        }
    }
}