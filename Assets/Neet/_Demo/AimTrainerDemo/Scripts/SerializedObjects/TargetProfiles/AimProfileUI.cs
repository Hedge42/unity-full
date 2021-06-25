using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class AimProfileUI : MonoBehaviour, ISettingUI<AimProfile>
{
    // public TMP_InputField radius;
    public Toggle showCenterLines;
    public TMP_InputField xMin;
    public TMP_InputField xMax;
    public TMP_InputField yMin;
    public TMP_InputField yMax;


    public TMP_InputField spawnRotate;
    public Toggle canSpawnRotate;
    public Toggle failTargetOnMissClick;

    public Toggle useDistRange;
    public TMP_InputField distMin;
    public TMP_InputField distMax;

    // private GameObject radiusWarning;
    private GameObject xLimitWarning;
    private GameObject xMinMaxWarning;
    private GameObject yLimitWarning;
    private GameObject yMinMaxWarning;
    private GameObject spawnRotateWarning;

    private GameObject distLimitWarning;
    private GameObject distMinMaxWarning;

    public void LoadFields(AimProfile a)
    {
        // radius.text = a.radius.ToString();
        showCenterLines.isOn = a.showCenterLines;
        xMin.text = a.xMin.ToString();
        xMax.text = a.xMax.ToString();
        yMin.text = a.yMin.ToString();
        yMax.text = a.yMax.ToString();

        useDistRange.isOn = a.useDistRange;
        distMin.text = a.distMin.ToString();
        distMax.text = a.distMax.ToString();

        failTargetOnMissClick.isOn = a.failTargetOnMissClick;
        canSpawnRotate.isOn = a.canSpawnRotate;
        spawnRotate.text = a.spawnRotate.ToString();

        useDistRange.onValueChanged.Invoke(useDistRange.isOn);
    }

    public void CreateWarnings(GameObject warningPrefab, Transform container)
    {
        //radiusWarning = UIHelpers.CreateWarning(warningPrefab,
        //    radius.gameObject, container, "Radius must be in range ["
        //    + AimProfile.RADIUS_MIN.ToString() + ","
        //    + AimProfile.RADIUS_MAX.ToString());

        xLimitWarning = UIHelpers.CreateWarning(warningPrefab,
            xMin.gameObject, container.transform,
            "X fields must be in range ["
            + AimProfile.X_MIN.ToString() + ","
            + AimProfile.X_MAX.ToString());

        xMinMaxWarning = UIHelpers.CreateWarning(warningPrefab,
            xMin.gameObject, container.transform,
            "Min cannot exceed max");

        yLimitWarning = UIHelpers.CreateWarning(warningPrefab,
            yMin.gameObject, container.transform,
            "Y fields must be in range ["
            + AimProfile.Y_MIN.ToString() + ","
            + AimProfile.Y_MAX.ToString());

        yMinMaxWarning = UIHelpers.CreateWarning(warningPrefab,
            yMin.gameObject, container.transform,
            "Min cannot exceed max");

        distLimitWarning = UIHelpers.CreateWarning(warningPrefab,
            distMin.gameObject, container.transform,
            "Distance fields must be in range ["
            + AimProfile.DIST_MIN + "," + AimProfile.DIST_MAX + "]");

        distMinMaxWarning = UIHelpers.CreateWarning(warningPrefab,
            distMin.gameObject, container.transform,
            "Min distance cannot exceed max distance");


        spawnRotateWarning = UIHelpers.CreateWarning(warningPrefab,
            spawnRotate.gameObject, container, "Spawn rotation must be " +
            "in range [" + AimProfile.SPAWN_ROTATE_MIN + ","
            + AimProfile.SPAWN_ROTATE_MAX + "]");
    }

    public void SetUIValidation(UnityAction endAction = null)
    {
        //UIHelpers.SetInputValidation(radius, radiusWarning,
        //    AimProfile.RADIUS_MIN, AimProfile.RADIUS_MAX, endAction);

        UIHelpers.SetInputMinMaxValidation(xMin, xMax,
            xLimitWarning, xMinMaxWarning,
            AimProfile.X_MIN, AimProfile.X_MAX, endAction);

        UIHelpers.SetInputMinMaxValidation(yMin, yMax,
            yLimitWarning, yMinMaxWarning,
            AimProfile.Y_MIN, AimProfile.Y_MAX, endAction);

        UIHelpers.SetInputValidation(spawnRotate, spawnRotateWarning,
            AimProfile.SPAWN_ROTATE_MIN, AimProfile.SPAWN_ROTATE_MAX, endAction);

        UIHelpers.SetInputMinMaxValidation(distMin, distMax,
            distLimitWarning, distMinMaxWarning,
            AimProfile.DIST_MIN, AimProfile.DIST_MAX, endAction);

        canSpawnRotate.onValueChanged.AddListener(delegate
        {
            endAction.Invoke();
        });

        showCenterLines.onValueChanged.AddListener(delegate
        {
            endAction.Invoke();
        });

        failTargetOnMissClick.onValueChanged.AddListener(delegate
        {
            endAction.Invoke();
        });

        useDistRange.onValueChanged.AddListener(delegate(bool isOn)
        {
            distMax.interactable = isOn;

            if (!isOn)
                distMax.text = distMax.text;

            endAction.Invoke();
        });


        // automatically change distMax if not interactable
        UnityAction<string> fixMax = delegate
        {
            if (!useDistRange.isOn)
                distMax.text = distMin.text;
        };

        distMin.onValueChanged.AddListener(fixMax);
        distMin.onEndEdit.AddListener(fixMax);
        distMin.onSubmit.AddListener(fixMax);
        distMin.onDeselect.AddListener(fixMax);
    }

    public void Apply(ref AimProfile profile)
    {
        // profile.radius = float.Parse(radius.text);
        profile.showCenterLines = showCenterLines.isOn;
        profile.xMax = float.Parse(xMax.text);
        profile.xMin = float.Parse(xMin.text);
        profile.yMax = float.Parse(yMax.text);
        profile.yMin = float.Parse(yMin.text);

        profile.useDistRange = useDistRange.isOn;
        profile.distMin = float.Parse(distMin.text);
        profile.distMax = float.Parse(distMax.text);

        profile.failTargetOnMissClick = failTargetOnMissClick.isOn;
        profile.canSpawnRotate = canSpawnRotate.isOn;
        profile.spawnRotate = float.Parse(spawnRotate.text);
    }
}
