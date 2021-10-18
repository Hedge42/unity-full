using UnityEngine;
using Neat.States;

namespace Neat.Music
{
    public class ChartStateManager : MonoBehaviour
    {
        private ChartPlayer _controller;
        public ChartPlayer controller
        {
            get
            {
                if (_controller == null)
                    _controller = GetComponent<ChartPlayer>();
                return _controller;
            }
        }

        private InputState _input;
        public InputState input
        {
            get
            {
                if (_input == null)
                    _input = new ChartPlayerInput(controller); // default
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
                    UpdateSkipState(); // set default
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
            input = new ChartPlayerInput(controller);
        }
        public void SetInput(InputState s)
        {
            input = s;
        }

        public void UpdateSkipState()
        {
            // set default or calculate
            if (controller.chart.timingMap.signatures.Count > 0)
                skip = controller.ui.timingBar;
            else
                throw new System.NotImplementedException();
        }
    }
}
