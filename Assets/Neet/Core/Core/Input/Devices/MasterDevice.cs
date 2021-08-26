using UnityEngine;

namespace Neat.Input
{
    public class MasterDevice : IDevice
    {
        public Device[] devices { get; private set; }

        public MasterDevice(Device[] devices)
        {
            this.devices = devices;
        }

        public bool GetButton(GamepadControl b)
        {
            foreach (Device d in devices)
                if (d.deviceMap.GetControl(b).GetButton())
                    return true;

            return false;
        }
        public bool GetButtonDown(GamepadControl b)
        {
            foreach (Device d in devices)
                if (d.deviceMap.GetControl(b).GetButtonDown())
                    return true;

            return false;
        }
        public bool GetButtonUp(GamepadControl b)
        {
            foreach (Device d in devices)
                if (d.deviceMap.GetControl(b).GetButtonUp())
                    return true;

            return false;
        }
        public float GetAxis(GamepadControl b)
        {
            float value = 0f;

            foreach (Device d in devices)
                value += d.deviceMap.GetControl(b).GetAxis();

            return value;
        }
        public bool AnyButton()
        {
            foreach (GamepadControl c in System.Enum.GetValues(typeof(GamepadControl)))
                if (GetButton(c))
                    return true;

            return false;
        }
        public bool AnyAxis()
        {
            foreach (GamepadControl c in System.Enum.GetValues(typeof(GamepadControl)))
                if (Mathf.Abs(GetAxis(c)) > AxisControl.ANALOG_THRESHOLD)
                    return true;

            return false;
        }
    }
}
