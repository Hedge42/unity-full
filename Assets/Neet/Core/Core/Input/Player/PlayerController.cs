using UnityEngine;

namespace Neet.Input
{
    public class PlayerController : MonoBehaviour
    {
        public string deviceName;
        public int playerIndex { get; private set; }
        public Device device { get; private set; }

        private ActionMap map;

        private void Start()
        {
            map = new ActionMap();

            InputManager.instance.onDeviceNewConnection += SetDevice;
        }

        public void SetPlayerIndex(int index)
        {
            playerIndex = index;
        }
        public void SetDevice(Device d)
        {
            device = d;
            deviceName = device.name;
        }

        public bool GetAction(GameAction g)
        {
            return GetButton(map.Get(g));
        }
        public bool GetActionDown(GameAction g)
        {
            return GetButtonDown(map.Get(g));
        }
        public bool GetActionUp(GameAction g)
        {
            return GetButtonUp(map.Get(g));
        }
        public float GetAxis(GameAction g)
        {
            return GetAxis(map.Get(g));
        }

        public bool GetButton(GamepadControl b)
        {
            if (device == null)
                return false;

            return device.GetButton(b);
        }
        public bool GetButtonDown(GamepadControl b)
        {
            if (device == null)
                return false;

            return device.GetButtonDown(b);
        }
        public bool GetButtonUp(GamepadControl b)
        {
            if (device == null)
                return false;

            return device.GetButtonUp(b);
        }
        public float GetAxis(GamepadControl b)
        {
            if (device == null)
                return 0;

            return device.GetAxis(b);
        }
    }
}
