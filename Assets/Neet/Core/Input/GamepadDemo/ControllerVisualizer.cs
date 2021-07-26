using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neet.Input.Exp
{
    public class ControllerVisualizer : MonoBehaviour
    {
        public Image faceTop;
        public Image faceRight;
        public Image faceBottom;
        public Image faceLeft;
        public Image dpadTop;
        public Image dpadRight;
        public Image dpadBottom;
        public Image dpadLeft;
        public Image stickLeft;
        public Image stickRight;
        public Image bumperLeft;
        public Image bumperRight;
        public Image triggerLeft;
        public Image triggerRight;
        public Image start;
        public Image menu;

        public bool test;

        public Color activeColor;
        private Color defaultColor;

        private Vector2 leftStickStartPos;
        private Vector2 rightStickStartPos;

        private DeviceListener dl;

        private void Awake()
        {
            dl = GetComponent<DeviceListener>();

            leftStickStartPos = stickLeft.GetComponent<RectTransform>().localPosition;
            rightStickStartPos = stickRight.GetComponent<RectTransform>().localPosition;
        }
        private void Start()
        {
            defaultColor = faceTop.color;
        }

        private void Update()
        {
            if (dl == null)
                return;

            // buttons
            faceTop.color = dl.GetButton(GamepadControl.FaceTop) ? activeColor : defaultColor;
            faceRight.color = dl.GetButton(GamepadControl.FaceRight) ? activeColor : defaultColor;
            faceBottom.color = dl.GetButton(GamepadControl.FaceBottom) ? activeColor : defaultColor;
            faceLeft.color = dl.GetButton(GamepadControl.FaceLeft) ? activeColor : defaultColor;
            dpadTop.color = dl.GetButton(GamepadControl.DpadUp) ? activeColor : defaultColor;
            dpadRight.color = dl.GetButton(GamepadControl.DpadRight) ? activeColor : defaultColor;
            dpadBottom.color = dl.GetButton(GamepadControl.DpadDown) ? activeColor : defaultColor;
            dpadLeft.color = dl.GetButton(GamepadControl.DpadLeft) ? activeColor : defaultColor;
            bumperLeft.color = dl.GetButton(GamepadControl.BumperLeft) ? activeColor : defaultColor;
            bumperRight.color = dl.GetButton(GamepadControl.BumperRight) ? activeColor : defaultColor;
            start.color = dl.GetButton(GamepadControl.Start) ? activeColor : defaultColor;
            menu.color = dl.GetButton(GamepadControl.Menu) ? activeColor : defaultColor;
            triggerLeft.color = dl.GetButton(GamepadControl.TriggerLeft) ? activeColor : defaultColor;
            triggerRight.color = dl.GetButton(GamepadControl.TriggerRight) ? activeColor : defaultColor;
            stickLeft.color = dl.GetButton(GamepadControl.StickLeftClick) ? activeColor : defaultColor;
            stickRight.color = dl.GetButton(GamepadControl.StickRightClick) ? activeColor : defaultColor;

            float leftStickX = dl.GetAxis(GamepadControl.StickLeftX);
            float leftStickY = dl.GetAxis(GamepadControl.StickLeftY);
            float rightStickX = dl.GetAxis(GamepadControl.StickRightX);
            float rightStickY = dl.GetAxis(GamepadControl.StickRightY);
            Vector2 left2D = new Vector2(leftStickX, leftStickY);
            Vector2 right2D = new Vector2(rightStickX, rightStickY);

            float sensitivity = 10f;
            left2D *= sensitivity;
            right2D *= sensitivity;
            stickLeft.GetComponent<RectTransform>().localPosition = leftStickStartPos + left2D;
            stickRight.GetComponent<RectTransform>().localPosition = rightStickStartPos + right2D;
        }
    }
}
