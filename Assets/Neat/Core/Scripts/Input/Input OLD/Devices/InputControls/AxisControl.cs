using UnityEngine;

namespace Neat.GameManager
{
    public class AxisControl : IInputControl
    {
        public const float BUTTON_THRESHOLD = .6f;
        public const float ANALOG_THRESHOLD = .15f;

        private string name;
        private bool invert;
        private bool flip;

        private bool isPressed;
        private bool wasReleased;

        private bool ignorePositive;
        private bool ignoreNegative;

        public AxisControl(string name)
        {
            this.name = name;
            invert = false;
            flip = false;
        }
        public AxisControl(string name, bool invert)
        {
            this.name = name;
            this.invert = invert;
            flip = false;
        }
        public AxisControl(string name, bool invert, bool flip)
        {
            this.name = name;
            this.invert = invert;
            this.flip = flip;
        }
        public AxisControl(string name, bool invert, bool flip, bool ignorePositive)
        {
            this.name = name;
            this.invert = invert;
            this.flip = flip;

            this.ignorePositive = ignorePositive;
            this.ignoreNegative = !ignorePositive;
        }

        public bool GetButton()
        {
            // absolute value because (-)threshold counts as being pressed too
            return Mathf.Abs(GetAxis()) > BUTTON_THRESHOLD;
        }
        public bool GetButtonDown()
        {
            // GetButton() modifies the flag, so check the flag before calling the method
            return !isPressed && GetButton();
        }
        public bool GetButtonUp()
        {
            // GetButton() modifies the flag, so check the flag before calling the method
            return !wasReleased && !GetButton();
        }
        public float GetAxis()
        {
            float value = UnityEngine.Input.GetAxis(name);

            // before or after inversion?
            if ((ignorePositive && value > 0f) || (ignoreNegative && value < 0f))
                value = 0f;

            if (invert)
                value *= -1;

            if (Mathf.Abs(value) > BUTTON_THRESHOLD)
            {
                isPressed = true;
                wasReleased = false;
            }
            else
            {
                isPressed = false;
                wasReleased = true;
            }

            if (flip)
                value = 1 - value;

            if (Mathf.Abs(value) < ANALOG_THRESHOLD)
                return 0f;

            return value;
        }

        // These are for getting buttons out of 1D axes that range (+-1)
        public AxisControl GetNegative()
        {
            return new AxisControl(name, invert, flip, true);
        }
        public AxisControl GetPositive()
        {
            return new AxisControl(name, invert, flip, false);
        }
    }
}
