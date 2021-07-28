﻿using System.Collections.Generic;
using UnityEngine;

namespace Neet.Input.DeviceMaps
{
    public class XboxOneMap : IDeviceMap
    {
        private Dictionary<GamepadControl, IInputControl> map;

        public XboxOneMap(int index)
        {
            map = GetDefault(index);
        }

        // https://answers.unity.com/questions/1350081/xbox-one-controller-mapping-solved.html
        public Dictionary<GamepadControl, IInputControl> GetDefault(int index)
        {
            Dictionary<GamepadControl, IInputControl> d = new Dictionary<GamepadControl, IInputControl>();

            // TODO: get prefix from somewhere else
            // prefix to match axis names in ProjectSettings -> Input
            string prefix = InputManager.JOYSTICK_PREFIX + index + InputManager.AXIS_PREFIX;

            AxisControl dpadX = new AxisControl(prefix + 5);
            AxisControl dpadY = new AxisControl(prefix + 6);

            d.Add(GamepadControl.StickLeftX, new AxisControl(prefix + 0));
            //d.Add(GamepadControl.StickLeftXLeft, new AxisControl(prefix + 0, true, false, true));
            //d.Add(GamepadControl.StickLeftXRight, new AxisControl(prefix + 0, false, false, true));
            d.Add(GamepadControl.StickLeftY, new AxisControl(prefix + 1, true));
            d.Add(GamepadControl.StickRightX, new AxisControl(prefix + 3));
            d.Add(GamepadControl.StickRightY, new AxisControl(prefix + 4, true));
            d.Add(GamepadControl.DpadX, dpadX);
            d.Add(GamepadControl.DpadY, dpadY);
            d.Add(GamepadControl.TriggerLeft, new AxisControl(prefix + 8));
            d.Add(GamepadControl.TriggerRight, new AxisControl(prefix + 9));

            d.Add(GamepadControl.DpadDown, dpadY.GetNegative());
            d.Add(GamepadControl.DpadLeft, dpadX.GetNegative());
            d.Add(GamepadControl.DpadRight, dpadX.GetPositive());
            d.Add(GamepadControl.DpadUp, dpadY.GetPositive());

            prefix = "Joystick" + index + "Button";
            d.Add(GamepadControl.FaceBottom, new KeyControl(ParseKeyCode(prefix + 0)));
            d.Add(GamepadControl.FaceRight, new KeyControl(ParseKeyCode(prefix + 1)));
            d.Add(GamepadControl.FaceLeft, new KeyControl(ParseKeyCode(prefix + 2)));
            d.Add(GamepadControl.FaceTop, new KeyControl(ParseKeyCode(prefix + 3)));
            d.Add(GamepadControl.BumperLeft, new KeyControl(ParseKeyCode(prefix + 4)));
            d.Add(GamepadControl.BumperRight, new KeyControl(ParseKeyCode(prefix + 5)));
            d.Add(GamepadControl.Menu, new KeyControl(ParseKeyCode(prefix + 6)));
            d.Add(GamepadControl.Start, new KeyControl(ParseKeyCode(prefix + 7)));
            d.Add(GamepadControl.StickLeftClick, new KeyControl(ParseKeyCode(prefix + 8)));
            d.Add(GamepadControl.StickRightClick, new KeyControl(ParseKeyCode(prefix + 9)));

            return d;
        }

        public IInputControl GetControl(GamepadControl c)
        {
            if (!map.ContainsKey(c))
            {
                Debug.Log("Map doesn't contain " + c.ToString());
                return null;
            }

            return map[c];
        }

        // TODO put this somewhere else or don't use it
        private static KeyCode ParseKeyCode(string s)
        {
            return (KeyCode)System.Enum.Parse(typeof(KeyCode), s);
        }
    }
}
