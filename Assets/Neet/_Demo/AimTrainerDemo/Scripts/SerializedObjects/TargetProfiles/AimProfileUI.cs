using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class AimProfileUI : MonoBehaviour, ISettingUI<AimProfile>
{
    public TMP_InputField radius;
    public Toggle showCenterLines;
    public TMP_InputField xMin;
    public TMP_InputField xMax;
    public TMP_InputField yMin;
    public TMP_InputField yMax;

    public TMP_InputField spawnRotate;
    public Toggle canSpawnRotate;

    private GameObject radiusWarning;
    private GameObject xLimitWarning;
    private GameObject xMinMaxWarning;
    private GameObject yLimitWarning;
    private GameObject yMinMaxWarning;
    private GameObject spawnRotateWarning;

    public void LoadFields(AimProfile a)
    {
        if (radius != null)
            radius.text = a.radius.ToString();
        if (showCenterLines != null)
            showCenterLines.isOn = a.showCenterLines;
        if (xMin != null)
            xMin.text = a.xMin.ToString();
        if (xMax != null)
            xMax.text = a.xMax.ToString();
        if (yMin != null)
            yMin.text = a.yMin.ToString();
        if (yMax != null)
            yMax.text = a.yMax.ToString();


        canSpawnRotate.isOn = a.canSpawnRotate;
        spawnRotate.text = a.spawnRotate.ToString();
    }

    public void CreateWarnings(GameObject warningPrefab, Transform container)
    {
        if (radius != null)
            radiusWarning = UIHelpers.CreateWarning(warningPrefab,
                radius.gameObject, container, "Radius must be in range ["
                + AimProfile.RADIUS_MIN.ToString() + ","
                + AimProfile.RADIUS_MAX.ToString());

        if (xMin != null || xMax != null)
            xLimitWarning = UIHelpers.CreateWarning(warningPrefab,
                xMin.gameObject, container.transform,
                "X fields must be in range ["
                + AimProfile.X_MIN.ToString() + ","
                + AimProfile.X_MAX.ToString());

        if (xMin != null && xMax != null)
            xMinMaxWarning = UIHelpers.CreateWarning(warningPrefab,
                xMin.gameObject, container.transform,
                "Min cannot exceed max");

        if (yMin != null || yMax != null)
            yLimitWarning = UIHelpers.CreateWarning(warningPrefab,
                yMin.gameObject, container.transform,
                "Y fields must be in range ["
                + AimProfile.Y_MIN.ToString() + ","
                + AimProfile.Y_MAX.ToString());

        if (yMin != null && yMax != null)
            yMinMaxWarning = UIHelpers.CreateWarning(warningPrefab,
                yMin.gameObject, container.transform,
                "Min cannot exceed max");

        spawnRotateWarning = UIHelpers.CreateWarning(warningPrefab,
            spawnRotate.gameObject, container, "Spawn rotation must be " +
            "in range [" + AimProfile.SPAWN_ROTATE_MIN + ","
            + AimProfile.SPAWN_ROTATE_MAX + "]");
    }

    public void SetUIValidation(UnityAction endAction = null)
    {
        if (radius != null)
            UIHelpers.SetInputValidation(radius, radiusWarning,
                AimProfile.RADIUS_MIN, AimProfile.RADIUS_MAX, endAction);

        if (xMin != null && xMax != null)
            UIHelpers.SetInputMinMaxValidation(xMin, xMax,
                xLimitWarning, xMinMaxWarning,
                AimProfile.X_MIN, AimProfile.X_MAX, endAction);

        if (yMin != null && yMax != null)
            UIHelpers.SetInputMinMaxValidation(yMin, yMax,
                yLimitWarning, yMinMaxWarning,
                AimProfile.Y_MIN, AimProfile.Y_MAX, endAction);

        UIHelpers.SetInputValidation(spawnRotate, spawnRotateWarning,
            AimProfile.SPAWN_ROTATE_MIN, AimProfile.SPAWN_ROTATE_MAX, endAction);

        canSpawnRotate.onValueChanged.AddListener(delegate
        {
            endAction.Invoke();
        });

        showCenterLines.onValueChanged.AddListener(delegate
        {
            endAction.Invoke();
        });
    }

    public void Apply(ref AimProfile profile)
    {
        profile.radius = float.Parse(radius.text);
        profile.showCenterLines = showCenterLines.isOn;
        profile.xMax = float.Parse(xMax.text);
        profile.xMin = float.Parse(xMin.text);
        profile.yMax = float.Parse(yMax.text);
        profile.yMin = float.Parse(yMin.text);

        profile.canSpawnRotate = canSpawnRotate.isOn;
        profile.spawnRotate = float.Parse(spawnRotate.text);
    }
}
