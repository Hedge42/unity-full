using System.Collections;
using UnityEngine;

namespace Neat.Input
{
    public class DeviceListener : MonoBehaviour
    {
        private ActionMap map;
        private Device device;

        public bool printAxes;
        public bool printButtons;
        private InputPrinter printer;

        private void Start()
        {
            map = new ActionMap();
            StartCoroutine(DeviceScanner());
        }
        private void Update()
        {
            if (printer != null)
            {
                if (printAxes)
                    printer.PrintAxes();
                if (printButtons)
                    printer.PrintButtons();
            }
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

        private IEnumerator DeviceScanner()
        {
            DeviceFinder d = new DeviceFinder();

            while (device == null)
            {
                device = d.GetNextDevice();
                yield return null;
            }

            printer = new InputPrinter(device);
        }
    }
}
