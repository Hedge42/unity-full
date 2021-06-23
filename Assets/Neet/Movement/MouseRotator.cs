using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.File;
using Neet.UI;
using UnityEngine.Events;

public class MouseRotator : MonoBehaviour
{
    // Valorant sens: .44
    // 14.61039 inches/360

    /*
     * The gameObject attached to this script will have its rotation tied to mouse movement.
     * TODO: Look sensitivity should be set using PlayerPrefs (along with pretty much all settings)
     */

    // To show condensed options in editor
    [System.Serializable]
    public class SensitivitySetting
    {
        [Range(.01f, 10f)]
        public float lookSensitivity = 1;

        // this bool determines if the next 3 are used
        [Header("Distance settings")]
        public bool convertSensitivity;
        public float mouseDPI;
        public float inchesPer360;
        public bool convertFromCM;
    }

    public bool active;
    public bool debugMouseMove;

    public Transform xRotTarget;
    public Transform yRotTarget;

    public SensitivitySetting sens;

    private Quaternion xRotStart;
    private Quaternion yRotStart;

    private void Awake()
    {
        if (xRotTarget == null)
            xRotTarget = transform;
        if (yRotTarget == null)
            yRotTarget = transform;

        xRotStart = xRotTarget.localRotation;
        yRotStart = yRotTarget.localRotation;
    }

    private void Start()
    {
        ReadSens();
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
        xRotTarget.localRotation = xRotStart;
        yRotTarget.localRotation = yRotStart;
    }
    private IEnumerator UpdateRotation()
    {
        // Get current rotation
        Vector3 current = transform.rotation.eulerAngles;

        float xRot = current.y;
        float yRot = -current.x;

        // no going upsidedown
        if (current.x >= 269)
            yRot = (360f - current.x);
        if (current.y > 180)
            xRot = -(360 - current.y);

        while (active)
        {
            float xRotRaw = Input.GetAxis("Mouse X");
            float yRotRaw = Input.GetAxis("Mouse Y");

            if (debugMouseMove)
                print("Delta mouse (Unity): (" + xRotRaw.ToString("f2")
                    + ", " + yRotRaw.ToString("f2") + ")");

            if (sens.convertSensitivity)
            {
                // from Rotator360.cs (not mine)
                float deltaDistX = Mathf.Abs(xRotRaw * 20f) / sens.mouseDPI;

                // handle metric conversion
                float unitsPer360 = sens.convertFromCM ? sens.inchesPer360 / 2.52f : sens.inchesPer360;

                float moveRatio = deltaDistX / unitsPer360;
                float desiredDegrees = moveRatio * 360;
                float multiplier = desiredDegrees / xRotRaw;

                if (!float.IsNaN(multiplier))
                    sens.lookSensitivity = Mathf.Abs(multiplier);

                if (debugMouseMove)
                {
                    print("Delta mouse (in/cm): " + deltaDistX);
                    print("Converted sensitivity: " + sens.lookSensitivity);
                }
            }

            xRot += xRotRaw * sens.lookSensitivity;
            yRot += yRotRaw * sens.lookSensitivity;

            // Prevent the up-down rotation from flipping upside-down
            if (yRot > 90) yRot = 90;
            else if (yRot < -90) yRot = -90;

            // Prevent the left-right rotation from breaking floating-point limits
            if (xRot > 360) xRot -= 360;
            else if (xRot < -360) xRot += 360;

            // doing the rotations
            if (xRotTarget != null)
                xRotTarget.localRotation = Quaternion.Euler(new Vector3(-yRot, xRotTarget.localRotation.eulerAngles.y));
            if (yRotTarget != null)
                yRotTarget.localRotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, xRot));
            yield return null;
        }
    }


    public void ReadSens()
    {
        // sens.inchesPer360 = SettingsManager.instance.setting.cmPer360;
        sens.inchesPer360 = ControlSetting.Load().distance;
    }
}
