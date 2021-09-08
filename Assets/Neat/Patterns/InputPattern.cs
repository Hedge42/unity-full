using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace Neat.Patterns
{
    public interface IInput
    {
        void GetInput();
    }
    public interface IControl
    {
        bool WasPressed();
        bool IsPressed();
        bool WasReleased();
        float Axis();
    }
    public class KeyControl : IControl
    {
        public KeyCode key;

        public bool WasPressed()
        {
            return Input.GetKeyDown(key);
        }
        public bool IsPressed()
        {
            return Input.GetKey(key);
        }
        public bool WasReleased()
        {
            return Input.GetKeyUp(key);
        }
        public float Axis()
        {
            return IsPressed() ? 1f : 0f;
        }
    }
    public class AxisControl : IControl
    {
        public const float threshold = .2f;
        public string axis;

        private bool isDown;

        public float Axis()
        {
            float value = Input.GetAxis(axis);
            isDown = value > threshold;

            return value;
        }
        public bool IsPressed()
        {
            return Axis() > threshold;
        }
        public bool WasPressed()
        {
            return false;
        }
        public bool WasReleased()
        {
            return isDown && !IsPressed();
        }
    }
    public class InputPattern : IInput
    {
        List<IControl> controls;

        public void GetInput()
        {
            foreach (IControl i in controls)
            {
            }
        }

        public void Jump()
        {

        }
        public void Pause()
        {

        }
    }
    public class InputPatternUI
    {
        public InputPattern input;


    }
}
