using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.GameManager;
using Neat.Tools.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Neat.InputHelpers;

public class MouseRotator : MonoBehaviour
{
    public bool active;
    public bool debugMouse;

    public Transform xTarget;
    public Transform yTarget;

    public SensitivitySetting sensitivitySetting;


    private Quaternion xStart;
    private Quaternion yStart;

    public Keybinds _keybinds;
    public Keybinds keybinds => _keybinds ??= GetComponent<KeybindsComponent>().keybinds;
    public InputAction Look => keybinds.FPS.Look;

    public Vector3 current { get; private set; }


    private void OnEnable()
    {
        keybinds.Enable();
        _keybinds = new Keybinds();
        _keybinds.Enable();
        xStart = xTarget.localRotation;
        yStart = yTarget.localRotation;
        sensitivitySetting.inchesPer360 = ControlSetting.Load().distance;
        //keybinds.FPS.Look.performed += HandleMouse;
    }

    public void Toggle(bool value)
    {
        active = value;
        if (value)
        {
            CursorLocker.instance.SetLockMode(CursorLockMode.Locked);
            StartCoroutine(UpdateRotation());
        }
        else
        {
            CursorLocker.instance.SetLockMode(CursorLockMode.None);
        }
    }
    public void ResetRotation()
    {
        xTarget.localRotation = xStart;
        yTarget.localRotation = yStart;
    }

    private IEnumerator UpdateRotation()
    {
        // Get current rotation
        current = transform.rotation.eulerAngles;
        Vector2 currentRot = new Vector2(current.y, -current.x);

        while (active)
        {
            var look = Look.ReadValue<Vector2>();
            if (debugMouse)
                print($"look: {look}");

            if (sensitivitySetting.convertSensitivity)
                AdjustSensitivity(look);

            var scaledLook = look * sensitivitySetting.lookSensitivity;

            currentRot = DontFlip(currentRot + scaledLook);
            ApplyRotation(currentRot);
            yield return null;
        }
    }

    private static Vector2 DontFlip(Vector2 currentRot)
    {
        // Prevent the up-down rotation from flipping upside-down
        if (currentRot.y > 90) currentRot.y = 90;
        else if (currentRot.y < -90) currentRot.y = -90;

        // Prevent the left-right rotation from breaking floating-point limits
        if (currentRot.x > 360) currentRot.x -= 360;
        else if (currentRot.x < -360) currentRot.x += 360;
        return currentRot;
    }

    private void ApplyRotation(Vector2 rot)
    {
        if (xTarget != null)
            xTarget.localRotation = Quaternion.Euler(new Vector3(-rot.y, xTarget.localRotation.eulerAngles.y));
        if (yTarget != null)
            yTarget.localRotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, rot.x));
    }

    private void AdjustSensitivity(Vector2 mouseDelta)
    {
        // please leave me alone

        // normalize mouse move with DPI
        float deltaDistX = Mathf.Abs(mouseDelta.x / sensitivitySetting.mouseDPI);

        // inches per 360...
        float unitsPer360 = sensitivitySetting.convertFromCM ? sensitivitySetting.inchesPer360 / 2.52f : sensitivitySetting.inchesPer360;

        // num360s = inches / inches per 360
        float moveRatio = deltaDistX / unitsPer360;

        // degrees intended
        float desiredDegrees = moveRatio * 360;

        // degrees per mouse unit
        float multiplier = desiredDegrees / mouseDelta.x;

        if (!float.IsNaN(multiplier))
        {
            var value = Mathf.Abs(multiplier);
            sensitivitySetting.lookSensitivity = value;
            // print($"Calculated sensitivity: {value}");
        }
    }
}
