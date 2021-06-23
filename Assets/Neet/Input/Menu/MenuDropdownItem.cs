using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Neet.Input
{
    [System.Serializable]
    public class MenuDropdownItem
    {
        public string text;
        public UnityEvent action;
    }
}
