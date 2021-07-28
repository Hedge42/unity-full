using Neet.Input;
using System.Collections;
using UnityEngine;

namespace Neet.Input
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;

        [Range(1, 4)] public int maxControllers;

        private PlayerController[] controllers;
        public Player[] players;

        // used in device scanning
        // private bool isWaitingForInput;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        private void Start()
        {
            // in start because devices aren't synced until after awake()
            InputManager.instance.onDeviceNewConnection += DeviceNewConnectionHandler;
            InputManager.instance.onDeviceReconnected += DeviceReconnectedHandler;
            InputManager.instance.onDeviceDisconnected += DeviceDisconnectedHandler;
            CreatePlayerControllers();
        }

        /// <summary> Adds a PlayerController component for each of InputManager's device </summary>
        private void CreatePlayerControllers()
        {
            // create a PlayerController for each device
            controllers = new PlayerController[InputManager.instance.devices.Length];
            for (int i = 0; i < controllers.Length; i++)
            {
                controllers[i] = gameObject.AddComponent<PlayerController>();
                controllers[i].SetDevice(InputManager.instance.devices[i]);
            }
        }

        // Handle controller changes
        private void DeviceNewConnectionHandler(Device d)
        {
            PlayerController[] newArr = new PlayerController[controllers.Length + 1];
            for (int i = 0; i < controllers.Length; i++)
                newArr[i] = controllers[i];

            newArr[newArr.Length - 1] = gameObject.AddComponent<PlayerController>();
            newArr[newArr.Length - 1].SetDevice(d);
        }
        private void DeviceReconnectedHandler(Device d)
        {
            PlayerController controller = GetAssociatedPlayerController(d);

            controller.SetDevice(d);
        }
        private void DeviceDisconnectedHandler(Device d)
        {
            PlayerController controller = GetAssociatedPlayerController(d);
        }


        public PlayerController RequestNextAvailableController()
        {
            if (controllers == null)
            {
                Debug.LogWarning("Controllers not yet set up");
                return null;
            }

            foreach (PlayerController pc in controllers)
            {
                if (!pc.device.isInUse)
                    return pc;
            }

            Debug.LogWarning("No PlayerController available");
            return null;
        }
        /// <summary> Finds the player controller with a matching device </summary>
        private PlayerController GetAssociatedPlayerController(Device d)
        {
            foreach (PlayerController p in controllers)
                if (p.device == d)
                    return p;

            // device doesn't have an associated controller...
            int index = -1;
            for (int i = 0; i < InputManager.instance.devices.Length; i++)
                if (d == InputManager.instance.devices[i])
                    index = i;

            if (index >= 0)
                return controllers[index];

            Debug.LogWarning("Something went very, very wrong.");
            return null;
        }


        // For player indexing WIP
        /// <summary> First player is Player 0 </summary>
        public PlayerController GetControllerAtIndex(int index)
        {
            if (index < 0 || index >= maxControllers)
                throw new System.IndexOutOfRangeException();

            return controllers[index];
        }
        /// <summary> First player is Player 1 </summary>
        public PlayerController GetControllerAtAdjustedIndex(int index)
        {
            return GetControllerAtIndex(index - 1);
        }


        // Not doing anything WIP
        /// <summary> Assigns Devices to PlayerControllers when input is detected </summary>
        public void ScanForDevices()
        {
            StartCoroutine(DeviceScanner());
        }
        private IEnumerator DeviceScanner()
        {
            // isWaitingForInput = true;
            DeviceFinder finder = new DeviceFinder();
            foreach (PlayerController pc in controllers)
            {
                pc.SetDevice(finder.GetNextDevice());
                while (pc.device == null)
                    yield return null;

                // prevents multiple controllers from being assigned on the same frame
                yield return new WaitForEndOfFrame();
            }
            // isWaitingForInput = false;
        }
    }
}
