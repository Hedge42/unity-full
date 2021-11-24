using UnityEngine;
using System;
using System.Collections.Generic;

namespace Neat.InputManagement
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        public const string JOYSTICK_PREFIX = "joystick ";
        public const string AXIS_PREFIX = " analog ";

        /// <summary> Invokes when a new device is plugged in and passes the device. </summary>
        public event Action<Device> onDeviceNewConnection;
        /// <summary> Invokes when previously disconnected device is reconnected and passes the device. </summary>
        public event Action<Device> onDeviceReconnected;
        /// <summary> Invokes when a device is disconnected and passes the device. </summary>
        public event Action<Device> onDeviceDisconnected;
        /// <summary> Generic, void call that invokes whenever there is a change to the detected devices. </summary>
        public event Action onDeviceChanged;

        public string[] possibleAxes { get; private set; }

        // Devices are automatically populated when the game is started
        // and whenever a connection changes
        public Device[] devices { get; private set; }
        public MasterDevice masterDevice { get; private set; }

        public bool debugConnections;
        public bool debugAxes;
        public bool debugButtons;

        private string[] storedGamepadNames;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            if (debugConnections)
                PrintConnections();

            SetPossibleAxes();

            storedGamepadNames = new string[0];
        }
        private void Start()
        {
            DetectDeviceChange();

            if (devices == null)
                SyncDevices();
        }
        private void Update()
        {
            DetectDeviceChange();

            foreach (Device d in devices)
                d?.PrintInput(debugAxes, debugButtons);
        }

        /// <summary> Prints default (string) names of connected controllers </summary>
        private void PrintConnections()
        {
            string[] detectedGamepads = UnityEngine.Input.GetJoystickNames();
            string toPrint = "Gamepads: ";
            for (int i = 0; i < detectedGamepads.Length; i++)
                toPrint += detectedGamepads[i] + " (" + i + "), ";
            print(toPrint);
        }
        /// <summary> Match what exists in ProjectSettings -> Input </summary>
        private void SetPossibleAxes()
        {
            List<string> axes = new List<string>();
            for (int joystick = 1; joystick <= 10; joystick++)
                for (int analog = 0; analog < 20; analog++)
                    axes.Add(JOYSTICK_PREFIX + joystick + AXIS_PREFIX + analog);
            possibleAxes = axes.ToArray();
        }
        /// <summary> On device connection, syncs THEN triggers event so that the (new) device can be passed.
        /// On device disconnection, triggers event but does NOT sync. </summary>
        private void DetectDeviceChange()
        {
            string[] currentGamepadNames = UnityEngine.Input.GetJoystickNames();

            // new device connected
            if (currentGamepadNames.Length != storedGamepadNames.Length)
            {
                SyncDevices();
                onDeviceNewConnection?.Invoke(devices[devices.Length - 1]);
                onDeviceChanged?.Invoke();
            }

            else
                for (int i = 0; i < currentGamepadNames.Length; i++)
                    if (currentGamepadNames[i] != storedGamepadNames[i])
                    {

                        // device disconnected
                        if (currentGamepadNames[i] == "")
                        {
                            // +1 to offset for keyboard
                            onDeviceDisconnected?.Invoke(devices[i + 1]);
                            onDeviceChanged?.Invoke();
                        }

                        // device reconnected
                        else
                        {
                            // +1 to offset for keyboard
                            SyncDevices();
                            onDeviceReconnected?.Invoke(devices[i + 1]);
                            onDeviceChanged?.Invoke();
                        }
                    }
        }
        /// <summary> Creates device array equal to JoystickNames() + 1 (to compensate for keyboard) </summary>
        private void SyncDevices()
        {
            storedGamepadNames = UnityEngine.Input.GetJoystickNames();
            Device[] newDeviceArr = new Device[storedGamepadNames.Length + 1];

            // create keyboard device if devices have not previously been synced
            if (devices == null)
                newDeviceArr[0] = new Device("keyboard", 0);
            else
                newDeviceArr[0] = devices[0];

            for (int i = 1; i < newDeviceArr.Length; i++)
            {
                // copy devices from old array only if...
                // ...devices have been previously synced AND
                // ...'i' will not cause an OutOfRangeExeption AND
                // ...the device at 'i' is valid
                if (devices != null && i < devices.Length && devices[i] != null && devices[i].deviceType != Device.DeviceType.Unsupported)
                    newDeviceArr[i] = devices[i];
                // else create a new device based on gamepadNames
                else
                    newDeviceArr[i] = new Device(storedGamepadNames[i - 1], i);
            }

            devices = newDeviceArr;

            if (debugConnections)
                for (int i = 0; i < newDeviceArr.Length; i++)
                    Debug.Log("Devices[" + i + "] sync " + (devices[i].deviceType != Device.DeviceType.Unsupported ? "SUCCESS" : "FAIL") + " (" + newDeviceArr[i].name + ")");

            masterDevice = new MasterDevice(devices);
        }
    }
}
