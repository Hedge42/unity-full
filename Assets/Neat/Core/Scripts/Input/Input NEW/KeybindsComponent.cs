using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Neat.InputHelpers
{
    
    public class KeybindsComponent : MonoBehaviour
    {
        private Keybinds _keybinds;
        public Keybinds keybinds => _keybinds ??= new Keybinds();

        private void OnEnable()
        {
            keybinds.Enable();
        }
        private void OnDisable()
        {
            keybinds.Disable();
        }
    }
}
