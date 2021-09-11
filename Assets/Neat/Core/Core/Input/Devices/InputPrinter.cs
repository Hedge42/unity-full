using UnityEngine;

namespace Neat.InputManagement
{
    public class InputPrinter
    {
        private Device device;
        private static GamepadControl[] controls;
        public InputPrinter(Device d)
        {
            device = d;

            if (controls == null)
                controls = (GamepadControl[])System.Enum.GetValues(typeof(GamepadControl));
        }

        public void PrintButtons()
        {
            if (device == null)
            {
                Debug.Log("No device...");
                return;
            }

            // for each keycode, print if down or up
            // for each axis, print value if > 0

            foreach (GamepadControl c in controls)
            {
                if (device.GetButtonDown(c))
                    Debug.Log(device.name + " - " + c.ToString() + " button down");
                else if (device.GetButtonUp(c))
                    Debug.Log(device.name + " - " + c.ToString() + " button up");
            }
        }
        public void PrintAxes()
        {
            if (device == null)
            {
                Debug.Log("No device...");
                return;
            }

            foreach (GamepadControl c in controls)
            {
                float value = device.GetAxis(c);
                if (Mathf.Abs(value) > .2f)
                    Debug.Log(device.name + " - " + c.ToString() + " axis (" + value + ")");
            }
        }
    }
}
