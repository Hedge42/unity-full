using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Neat.InputManagement
{
    public class MenuDropdown : MonoBehaviour
    {
        public MenuItem itemPrefab;
        public MenuDropdownItem[] dropdownItems;

        private bool isActive;
        private MenuItem[] activeItems;

        private void Awake()
        {
            GetComponent<MenuItem>().OnSelect.AddListener(ToggleItems);
        }

        private void ToggleItems()
        {
            if (!isActive)
            {
                activeItems = new MenuItem[dropdownItems.Length];
                int i = 0;

                foreach (MenuDropdownItem item in dropdownItems)
                {
                    MenuItem m = Instantiate(itemPrefab, transform);
                    m.GetComponentInChildren<TextMeshProUGUI>().text = item.text;
                    m.OnSelect.AddListener(item.action.Invoke);
                    activeItems[i++] = m;
                }

                isActive = true;
            }
            else
            {
                foreach (MenuItem item in activeItems)
                    Destroy(item.gameObject);

                isActive = false;
                activeItems = new MenuItem[0];
            }
        }
    }
}
