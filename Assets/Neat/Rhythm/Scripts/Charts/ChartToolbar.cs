using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Neat.Audio;
using Neat.FileManagement;

namespace Neat.Music
{
    public class ChartToolbar : MonoBehaviour
    {
        private ChartController _controller;
        public ChartController controller
        {
            get
            {
                if (_controller == null)
                    _controller = GetComponent<ChartController>();
                return _controller;
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

        // time signature
        public Button btnNewTimeSignature;

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
            ipfNumerator.onValueChanged.AddListener(NumeratorInputChange);
            ipfDenominator.onValueChanged.AddListener(DenominatorInputChange);

            btnNewTimeSignature.onClick.AddListener(NewTimeSignatureClick);

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
        public void NewTimeSignatureClick()
        {
            bool validNum = int.TryParse(ipfNumerator.text, out int num);
            bool validDen = int.TryParse(ipfDenominator.text, out int den);
            bool validBpm = float.TryParse(ipfBpm.text, out float bpm);

            if (validNum && validDen && validBpm)
            {
                var ts = new TimeSignature();
                ts.offset = controller.time;
                ts.numerator = num;
                ts.denominator = den;
                ts.beatsPerMinute = bpm;
                controller.chart.timingMap.Add(ts);
            }

        }
        public void NumeratorInputChange(string s) { }
        public void DenominatorInputChange(string s) { }

        public void BpmClick() { }
        public void BpmDownClick()
        {
            //var ts = player.chart.timingMap.GetSignatureAtTime(player.player.time);
            //ts.beatsPerMinute -= 1;
        }
        public void BpmInputchanged(string s)
        {
            // parse input field float
        }
        public void BpmUpClick()
        {
            //var ts = player.chart.timingMap.GetSignatureAtTime(player.player.time);
            //ts.beatsPerMinute += 1;
        }

        public void UpdateBpmText()
        {
            // ipfBpm.text = player.timeSignature.beatsPerMinute.ToString("f3");
        }

        public void SnappingClick() { }
        public void SnappingDownClick()
        {
            // player.snap = Mathf.Clamp(player.snap - 1, 1, 16);
        }
        public void SnappingInputChange(string s)
        {
            // parse int from field
        }
        public void SnappingUpClick()
        {
            // player.snap = Mathf.Clamp(player.snap + 1, 1, 16);
        }

        public void NewChartClick()
        {
            controller.LoadChart(new Chart());
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

            var chart = controller.chart;

            FileManager.SerializeBinary(chart, Chart.directory + chart.name + Chart.ext);
        }
        public void FindMusicClick()
        {
            controller.ui.player.LoadFile();
        }





    }

    // idea
    public class ToolbarState
    {
        bool hasChart;
        bool hasMusic;
        bool hasTimeSignature;
    }

}