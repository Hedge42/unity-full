using UnityEngine;
using UnityEngine.UI;

namespace Neet.Input
{
    public class NewPlayerManager : MonoBehaviour
    {
        public GameObject[] iconPrefabs;
        public GameObject container;
        public TMPro.TextMeshProUGUI numControllersText;

        private ControllerIcon[] icons;

        /* 
         * 0 - keyboard
         * 1 - xbox
         * 2 - playstation
         */

        private void Start()
        {
            InputManager.instance.onDeviceChanged += SetIcons;
            InputManager.instance.onDeviceChanged += UpdateText;

            SetIcons();
            UpdateText();
        }
        private void Update()
        {
            HandleInput();
        }

        private void SetIcons()
        {
            if (icons != null)
                foreach (ControllerIcon c in icons)
                    Destroy(c.gameObject);

            icons = new ControllerIcon[InputManager.instance.devices.Length];
            for (int i = 0; i < icons.Length; i++)
            {
                int prefabIndex = GetPrefabIndex(InputManager.instance.devices[i].deviceType);
                icons[i] = SpawnPrefab(prefabIndex)?.GetComponent<ControllerIcon>();

                icons[i]?.ToggleIcon(false);
                icons[i]?.SetDevice(InputManager.instance.devices[i]);
            }
        }
        private void UpdateText()
        {
            if (numControllersText == null)
                numControllersText = GetComponent<TMPro.TextMeshProUGUI>();

            int count = 0;
            foreach (ControllerIcon c in icons)
                if (c != null && !c.isToggled)
                    count++;

            if (count == 0)
            {
                // remove background image...
                container.GetComponent<Image>().enabled = false;
                numControllersText.text = "";
            }
            else if (count == 1)
            {
                // show icon on last image...

                // toggle the only one that isn't shown
                foreach (ControllerIcon c in icons)
                    if (!c.isToggled)
                        c.ToggleIcon(true);

                // then hide the background
                container.GetComponent<Image>().enabled = false;
                numControllersText.text = "";
            }
            else
                numControllersText.text = count.ToString();
        }

        private void HandleInput()
        {
            if (icons != null)
                foreach (ControllerIcon c in icons)
                {
                    if (c == null || c.device == null)
                        continue;

                    if (!c.isToggled && c.device.AnyAxis())
                    {
                        c.ToggleIcon(true);
                        UpdateText();
                    }

                    c.transform.Translate(c.GetMovement());

                    if (c.GetConfirm())
                        Debug.Log(c.name + " confirmed.");
                    if (c.GetCancel())
                        Debug.Log(c.name + " cancelled.");
                }
        }

        private int GetPrefabIndex(Device.DeviceType type)
        {
            switch (type)
            {
                case Device.DeviceType.Keyboard:
                    return 0;
                case Device.DeviceType.Xbox:
                    return 1;
                case Device.DeviceType.PlayStation:
                    return 2;
                default:
                    return -1;
            }
        }

        public GameObject SpawnPrefab(int index)
        {
            // if there is an indexing error, just return null
            try
            {
                return Instantiate(iconPrefabs[index], container.transform);
            }
            catch
            {
                return null;
            }
        }
    }
}
