using UnityEngine;

namespace Neet.Input
{
    public class KeyControl : IInputControl
    {
        private KeyCode key;
        private KeyCode negative;
        private bool invert;

        public KeyControl()
        {
            key = KeyCode.None;
            negative = KeyCode.None;
        }
        public KeyControl(KeyCode key)
        {
            this.key = key;
            this.invert = false;
        }
        public KeyControl(KeyCode key, bool invert)
        {
            this.key = key;
            this.invert = invert;
        }
        public KeyControl(KeyCode positive, KeyCode negative)
        {
            this.key = positive;
            this.negative = negative;
        }

        public bool GetButton()
        {
            return UnityEngine.Input.GetKey(key) || UnityEngine.Input.GetKey(negative);
        }

        public bool GetButtonDown()
        {
            return UnityEngine.Input.GetKeyDown(key) || UnityEngine.Input.GetKeyDown(negative);
        }

        public bool GetButtonUp()
        {
            return UnityEngine.Input.GetKeyUp(key) || UnityEngine.Input.GetKeyUp(negative);
        }

        public float GetAxis()
        {
            float value = 0f;

            if (UnityEngine.Input.GetKey(key))
                value += 1f;
            if (UnityEngine.Input.GetKey(negative))
                value = value - 1f;

            if (invert)
                value *= -1;

            return value;
        }
    }
}
