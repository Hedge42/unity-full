using Neat.States;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Neat.Music
{
    [ExecuteAlways]
    // controller ??
    public class ChartPlayer : MonoBehaviour, MediaPlayer
    {
        // this class exists for the media functions

        // references
        public MusicPlayer player;
        private ChartSerializer _serializer;
        private NoteReader _noteReader;

        private ChartTimer _timer;
        public ChartTimer timer
        {
            get
            {
                if (_timer == null)
                    _timer = new ChartTimer(this);
                return _timer;
            }
        }


        // ??
        public ChartStateManager states { get; private set; }
        public ChartUI ui { get; private set; } // fuck this thing so much

        private InputState input;
        // ??

        // properties
        public Chart chart
        {
            get { return serializer.chart; }
            set { serializer.chart = value; }
        }
        public float time => player.time;
        public ChartSerializer serializer
        {
            get
            {
                if (_serializer == null)
                    _serializer = GetComponent<ChartSerializer>();
                return _serializer;
            }
        }
        public NoteMap noteMap => chart.noteMaps[0];
        public NoteReader noteReader
        {
            get
            {
                if (_noteReader == null)
                    _noteReader = new NoteReader(this);
                return _noteReader;
            }
        }

        // mono
        private void Awake()
        {
            input = new ChartPlayerInput(this);
            ui = GetComponent<ChartUI>();
            states = GetComponent<ChartStateManager>();

            SetEvents();
        }
        private void SetEvents()
        {
            player.onTick.AddListener(UpdateTime);
            player.onSkip.AddListener(SetTime);
            player.onClipReady.AddListener(delegate { Stop(); });
        }

        // updating 
        public void SetTime(float t)
        {
            ui.scroller.SetTime(t);
            ui.timingBar.SetTime(t);

            // fretboardNoteReader.SetTime(t);
        }
        public void UpdateTime(float t)
        {
            ui.scroller.UpdateTime(t);
            ui.timingBar.UpdateTime(t);

            // fretboardNoteReader.UpdateTime(t);
        }

        // media functions
        public void SkipTo(float t)
        {
            player.SkipTo(t);
        }
        public void SkipForward()
        {
            states.skip.SkipForward();
        }
        public void SkipBack()
        {
            states.skip.SkipBack();
        }
        public void Play()
        {
            player.Play();
        }
        public void Pause()
        {
            player.Pause();
        }
        public void TogglePlay()
        {
            if (player.isPlaying)
                Pause();
            else
                Play();
        }
        public void Stop()
        {
            player.Pause();
            SkipTo(0f);
        }

        // this shouldn't be here - use serializer 
        // or maybe it's fine idk
        private List<Loadable> loadable;
        public void LoadChart(Chart c)
        {
            Stop();
            chart = c;
            ui.timingBar.DiscardAll(); // ???
            player.LoadClip(c.musicPath);

            // load dependent components
            var x = gameObject.GetComponents<Loadable>();
            foreach (var i in x) i.OnLoad(chart);
        }
    }

    public interface Loadable
    {
        void OnLoad(Chart c);
    }
}