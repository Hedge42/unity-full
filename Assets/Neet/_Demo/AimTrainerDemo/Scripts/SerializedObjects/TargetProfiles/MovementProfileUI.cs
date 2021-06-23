using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class MovementProfileUI : MonoBehaviour, ISettingUI<MovementProfile>
{
    public Toggle canMove;
    public TMP_InputField maxSpeed;
    public TMP_InputField accelRate;
    public Toggle useAccuracyRate;
    public TMP_InputField accuracyRate;
    public TMP_InputField accelPow;
    public TMP_InputField decelPow;

    private GameObject speedWarning;
    private GameObject accelWarning;
    private GameObject accuracyWarning;
    private GameObject accPowWarning;
    private GameObject decPowWarning;

    public void Apply(ref MovementProfile profile)
    {
        profile.canMove = canMove.isOn;
        profile.maxSpeed = float.Parse(maxSpeed.text);
        profile.accelRate = float.Parse(accelRate.text);
        profile.useAccuracyRate = useAccuracyRate.isOn;
        profile.accuracyRate = float.Parse(accuracyRate.text);
        profile.accelPow = float.Parse(accelPow.text);
        profile.decelPow = float.Parse(decelPow.text);
    }

    public void CreateWarnings(GameObject warningPrefab, Transform container)
    {
        speedWarning = UIHelpers.CreateWarning(warningPrefab, maxSpeed.gameObject,
            container, "Max speed must be at least " + MovementProfile.SPEED_MIN);
        accelWarning = UIHelpers.CreateWarning(warningPrefab, accelRate.gameObject,
            container, "Accelleration must be at least 0");
        accuracyWarning = UIHelpers.CreateWarning(warningPrefab, accuracyRate.gameObject,
            container, "Accuracy rate must be in range ["
            + MovementProfile.ACCURACY_MIN + "," + MovementProfile.ACCURACY_MAX + "]");

        accPowWarning = UIHelpers.CreateWarning(warningPrefab, accelPow.gameObject,
            container, "Acceleration power must be at least 0");
        decPowWarning = UIHelpers.CreateWarning(warningPrefab, decelPow.gameObject,
            container, "Deceleration power must be at least 0");

    }

    public void LoadFields(MovementProfile profile)
    {
        canMove.isOn = profile.canMove;
        maxSpeed.text = profile.maxSpeed.ToString();
        accelRate.text = profile.accelRate.ToString();
        useAccuracyRate.isOn = profile.useAccuracyRate;
        accuracyRate.text = profile.accuracyRate.ToString();
        accelPow.text = profile.accelPow.ToString();
        decelPow.text = profile.decelPow.ToString();
    }

    public void SetUIValidation(UnityAction endAction = null)
    {
        UIHelpers.SetInputValidation(maxSpeed, speedWarning,
            MovementProfile.SPEED_MIN, float.MaxValue, endAction);
        UIHelpers.SetInputValidation(accelRate, accelWarning,
            MovementProfile.ACCEL_MIN, float.MaxValue, endAction);
        UIHelpers.SetInputValidation(accuracyRate, accuracyWarning,
            MovementProfile.ACCURACY_MIN, MovementProfile.ACCURACY_MAX, endAction);
        UIHelpers.SetInputValidation(accelPow, accPowWarning,
            MovementProfile.POW_MIN, float.MaxValue, endAction);
        UIHelpers.SetInputValidation(decelPow, decPowWarning,
            MovementProfile.POW_MIN, float.MaxValue, endAction);

        accelPow.interactable = false;
        decelPow.interactable = false;

        canMove.onValueChanged.AddListener(delegate(bool value)
        {
            // disable other movement fields
            maxSpeed.interactable = value;
            accelRate.interactable = value;
            useAccuracyRate.interactable = value;
            // accuracyRate.interactable = value;
            // accelPow.interactable = value;
            // decelPow.interactable = value;

            if (!value)
                // calls other onValueChaned event
                useAccuracyRate.isOn = false;

            endAction.Invoke();
        });

        useAccuracyRate.onValueChanged.AddListener(delegate(bool value)
        {
            accuracyRate.interactable = value;
            endAction.Invoke();
        });
    }
}
