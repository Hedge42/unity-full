using UnityEngine;
using Neat.InputManagement.DeviceMaps;
using System;

namespace Neat.InputManagement
{
    public class Device : IDevice
    {
        public enum DeviceType
        {
            Unsupported,
            Keyboard,
            Xbox,
            PlayStation,
        }



        public string name { get; private set; }
        public bool isInUse { get; set; }
        public DeviceType deviceType { get; private set; }
        public IDeviceMap deviceMap { get; private set; }
        public int index { get; private set; }

        private InputPrinter inputPrinter;

        // Sets deviceMap based on parameters
        public Device(string name, int index)
        {
            this.index = index;
            this.name = name;
            this.isInUse = false;
            deviceType = GetDeviceType(name);

            switch (deviceType)
            {
                case DeviceType.Keyboard:
                    deviceMap = new KeyboardMap();
                    break;
                case DeviceType.Xbox:
                    deviceMap = new XboxOneMap(index);
                    break;
                case DeviceType.PlayStation:
                    deviceMap = new PlayStation4Map(index);
                    break;
                case DeviceType.Unsupported:
                    Debug.Log("Unrecognized device \"" + name + "\"");
                    break;
            }

            inputPrinter = new InputPrinter(this);
        }

        public bool GetButton(GamepadControl b)
        {
            return deviceMap.GetControl(b).GetButton();
        }
        public bool GetButtonDown(GamepadControl b)
        {
            return deviceMap.GetControl(b).GetButtonDown();
        }
        public bool GetButtonUp(GamepadControl b)
        {
            return deviceMap.GetControl(b).GetButtonUp();
        }
        public float GetAxis(GamepadControl b)
        {
            return deviceMap.GetControl(b).GetAxis();
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

        public void PrintInput(bool axes, bool buttons)
        {
            if (axes)
                inputPrinter.PrintAxes();
            if (buttons)
                inputPrinter.PrintButtons();
        }

        private DeviceType GetDeviceType(string s)
        {
            s = s.ToLower();
            switch (s)
            {
                case "keyboard":
                    return DeviceType.Keyboard;
                case "controller (xbox one for windows)":
                    return DeviceType.Xbox;
                default:
                    return DeviceType.PlayStation;
            }
        }













    }
}
