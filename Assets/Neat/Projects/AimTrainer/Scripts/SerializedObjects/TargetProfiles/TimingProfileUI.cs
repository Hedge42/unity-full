using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TimingProfileUI : MonoBehaviour, ISettingUI<TimingProfile>
{
    public TMP_InputField timeout;
    public TMP_InputField delayMin;
    public TMP_InputField delayMax;
    public Toggle canDelay;
    public Toggle canTimeout;

    private GameObject timeoutWarning;
    private GameObject delayLimitWarning;
    private GameObject delayMinMaxWarning;

    private string timeoutText;
    private string delayText;

    public void LoadFields(TimingProfile t)
    {
        if (timeout != null)
            timeout.text = t.timeout.ToString();
        if (delayMin != null)
            delayMin.text = t.delayMin.ToString();
        if (delayMax != null)
            delayMax.text = t.delayMax.ToString();
        if (canTimeout != null)
            canTimeout.isOn = t.canClickTimeout;
        if (canDelay != null)
            canDelay.isOn = t.canDelay;
    }

    public void CreateWarnings(GameObject warningPrefab, Transform container)
    {
        if (timeout != null)
            timeoutWarning = UIHelpers.CreateWarning(warningPrefab,
                timeout.gameObject, container, "Timeout must be in range ["
                + TimingProfile.TIMEOUT_MIN.ToString() + ","
                + TimingProfile.TIMEOUT_MAX.ToString());

        if (delayMin != null || delayMax != null)
            delayLimitWarning = UIHelpers.CreateWarning(warningPrefab,
                delayMin.gameObject, container.transform,
                "Delay must be in range ["
                + TimingProfile.DELAY_MIN.ToString() + ","
                + TimingProfile.DELAY_MAX.ToString());

        if (delayMin != null && delayMax != null)
            delayMinMaxWarning = UIHelpers.CreateWarning(warningPrefab,
                delayMin.gameObject, container.transform,
                "Min cannot exceed max");
    }

    public void SetUIValidation(UnityAction endAction = null)
    {
        if (timeout != null)
            UIHelpers.SetInputValidation(timeout, timeoutWarning,
                TimingProfile.TIMEOUT_MIN, TimingProfile.TIMEOUT_MAX, endAction);

        if (delayMin != null && delayMax != null)
            UIHelpers.SetInputMinMaxValidation(delayMin, delayMax,
                delayLimitWarning, delayMinMaxWarning,
                TimingProfile.DELAY_MIN, TimingProfile.DELAY_MAX, endAction);

        UnityAction<bool> canDelayChange = delegate (bool isOn)
        {
            endAction.Invoke();
        };
        canDelay.onValueChanged.AddListener(canDelayChange);

        UnityAction<bool> canClickTimeoutChange = delegate (bool isOn)
        {
            endAction.Invoke();
        };
        canTimeout.onValueChanged.AddListener(canClickTimeoutChange);
    }

    public void Apply(ref TimingProfile profile)
    {
        profile.canDelay = canDelay.isOn;
        profile.canClickTimeout = canTimeout.isOn;
        profile.delayMin = float.Parse(delayMin.text);
        profile.delayMax = float.Parse(delayMax.text);
        profile.timeout = float.Parse(timeout.text);
    }

    public void AddAllTooltips(Transform container, GameObject prefab)
    {
        SetContextTexts();

        AddTooltip(timeout.transform, container, timeoutText, prefab);
        AddTooltip(delayMax.transform, container, delayText, prefab);
    }

    public void AddTooltip(Transform obj, Transform container, string text, GameObject prefab)
    {
        while (obj.parent != container.transform)
            obj = obj.parent;
        Transform label = obj.GetChild(0);
        UIHelpers.AddTooltip(prefab, label, text);
    }

    public void SetContextTexts()
    {
        timeoutText = "If enabled, the target will wait for this amount of time, " +
            "in seconds, to destroy and fail the target.";

        delayText = "The range of time, in seconds, to wait to spawn a new target " +
            "after the last one has been destroyed.";
    }
}
