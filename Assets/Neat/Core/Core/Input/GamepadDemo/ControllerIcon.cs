using UnityEngine;
using UnityEngine.UI;

namespace Neat.InputManagement
{
    public class ControllerIcon : MonoBehaviour
    {
        public Device device { get; private set; }
        public bool isToggled { get; private set; }

        public void SetDevice(Device d)
        {
            device = d;
        }

        public Vector2 GetMovement()
        {
            if (device == null)
                return Vector2.zero;

            Vector2 vector = Vector2.zero;

            // both analog sticks
            vector += new Vector2(device.GetAxis(GamepadControl.StickLeftX), device.GetAxis(GamepadControl.StickLeftY));
            vector += new Vector2(device.GetAxis(GamepadControl.StickRightX), device.GetAxis(GamepadControl.StickRightY));

            return vector;
        }
        public bool GetConfirm()
        {
            if (device == null)
                return false;
            else
                return device.GetButtonDown(GamepadControl.FaceBottom);
        }
        public bool GetCancel()
        {
            if (device == null)
                return false;
            else
                return device.GetButtonDown(GamepadControl.FaceRight);
        }

        public void ToggleIcon(bool state)
        {
            isToggled = state;

            try
            {
                GetComponent<Image>().enabled = isToggled;
            }
            catch
            {
                Debug.LogWarning("No image to toggle!");
            }

            if (!isToggled)
                gameObject.transform.localPosition = Vector2.zero;
        }
        public void ToggleIcon()
        {
            ToggleIcon(!isToggled);
        }
    }
}
