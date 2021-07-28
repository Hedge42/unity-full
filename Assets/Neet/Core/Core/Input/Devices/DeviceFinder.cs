using System;
using UnityEngine;

namespace Neet.Input
{
    public class DeviceFinder
    {
        // run this every frame...
        public Device GetNextDevice()
        {
            // find a keystroke either from some devices, then return that Device
            Device device = null;

            // if multiple keys are pressed in the frame, will go with the first one it finds
            KeyCode key = KeyCode.None;
            var allKeycodes = Enum.GetValues(typeof(KeyCode));
            foreach (KeyCode k in allKeycodes)
                if (UnityEngine.Input.GetKeyDown(k))
                    key = k;

            if (key != KeyCode.None)
            {
                string keystring = key.ToString();
                if (keystring.Contains("Joystick"))
                {
                    // Joystick@Button@
                    // 0.2.4..[8]......
                    // need to find specific joystick index, because generic joystick keycodes exist
                    string joystickIndex = keystring.Substring(8, 1); // unverified
                    int index = 0;

                    if (int.TryParse(joystickIndex, out index))
                        return GetDeviceIfAvailable(index);
                    else
                        Debug.Log("Couldn't parse: " + keystring + "[" + index + "]");
                }
                else
                    // try to return Keyboard
                    return GetDeviceIfAvailable(0);
            }

            // now loop axes...
            foreach (string axis in InputManager.instance.possibleAxes)
            {
                float value = UnityEngine.Input.GetAxis(axis);
                // TODO better place for threshold
                if (Mathf.Abs(value) > .6f)
                {
                    // get device based on axis
                    // get index
                    int joystickIndex = -1; // verify this
                    if (int.TryParse(axis.Substring(InputManager.JOYSTICK_PREFIX.Length, 1), out joystickIndex))
                        return GetDeviceIfAvailable(joystickIndex);
                    else
                        Debug.Log("Oops, couldn't parse index " + axis.Substring(InputManager.JOYSTICK_PREFIX.Length, 1));
                }
            }

            return device;
        }

        private Device GetDeviceIfAvailable(int index)
        {
            Device found = InputManager.instance.devices[index];
            if (!found.isInUse)
            {
                found.isInUse = true;
                return found;
            }
            else
                return null;
        }
    }
}
