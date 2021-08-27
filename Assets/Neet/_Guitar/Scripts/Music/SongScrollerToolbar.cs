using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Neat.Audio;
using Neat.File;

namespace Neat.Guitar
{
    public class SongScrollerToolbar : MonoBehaviour
    {
        private SongScroller _scroller;
        public SongScroller scroller
        {
            get
            {
                if (_scroller == null)
                    _scroller = GetComponent<SongScroller>();
                return _scroller;
            }
        }

        public Button btnTimeSignature;
        public TMP_InputField ipfNumerator;
        public TMP_InputField ipfDenominator;

        // bpm
        public Button btnEditBpm;
        public Button btnBpmDown;
        public TMP_InputField ipfBpm;
        public Button btnBpmUp;

        // scroller.snapping
        public Button btnSnapping;
        public Button btnSnappingDown;
        public TMP_InputField ipfSnapping;
        public Button btnSnappingUp;

        // scroller.chart management
        public Button btnNewChart;
        public Button btnLoadChart;
        public Button btnSaveChart;
        public Button btnFindMusic;

        public Button btnChartName;
        public TMP_InputField ipfChartName;

        private void Start()
        {
            SetUIEvents();
        }

        private void SetUIEvents()
        {
            ipfNumerator.onValueChanged.AddListener(NumeratorInputChange);
            ipfDenominator.onValueChanged.AddListener(DenominatorInputChange);

            btnBpmDown.onClick.AddListener(BpmDownClick);
            btnBpmUp.onClick.AddListener(BpmUpClick);
            ipfBpm.onValueChanged.AddListener(BpmInputchanged);

            btnSnappingDown.onClick.AddListener(SnappingDownClick);
            btnSnappingUp.onClick.AddListener(SnappingUpClick);
            ipfSnapping.onValueChanged.AddListener(SnappingInputChange);

            btnFindMusic.onClick.AddListener(FindMusicClick);
            btnNewChart.onClick.AddListener(NewChartClick);
            btnLoadChart.onClick.AddListener(LoadChartClick);
            btnSaveChart.onClick.AddListener(SaveChartClick);
        }
        // need reference to current time signature
        public void NumeratorInputChange(string s) { }
        public void DenominatorInputChange(string s) { }

        public void BpmClick() { }
        public void BpmDownClick()
        {
            var ts = scroller.chart.timingMap.GetTimeSignature(scroller.player.time);
            ts.beatsPerMinute -= 1;
        }
        public void BpmInputchanged(string s)
        {
            // parse input field float
        }
        public void BpmUpClick()
        {
            var ts = scroller.chart.timingMap.GetTimeSignature(scroller.player.time);
            ts.beatsPerMinute += 1;
        }

        public void SnappingClick() { }
        public void SnappingDownClick()
        {
            scroller.snap = Mathf.Clamp(scroller.snap - 1, 1, 16);
        }
        public void SnappingInputChange(string s)
        {
            // parse int from field
        }
        public void SnappingUpClick()
        {
            scroller.snap = Mathf.Clamp(scroller.snap + 1, 1, 16);
        }

        public void NewChartClick()
        {
            scroller.chart = new Chart();
        }
        public void LoadChartClick()
        {
            // make this one line, instead of needing 3 methods
            Neat.FileBrowser.FileBrowser.instance.
                Show(Chart.directory, OnChartSelect, Chart.ext);
        }
        private void OnChartSelect(string path)
        {
            // deserialize scroller.chart at path


        }
        public void SaveChartClick()
        {
            // serialize scroller.chart at path

            // Chart.directory 

            var chart = scroller.chart;

            FileManager.SerializeBinary(chart, Chart.directory + chart.name + Chart.ext);
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