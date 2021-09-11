using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.InputManagement.DeviceMaps
{
    public class KeyboardMap : IDeviceMap
    {
        private Dictionary<GamepadControl, IInputControl> map;

        public KeyboardMap()
        {
            map = GetDefault(0);
        }

        public IInputControl GetControl(GamepadControl c)
        {
            try
            {
                return map[c];
            }
            catch
            {
                Debug.LogError(c.ToString() + " no map");
                return null;
            }
        }

        public Dictionary<GamepadControl, IInputControl> GetDefault(int index)
        {
            Dictionary<GamepadControl, IInputControl> d = new Dictionary<GamepadControl, IInputControl>();

            d.Add(GamepadControl.StickLeftX, new KeyControl(
                UnityEngine.KeyCode.RightArrow, UnityEngine.KeyCode.LeftArrow));
            d.Add(GamepadControl.StickLeftY, new KeyControl(
                UnityEngine.KeyCode.UpArrow, UnityEngine.KeyCode.DownArrow));
            d.Add(GamepadControl.StickRightX, new AxisControl("Mouse X"));
            d.Add(GamepadControl.StickRightY, new AxisControl("Mouse Y"));
            d.Add(GamepadControl.FaceBottom, new KeyControl(UnityEngine.KeyCode.K));
            d.Add(GamepadControl.FaceLeft, new KeyControl(UnityEngine.KeyCode.J));
            d.Add(GamepadControl.FaceRight, new KeyControl(UnityEngine.KeyCode.L));
            d.Add(GamepadControl.FaceTop, new KeyControl(UnityEngine.KeyCode.I));

            
            // WASD -> D-Pad
            d.Add(GamepadControl.DpadX, new KeyControl(
                UnityEngine.KeyCode.D, UnityEngine.KeyCode.A));
            d.Add(GamepadControl.DpadY, new KeyControl(
                UnityEngine.KeyCode.W, UnityEngine.KeyCode.S));
            d.Add(GamepadControl.DpadLeft, new KeyControl(UnityEngine.KeyCode.A));
            d.Add(GamepadControl.DpadRight, new KeyControl(UnityEngine.KeyCode.D));
            d.Add(GamepadControl.DpadUp, new KeyControl(UnityEngine.KeyCode.W));
            d.Add(GamepadControl.DpadDown, new KeyControl(UnityEngine.KeyCode.S));

            d.Add(GamepadControl.BumperRight, new KeyControl());
            d.Add(GamepadControl.BumperLeft, new KeyControl());
            d.Add(GamepadControl.TriggerRight, new KeyControl());
            d.Add(GamepadControl.TriggerLeft, new KeyControl());
            d.Add(GamepadControl.StickRightClick, new KeyControl());
            d.Add(GamepadControl.StickLeftClick, new KeyControl());
            d.Add(GamepadControl.Start, new KeyControl(UnityEngine.KeyCode.Return));
            d.Add(GamepadControl.Menu, new KeyControl(UnityEngine.KeyCode.Escape));

            return d;
        }
    }
}
