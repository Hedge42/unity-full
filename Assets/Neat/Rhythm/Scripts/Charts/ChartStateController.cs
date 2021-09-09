using UnityEngine;
using Neat.States;

namespace Neat.Music
{
    public class ChartStateController : MonoBehaviour
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

        private ClockState _clock;
        public ClockState clock
        {
            get
            {
                if (_clock == null)
                    UpdateClock();
                return _clock;
            }
            private set
            {
                _clock = value;
                Debug.Log("Clock state → " + _clock.ToString());
            }
        }
        private InputState _input;
        public InputState input
        {
            get
            {
                if (_input == null)
                    _input = new ChartControllerInput(controller); // default
                return _input;
            }
            private set
            {
                _input = value;
                Debug.Log("Input state → " + _input.ToString());
            }
        }

        private SkipState _skip;
        public SkipState skip
        {
            get
            {
                if (_skip == null)
                    UpdateSkip(); // set default
                return _skip;
            }
            private set
            {
                _skip = value;
                Debug.Log("Skip state → " + _skip.ToString());
            }
        }

        public void Update()
        {
            input.GetInput();
        }

        public void UpdateInput()
        {
            // set default
            input = new ChartControllerInput(controller);
        }
        public void SetInput(InputState s)
        {
            input = s;
        }

        public void UpdateSkip()
        {
            // set default or calculate
            if (controller.chart.timingMap.timeSignatures.Count > 0)
                skip = controller.ui.timingBar;
            else
                throw new System.NotImplementedException();
        }

        public void UpdateClock()
        {
            bool hadPlayerClock = _clock == null;

            // new clock
            if (controller.ui.player.clock.Playable())
                clock = controller.ui.player.NewClock();
            else
                clock = new Stopwatch(this);

            clock.onTick += controller.UpdateTime;
        }
        public void SetClock(ClockState s)
        {
            clock = s;
        }
    }
}
