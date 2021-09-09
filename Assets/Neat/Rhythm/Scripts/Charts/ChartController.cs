using Neat.States;
using UnityEngine;

namespace Neat.Music
{
    public class ChartController : MonoBehaviour
    {
        // references
        private ChartUI _ui;
        public ChartUI ui
        {
            get
            {
                if (_ui == null)
                    _ui = GetComponent<ChartUI>();
                return _ui;
            }
        }
        private ChartStateController _states;
        public ChartStateController states
        {
            get
            {
                if (_states == null)
                    _states = GetComponent<ChartStateController>();
                return _states;
            }
        }

        public float time => states.clock.GetTime();

        public Chart chart
        {
            get { return ui.serializer.chart; }
            set { ui.serializer.chart = value; }
        }

        private void Start()
        {
            ui.timingBar.DiscardAll();

            if (chart != null)
                Stop();
        }

        public void SetTime(float t)
        {
            states.clock.SetTime(t);
            ui.scroller.SetTime(t);
            ui.timingBar.SetTime(t);
        }
        public void UpdateTime(float t)
        {
            ui.scroller.UpdateTime(t);
            ui.timingBar.UpdateTime(t);
        }

        public void SkipForward()
        {
            states.skip.SkipForward();
        }
        public void SkipBack()
        {
            states.skip.SkipBack();
        }
        public void TogglePlay()
        {
            if (states.clock.isActive)
            {
                states.clock.Pause();
            }
            else
            {
                states.clock.Start();
            }
        }
        public void Stop()
        {
            states.clock.Stop();
            SetTime(0f);
        }

        public void LoadChart(Chart c)
        {
            ui.chart = c;

            print("Loading \"" + c.name + "\"...");

            states.UpdateClock();
            Stop();
        }
        public void OnClipReady(AudioClip clip)
        {
            states.SetClock(new AudioClock(ui.player.source, this));
        }
    }
}