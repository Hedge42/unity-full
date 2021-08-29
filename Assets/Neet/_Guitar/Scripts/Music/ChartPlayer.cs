using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Neat.Audio;
using Neat.FileManagement;
using UnityEngine.UI;
using TMPro;
using Neat.FileBrowser;
using System.IO;
using Neat.Music;

namespace Neat.Guitar
{
    using Input = UnityEngine.Input;
    public class ChartPlayer : MonoBehaviour
    {
        public MusicPlayer musicPlayer;
        public Chart chart;

        public RectTransform movingContainer;
        public RectTransform staticWindow;
        public float distancePerSecond;

        public TextMeshProUGUI tmpTitle;
        public TextMeshProUGUI tmpInfo;

        public RectTransform judgementLine;

        private ChartToolbar _toolbar;
        public ChartToolbar toolbar
        {
            get
            {
                if (_toolbar == null)
                    _toolbar = GetComponent<ChartToolbar>();
                return _toolbar;
            }
        }
        private BeatDrawer _beatDrawer;
        public BeatDrawer beatDrawer
        {
            get
            {
                if (_beatDrawer == null)
                    _beatDrawer = GetComponent<BeatDrawer>();
                return _beatDrawer;
            }
        }

        public float time
        {
            get
            {
                return musicPlayer.time;
            }
        }
        public float scrollerEndTime
        {
            get
            {
                return time + Mathf.Abs(judgementLine.position.x - staticWindow.rect.xMax) / distancePerSecond;
            }
        }
        public float scrollerStartTime
        {
            get
            {
                return time - Mathf.Abs(judgementLine.position.x - staticWindow.rect.xMin) / distancePerSecond;
            }
        }

        public TimeSignature timeSignature
        {
            get
            {
                return chart.timingMap.GetSignatureAtTime(time);
            }
        }


        [Range(1, 16)]
        public int snap = 1;

        private void Start()
        {
            print(scrollerStartTime + " < " + time + " < " + scrollerEndTime);

            musicPlayer.onTick += UpdatePosition;
            musicPlayer.onClipLoaded += OnMusicLoaded;

        }

        private void Update()
        {
            UpdateInfoText();
        }

        private IEnumerator _Play()
        {
            yield return null;
        }

        public void DrawBars()
        {
            beatDrawer.DrawBars(0f, 10f);
        }

        public void LoadChart(Chart c)
        { 
            chart = c;
            chart.timingMap.onChange.RemoveAllListeners();
            chart.timingMap.onChange.AddListener(TimingChanged);

            // chart.musicPath

            // load music from chart path

            DrawBars();
        }
        public void TimingChanged()
        {
            DrawBars();
        }

        public void OnMusicLoaded(AudioClip clip)
        {
            print("found " + clip.name);

            chart.duration = clip.length;
            UpdateWidth();
        }

        public void UpdateWidth()
        {
            var newx = chart.duration * distancePerSecond;
            movingContainer.sizeDelta = new Vector2(newx, movingContainer.sizeDelta.y);
        }

        private void UpdatePosition(float f)
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