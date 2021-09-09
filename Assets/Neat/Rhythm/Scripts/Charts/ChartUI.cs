using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Neat.States;

namespace Neat.Music
{
    public class ChartUI : MonoBehaviour
    {
        public GameObject timeSignaturePrefab;
        public NoteUI notePrefab;
        public MusicPlayer player;
        public Chart chart
        {
            get { return serializer.chart; }
            set { serializer.chart = value; }
        }

        // component properties
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
        private Track _track;
        public Track track
        {
            // simplify this
            get
            {
                if (_track == null)
                {
                    if (chart.tracks.Count == 0)
                        chart.tracks.Add(new Track());
                    _track = chart.tracks[0];
                }
                return _track;
            }
            set
            {
                _track = value;
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

        private TimingBar _timingBar;
        public TimingBar timingBar
        {
            get
            {
                if (_timingBar == null)
                    _timingBar = GetComponent<TimingBar>();
                return _timingBar;
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
    }
}