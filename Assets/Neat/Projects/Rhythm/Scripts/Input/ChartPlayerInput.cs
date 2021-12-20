using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.States;

namespace Neat.Audio.Music
{
    // media player input?
    using Input = UnityEngine.Input;
    public class ChartPlayerInput : InputState
    {
        public ChartPlayer player;

        public ChartPlayerInput(ChartPlayer player)
        {
            this.player = player;
        }
        public void GetInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.TogglePlay();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                player.SkipForward();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                player.SkipBack();
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                player.Stop();
            }

            // move this logic
            else if (Input.GetKeyDown(KeyCode.T))
            {
                //controller.EditTimeSignature();
                var t = new TimeSignature(player.time);
                player.chart.timingMap.Add(t);
                // TimeSignatureWindow.Edit()
            }

            HandleScrollWheel();
        }

        private void HandleScrollWheel()
        {
            var wheel = Input.mouseScrollDelta;

            if (wheel.y < 0)
                player.SkipForward();
            else if (wheel.y > 0)
                player.SkipBack();
        }
    }
}