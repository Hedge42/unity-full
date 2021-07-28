using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.Extensions;

public class WeaponHandler : MonoBehaviour
{
    public const string PRIMARY_FIRE_KEY = "primary fire";
    public const string ALTERNATE_FIRE_KEY = "alternate fire";
    public const string RELOAD_KEY = "reload";
    public const string ACTIVE_RAYCAST_KEY = "active raycast";
    public const string PASSIVE_RAYCAST_KEY = "passive raycast";

    public KeyCode reload;

    public Gun gun;
    private GunUI gui;

    public Transform bulletStart;

    public List<KeyCode> primaryFireKeys;
    public List<KeyCode> alternateFireKeys;
    public List<KeyCode> reloadKeys;

    // public event UnityAction<List<KeyCode>> onPrimaryFireDown;
    // public event UnityAction<List<KeyCode>> onPrimaryFireHold;

    private void Awake()
    {
        gui = GetComponentInChildren<GunUI>();

        this.SetUserListener(PRIMARY_FIRE_KEY, PrimaryFireHandler);
        this.SetUserListener(ALTERNATE_FIRE_KEY, AlternateFireHandler);
        this.SetUserListener(RELOAD_KEY, ReloadHandler);
        this.SetUserListener(ACTIVE_RAYCAST_KEY, ActiveRaycastHandler);
        this.SetUserListener(PASSIVE_RAYCAST_KEY, PassiveRaycastHandler);
    }
    private void Start()
    {
        SwapTo(gun);
    }
    void Update()
    {
        HandleInput();
    }

    public void SwapTo(Gun g)
    {
        gun = g;
        gun.weaponHandler = this;
    }

    private void PrimaryFireHandler(object k)
    {
        if (k.Cast(out KeyCode[] kc))
            gun?.StartPrimaryFire(kc);
    }
    private void AlternateFireHandler(object k)
    {
        if (k.Cast(out KeyCode[] kc))
            gun?.StartAlternateFire(kc);
    }
    private void ReloadHandler(object k)
    {
        if (k.Cast(out KeyCode[] kc))
            gun?.StartReload(kc);
    }
    private void ActiveRaycastHandler(object k)
    {
        if (k.Cast(out KeyCode[] kc))
            gun?.StartActiveRaycast(kc);
    }
    private void PassiveRaycastHandler()
    {
        gun?.PassiveRaycast();
    }

    private void UpdateUI(RaycastHit rh)
    {
        gui.UpdateUI(gun);
    }

    void HandleInput()
    {
        if (primaryFireKeys.GetKeyDown())
        {
            this.InvokeUserEvent(PRIMARY_FIRE_KEY, primaryFireKeys.ToArray());
            this.InvokeUserEvent(ACTIVE_RAYCAST_KEY, primaryFireKeys.ToArray());
        }
        if (alternateFireKeys.GetKeyDown())
            this.InvokeUserEvent(ALTERNATE_FIRE_KEY, alternateFireKeys.ToArray());

        if (reload.GetKeyDown())
            this.InvokeUserEvent(RELOAD_KEY, reloadKeys.ToArray());

        this.InvokeUserEvent(PASSIVE_RAYCAST_KEY);
    }
}
