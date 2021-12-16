using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Tools;
using Neat.InputHelpers;
using UnityEngine.InputSystem;

public class WeaponHandler : MonoBehaviour
{
    public Gun gun;
    private GunUI gui;

    private Keybinds _keybinds;
    public Keybinds keybinds => _keybinds ??= GetComponent<KeybindsComponent>().keybinds;

    private void Awake()
    {
        gui = GetComponentInChildren<GunUI>();
        print("here?");

        keybinds.FPS.PrimaryFire.performed += PrimaryFire_performed;
        keybinds.FPS.AltFire.performed += AltFire_performed;
        keybinds.FPS.Reload.performed += Reload_performed;
    }
    private void Reload_performed(InputAction.CallbackContext context)
    {
        gun.StartReload(context);
    }

    private void AltFire_performed(InputAction.CallbackContext context)
    {
        gun.StartAlternateFire(context);
    }
        
    private void PrimaryFire_performed(InputAction.CallbackContext context)
    {
        gun.StartPrimaryFire(context);
    }

    private void OnEnable()
    {
        SwapTo(gun);
    }


    public void SwapTo(Gun g)
    {
        gun = g;
        gun.weaponHandler = this;
    }
}
