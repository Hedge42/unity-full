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
using UnityEngine.Events;

namespace Neat.Guitar
{
    using Input = UnityEngine.Input;
    public class ChartController : MonoBehaviour
    {
        public MusicPlayer player;
        public Metronome metronome;

        public Chart chart;

        public RectTransform movingContainer;
        public RectTransform staticWindow;

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

        public bool isPaused;
        // 
        private float _time;
        public float time
        {
            get
            {
                if (player.hasClip)
                    _time = player.time;

                // else use internal time
                return _time;
            }
            set
            {
                OnSkip(value);
            }
        }



        // TODO change what's serialized, maybe min-max time?
        public float distancePerSecond;
        public float maxTime
        {
            get
            {
                var maxPos = staticWindow.position + Vector3.right * staticWindow.rect.width / 2;
                var rightOffset = judgementLine.InverseTransformPoint(maxPos) / distancePerSecond;
                return time + rightOffset.x;
            }
        }
        public float minTime
        {
            get
            {
                var minPos = staticWindow.position + Vector3.left * staticWindow.rect.width / 2;
                var leftOffset = judgementLine.InverseTransformPoint(minPos) / distancePerSecond;
                return time + leftOffset.x; // leftOffset will be negative
            }
        }
        public float scrollFullDuration
        {
            get { return maxTime - minTime; }
        }
        public float scrollDuration
        {
            get { return maxTime - time; }
        }

        // obsolete remove me
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
            player.onSourceTick += OnTimeTick;
            player.onSkip += OnSkip;
            player.onClipLoaded += OnMusicLoaded;

            beatDrawer.onBeatPassed.AddListener(PlayMetronome);

            OnSkip(0f);
        }

        private void PlayMetronome(Beat b)
        {
            metronome.Play(b.isMeasureStart);
        }

        public void LoadChart(Chart c)
        {
            if (c == null)
                c = new Chart();

            chart = c;
            chart.timingMap.onChange.RemoveAllListeners();
            chart.timingMap.onChange.AddListener(TimingMapChanged);

            // onChartLoaded?
            UpdateWidth();
            OnSkip(0f);
            print("Loaded chart: " + chart.name);
        }
        public void TimingMapChanged()
        {
            beatDrawer.OnTimeSet(time);
        }


        // event handling
        public void OnTimeTick(float t)
        {
            UpdatePosition(t);
        }
        public void OnSkip(float value)
        {
            UpdatePosition(value);
            beatDrawer.OnTimeSet(value);
        }
        public void OnMusicLoaded(AudioClip clip)
        {
            chart.duration = clip.length;
            UpdateWidth();
        }


        // transforming
        public void UpdateWidth()
        {
            float newx;
            if (player.hasClip)
                newx = player.clip.length * distancePerSecond;
            else
                newx = 0f;

            movingContainer.sizeDelta = new Vector2(newx, movingContainer.sizeDelta.y);
        }
        private void UpdatePosition(float f)
        {
            movingContainer.anchoredPosition = Vector2.left * f * distancePerSecond;
        }
        private void UpdateInfoText()
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