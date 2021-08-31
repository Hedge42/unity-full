using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Music;

namespace Neat.Guitar
{
    public class BeatPlayer : MonoBehaviour
    {
        public BeatDrawer drawer;
        public Metronome metronome;
        public float time;

        private ChartController _chartPlayer;
        public ChartController chartPlayer
        {
            get
            {
                if (_chartPlayer == null)
                    _chartPlayer = GetComponent<ChartController>();
                return _chartPlayer;
            }
        }

        public List<Beat> beats; // current beats

        private float windowDuration
        {
            get
            {
                return chartPlayer.maxTime - chartPlayer.time;
            }
        }

        private void UpdateBeats(float time)
        {
            float start = chartPlayer.time;
            float end = start + windowDuration;

            if (beats == null)
                beats = new List<Beat>();
        }

        public void UpdateTime(float value)
        {
            this.time = value;
        }
    }
}
