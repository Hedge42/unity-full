﻿using Neat.States;
using UnityEngine;

namespace Neat.Music
{
    public class ChartController : MonoBehaviour
    {
        // references
        public GameObject timeSignaturePrefab;

        private ChartSerializer _serializer;
        public ChartSerializer serializer
        {
            get
            {
                if (_serializer == null)
                    _serializer = GetComponent<ChartSerializer>();
                return _serializer;
            }
        }
        public Chart chart
        {
            get { return serializer.chart; }
            set { serializer.chart = value; }
        }

        public MusicPlayer player;

        private ClockState _clock;
        public ClockState clock
        {
            get
            {
                if (_clock == null)
                    UpdateClockState();
                return _clock;
            }
        }

        private OutputState[] _outputs;
        public OutputState[] outputs
        {
            get
            {
                if (_outputs == null)
                    _outputs = new OutputState[]
                    {
                        scroller,
                        beatDrawer
                    };
                return _outputs;
            }
        }
        private InputState _input;
        public InputState input
        {
            get
            {
                if (_input == null)
                    _input = new ChartControllerInput(this);
                return _input;
            }
        }
        private SkipState _skip;
        public SkipState skip
        {
            get
            {
                if (_skip == null)
                    UpdateSkipState();
                return _skip;
            }
        }
        private TimeScroller _scroller;
        public TimeScroller scroller
        {
            get
            {
                if (_scroller == null)
                    _scroller = GetComponent<TimeScroller>();
                return _scroller;
            }
        }
        private TimingUI _beatDrawer;
        public TimingUI beatDrawer
        {
            get
            {
                if (_beatDrawer == null)
                    _beatDrawer = GetComponent<TimingUI>();
                return _beatDrawer;
            }
        }
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

        

        // 
        public float time
        {
            get
            {
                return clock.GetTime();
            }
        }

        private void Start()
        {
            beatDrawer.DiscardAll();

            if (chart != null)
                Stop();
        }

        private void Update()
        {
            input.GetInput();
        }

        // state updates
        private void UpdateSkipState()
        {
            // update skip state
            if (chart != null && chart.timingMap.timeSignatures.Count > 0)
                _skip = beatDrawer;
            else
                _skip = null;

            print("Skip state → " + _skip?.GetType().ToString());
        }
        private void UpdateClockState()
        {
            bool hadPlayerClock = _clock == null;

            // new clock
            if (player.clock.Playable())
                _clock = player.NewClock();
            else
                _clock = new Stopwatch(this);

            clock.onTick += UpdateTime;

            print("Clock state → " + _clock.GetType().ToString());
        }
        private void UpdateInputState()
        {
            throw new System.NotImplementedException();
        }

        public void SetTime(float t)
        {
            clock.SetTime(t);
            foreach (var o in outputs)
                o.SetTime(t);
        }
        public void UpdateTime(float t)
        {
            foreach (var o in outputs)
                o.UpdateTime(t);
        }
        public void SkipForward()
        {
            skip.SkipForward();
        }
        public void SkipBack()
        {
            skip.SkipBack();
        }
        public void TogglePlay()
        {
            if (clock.isActive)
            {
                clock.Pause();
            }
            else
            {
                clock.Start();
            }
        }
        public void Stop()
        {
            clock.Stop();
            SetTime(0f);
        }

        public void LoadChart(Chart c)
        {
            chart = c;

            print("Loading \"" + c.name + "\"...");

            UpdateClockState();
            Stop();
        }
        public void LoadSerialized()
        {
            chart = serializer.chart;
            Stop();
        }

        public void OnClipReady(AudioClip clip)
        {
            _clock = new AudioClock(player.source, this);
        }
        public void TimingMapChanged()
        {
            beatDrawer.SetTime(time);
        }
    }
}