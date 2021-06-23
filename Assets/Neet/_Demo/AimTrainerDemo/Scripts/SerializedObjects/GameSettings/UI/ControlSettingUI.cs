using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ControlSettingUI : MonoBehaviour, ISettingUI<ControlSetting>
{
    public TMP_InputField dpi;
    public TMP_InputField inches;

    private GameObject dpiWarning;
    private GameObject inchesWarning;

    public void Apply(ref ControlSetting profile)
    {
        profile.dpi = int.Parse(dpi.text);
        profile.distance = float.Parse(inches.text);
    }

    public void CreateWarnings(GameObject prefab, Transform container)
    {
        dpiWarning = UIHelpers.CreateWarning(prefab, dpi.gameObject, container,
            "DPI must be in range [" + ControlSetting.DPI_MIN
            + "," + ControlSetting.DPI_MAX + "]"); ;

        inchesWarning = UIHelpers.CreateWarning(prefab, inches.gameObject, 
            container, "360 Distance must be in range [" 
            + ControlSetting.DPI_MIN + "," + ControlSetting.DIST_MAX + "]");
    }

    public void LoadFields(ControlSetting profile)
    {
        dpi.text = profile.dpi.ToString();
        inches.text = profile.distance.ToString();
    }

    public void SetUIValidation(UnityAction endAction = null)
    {
        UIHelpers.SetInputValidation(dpi, dpiWarning, ControlSetting.DPI_MIN,
            ControlSetting.DPI_MAX, endAction);
        UIHelpers.SetInputValidation(inches, inchesWarning, ControlSetting.DIST_MIN,
            ControlSetting.DIST_MAX, endAction);
    }
}
