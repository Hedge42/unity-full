using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class TrackingProfileUI : MonoBehaviour, ISettingUI<TrackingProfile>
{
    public TMP_InputField accelMin;
    public TMP_InputField accelMax;
    public TMP_InputField speedMin;
    public TMP_InputField speedMax;
    public TMP_InputField tickRate;
    public TMP_InputField timeToDestroy;
    public TMP_InputField trackTimeout;

    public Toggle canMove;
    public Toggle isMoveInstant;

    public Toggle canTrack;
    public Toggle isTrackInstant;

    public Toggle canTrackTimeout;
    public Toggle canTrackDestroy;

    private GameObject accelLimitWarning;
    private GameObject accelMinMaxWarning;
    private GameObject speedLimitWarning;
    private GameObject speedMinMaxWarning;
    private GameObject turnTickRateWarning;
    private GameObject timeLimitWarning;
    private GameObject timeMinMaxWarning;

    public void Apply(ref TrackingProfile profile)
    {
        profile.accelMin = float.Parse(accelMin.text);
        profile.accelMax = float.Parse(accelMax.text);
        profile.speedMin = float.Parse(speedMin.text);
        profile.speedMax = float.Parse(speedMax.text);
        profile.tickRate = float.Parse(tickRate.text);
        profile.timeToDestroy = float.Parse(timeToDestroy.text);
        profile.trackTimeout = float.Parse(trackTimeout.text);

        profile.canMove = canMove.isOn;
        profile.isMoveInstant = isMoveInstant.isOn;

        profile.canTrack = canTrack.isOn;
        profile.isTrackInstant = isTrackInstant.isOn;

        profile.canTrackTimeout = canTrackTimeout.isOn;
        profile.canTrackDestroy = canTrackDestroy.isOn;
    }

    public void CreateWarnings(GameObject warningPrefab, Transform container)
    {
        accelLimitWarning = UIHelpers.CreateWarning(warningPrefab,
            accelMin.gameObject, container,
            "Accelleration fields must be in range ["
            + TrackingProfile.ACCEL_MIN.ToString() + ","
            + TrackingProfile.ACCEL_MAX.ToString() + " ]");

        accelMinMaxWarning = UIHelpers.CreateWarning(warningPrefab,
            accelMin.gameObject, container,
            "Min cannot exceed max");

        speedLimitWarning = UIHelpers.CreateWarning(warningPrefab,
            speedMin.gameObject, container,
            "Speed fields must be in range ["
            + TrackingProfile.SPEED_MIN.ToString() + ","
            + TrackingProfile.SPEED_MAX.ToString() + " ]");

        speedMinMaxWarning = UIHelpers.CreateWarning(warningPrefab,
            speedMin.gameObject, container,
            "Min cannot exceed max");

        turnTickRateWarning = UIHelpers.CreateWarning(warningPrefab,
            tickRate.gameObject, container,
            "Tick rate must be in range ["
            + TrackingProfile.TICK_RATE_MIN.ToString() + ","
            + TrackingProfile.TICK_RATE_MAX.ToString() + "]");

        timeLimitWarning = UIHelpers.CreateWarning(warningPrefab,
            timeToDestroy.gameObject, container,
            "Tracking time fields must be in range ["
            + TrackingProfile.TIME_MIN.ToString() + ","
            + TrackingProfile.TIME_MAX.ToString() + " ]");

        timeMinMaxWarning = UIHelpers.CreateWarning(warningPrefab,
            trackTimeout.gameObject, container,
            "Destroy-time cannot exceed timeout-time");
    }

    public void LoadFields(TrackingProfile profile)
    {
        accelMin.text = profile.accelMin.ToString();
        accelMax.text = profile.accelMax.ToString();
        speedMin.text = profile.speedMin.ToString();
        speedMax.text = profile.speedMax.ToString();
        tickRate.text = profile.tickRate.ToString();
        timeToDestroy.text = profile.timeToDestroy.ToString();
        trackTimeout.text = profile.trackTimeout.ToString();

        canTrack.isOn = profile.canTrack;
        isTrackInstant.isOn = profile.isTrackInstant;
        canMove.isOn = profile.canMove;
        isMoveInstant.isOn = profile.isMoveInstant;

        canTrackTimeout.isOn = profile.canTrackTimeout;
        canTrackDestroy.isOn = profile.canTrackDestroy;
    }

    public void SetUIValidation(UnityAction endAction = null)
    {
        UIHelpers.SetInputMinMaxValidation(accelMin, accelMax,
            accelLimitWarning, accelMinMaxWarning,
            TrackingProfile.ACCEL_MIN, TrackingProfile.ACCEL_MAX, endAction);

        UIHelpers.SetInputMinMaxValidation(speedMin, speedMax,
            speedLimitWarning, speedMinMaxWarning,
            TrackingProfile.SPEED_MIN, TrackingProfile.SPEED_MAX, endAction);

        UIHelpers.SetInputMinMaxValidation(timeToDestroy, trackTimeout,
            timeLimitWarning, timeMinMaxWarning,
            TrackingProfile.TIME_MIN, TrackingProfile.TIME_MAX, endAction);

        UIHelpers.SetInputValidation(tickRate, turnTickRateWarning,
            TrackingProfile.TICK_RATE_MIN, TrackingProfile.TICK_RATE_MAX, endAction);

        UnityAction<bool> canTrackOnChange = delegate (bool isOn)
        {
            // called through canMoveOnChange, don't be redundant
            if (!isOn)
            {
                canTrackTimeout.isOn = false;
                canTrackDestroy.isOn = false;
                isTrackInstant.isOn = false;
            }

            isTrackInstant.interactable = isOn;
            canTrackTimeout.interactable = isOn;
            canTrackDestroy.interactable = isOn;
            trackTimeout.interactable = isOn;
            timeToDestroy.interactable = isOn;
            canTrackDestroy.interactable = isOn;

            endAction.Invoke();
        };
        canTrack.onValueChanged.AddListener(canTrackOnChange);

        UnityAction<bool> canMoveOnChange = delegate (bool isOn)
        {
            if (!isOn)
            {
                // can't move, can't track
                canTrack.isOn = false; // track settings fixed here
                isMoveInstant.isOn = false;
            }

            isMoveInstant.interactable = isOn;
            canTrack.interactable = isOn;
            accelMin.interactable = isOn;
            accelMax.interactable = isOn;
            speedMin.interactable = isOn;
            speedMax.interactable = isOn;
            tickRate.interactable = isOn;

            endAction.Invoke();
        };
        canMove.onValueChanged.AddListener(canMoveOnChange);

        UnityAction<bool> isMoveImmedaiteChange = delegate (bool isOn)
        {
            if (!isOn)
            {
                isTrackInstant.isOn = false;
            }

            isTrackInstant.interactable = isOn;

            endAction.Invoke();
        };
        isMoveInstant.onValueChanged.AddListener(isMoveImmedaiteChange);

        UnityAction<bool> canTrackTimeoutChange = delegate (bool isOn)
        {
            trackTimeout.interactable = isOn;

            endAction.Invoke();
        };
        canTrackTimeout.onValueChanged.AddListener(canTrackTimeoutChange);

        UnityAction<bool> canTrackDestroyChange = delegate (bool isOn)
        {
            if (!isOn)
            {
                timeToDestroy.interactable = isOn;
            }

            endAction.Invoke();
        };
        canTrackDestroy.onValueChanged.AddListener(canTrackDestroyChange);
    }
}
