using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.States;

namespace Neat.Music
{
    using Input = UnityEngine.Input;
    public class ChartControllerInput : InputState
    {
        public ChartController controller;

        public ChartControllerInput(ChartController controller)
        {
            this.controller = controller;
        }
        public void GetInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                controller.TogglePlay();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                controller.SkipForward();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                controller.SkipBack();
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                controller.Stop();
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                //controller.EditTimeSignature();

                var t = new TimeSignature(controller.time);
                controller.chart.timingMap.AddTimeSignature(t);
                // TimeSignatureWindow.Edit()
            }

            var wheel = Input.mouseScrollDelta;
            if (wheel.y < 0)
            {
                controller.SkipForward();
            }
            else if (wheel.y > 0)
            {
                controller.SkipBack();
            }
        }
    }
}