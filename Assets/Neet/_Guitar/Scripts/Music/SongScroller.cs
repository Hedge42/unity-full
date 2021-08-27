using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Neat.Audio;
using Neat.File;
using UnityEngine.UI;
using TMPro;
using Neat.FileBrowser;

namespace Neat.Guitar
{
    using Input = UnityEngine.Input;
    public class SongScroller : MonoBehaviour
    {
        public MusicPlayer player;

        // maybe these should be in a toolbar?

        // time signature
        public Button btnTimeSignature;
        public TMP_InputField ipfNumerator;
        public TMP_InputField ipfDenominator;

        // bpm
        public Button btnEditBpm;
        public Button btnBpmDown;
        public TMP_InputField ipfBpm;
        public Button btnBpmUp;

        // snapping
        public Button btnSnapping;
        public Button btnSnappingDown;
        public TMP_InputField ipfSnapping;
        public Button btnSnappingUp;

        // chart management
        public Button btnNewChart;
        public Button btnLoadChart;
        public Button btnSaveChart;
        public Button btnFindMusic;

        public Chart chart;

        public RectTransform movingContainer;
        public float distancePerSecond;

        [Range(1, 16)]
        public int snap = 1;

        private void Start()
        {
            player.onTick += UpdateTime;

            if (chart == null)
            {
                chart = new Chart();
            }

            SetUIEvents();
        }
        private void UpdateTime(float f)
        {
            movingContainer.anchoredPosition = Vector2.left * f * distancePerSecond;
        }


        private void SetUIEvents()
        {
            btnFindMusic.onClick.AddListener(FindMusicClick);
        }
        // need reference to current time signature
        public void NumeratorInputChange() { }
        public void DenominatorInputChange() { }

        public void BpmClick() { }            
        public void BpmDownClick()
        {
            var ts = chart.timingMap.GetTimeSignature(player.time);
            ts.beatsPerMinute -= 1;
        }
        public void BpmInputchanged()
        {
            // parse input field float
        }
        public void BpmUpClick()
        {
            var ts = chart.timingMap.GetTimeSignature(player.time);
            ts.beatsPerMinute += 1;
        }

        public void SnappingClick() { }
        public void SnappingDownClick()
        {
            snap = Mathf.Clamp(snap - 1, 1, 16);
        }
        public void SnappingInputChange()
        {
            // parse int from field
        }
        public void SnappingUpClick()
        {
            snap = Mathf.Clamp(snap + 1, 1, 16);
        }

        public void NewChartClick()
        {
            chart = new Chart();
        }
        public void LoadChartClick()
        {
            Neat.FileBrowser.FileBrowser.instance.
                Show(FileManager.RootPath, OnChartSelect, ".blb");
        }
        private void OnChartSelect(string path)
        {
            // deserialize chart at path
        }
        public void SaveChartClick()
        {
            // serialize chart at path
        }
        public void FindMusicClick()
        {
            Neat.FileBrowser.FileBrowser.instance.
                Show(FileManager.RootPath, OnMusicSelect, ".mp3", ".wav");
        }
        private void OnMusicSelect(string path)
        {
            AudioManager.LoadClip(path, AudioManager.instance.musicSource, SongLoaded);
        }
        private void SongLoaded()
        {
            // throw new System.NotImplementedException();

            print("do something");
        }

    }
}