using System.Collections;
using UnityEngine;

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