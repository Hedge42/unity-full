using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Neat.Tools;

public class ColorProfileUI : MonoBehaviour, ISettingUI<ColorProfile>
{
    public Image backgroundPreview;
    public TMP_InputField backgroundR;
    public TMP_InputField backgroundG;
    public TMP_InputField backgroundB;

    public Image targetPreview;
    public TMP_InputField targetR;
    public TMP_InputField targetG;
    public TMP_InputField targetB;

    public Image dummyPreview;
    public TMP_InputField dummyR;
    public TMP_InputField dummyG;
    public TMP_InputField dummyB;

    public Image centerPreview;
    public TMP_InputField centerR;
    public TMP_InputField centerG;
    public TMP_InputField centerB;

    public Image trackingPreview;
    public TMP_InputField trackingR;
    public TMP_InputField trackingG;
    public TMP_InputField trackingB;

    private GameObject backgroundWarning;
    private GameObject targetWarning;
    private GameObject dummyWarning;
    private GameObject centerWarning;
    private GameObject trackingWarning;

    private string backgroundText;
    private string targetText;
    private string centerText;
    private string trackingText;

    public void CreateWarnings(GameObject warningPrefab, Transform container)
    {
        if (backgroundR != null && backgroundG != null && backgroundB != null)
            backgroundWarning = UIHelpers.CreateWarning(warningPrefab,
                backgroundR.gameObject, container, "Color fields must be in range ["
                + ColorProfile.RGB_MIN.ToString() + ","
                + ColorProfile.RGB_MAX.ToString() + "]");

        if (targetR != null && targetG != null && targetB != null)
            targetWarning = UIHelpers.CreateWarning(warningPrefab,
                targetR.gameObject, container, "Color fields must be in range ["
                + ColorProfile.RGB_MIN.ToString() + ","
                + ColorProfile.RGB_MAX.ToString() + "]");

        if (centerR != null && centerG != null && centerB != null)
            centerWarning = UIHelpers.CreateWarning(warningPrefab,
                centerR.gameObject, container, "Color fields must be in range ["
                + ColorProfile.RGB_MIN.ToString() + ","
                + ColorProfile.RGB_MAX.ToString() + "]");

        if (trackingR != null && trackingG != null && trackingB != null)
            trackingWarning = UIHelpers.CreateWarning(warningPrefab,
                trackingR.gameObject, container, "Color fields must be in range ["
                + ColorProfile.RGB_MIN.ToString() + ","
                + ColorProfile.RGB_MAX.ToString() + "]");
    }

    public void Apply(ref ColorProfile c)
    {
        if (int.TryParse(backgroundR.text, out int _backgroundR))
            c.backgroundR = _backgroundR;
        if (int.TryParse(backgroundG.text, out int _backgroundG))
            c.backgroundG = _backgroundG;
        if (int.TryParse(backgroundB.text, out int _backgroundB))
            c.backgroundB = _backgroundB;

        if (int.TryParse(targetR.text, out int _targetR))
            c.targetR = _targetR;
        if (int.TryParse(targetG.text, out int _targetG))
            c.targetG = _targetG;
        if (int.TryParse(targetB.text, out int _targetB))
            c.targetB = _targetB;

        if (int.TryParse(centerR.text, out int _centerR))
            c.centerR = _centerR;
        if (int.TryParse(centerG.text, out int _centerG))
            c.centerG = _centerG;
        if (int.TryParse(centerB.text, out int _centerB))
            c.centerB = _centerB;

        if (int.TryParse(trackingR.text, out int _trackingR))
            c.trackingR = _trackingR;
        if (int.TryParse(trackingG.text, out int _trackingG))
            c.trackingG = _trackingG;
        if (int.TryParse(trackingB.text, out int _trackingB))
            c.trackingB = _trackingB;
    }
    public void LoadFields(ColorProfile c)
    {
        // background
        if (backgroundR != null)
        {
            backgroundR.text = c.backgroundR.ToString();
            backgroundR.SetData(Neat.Keys.LAST_VALID, c.backgroundR);
        }
        if (backgroundG != null)
        {
            backgroundG.text = c.backgroundG.ToString();
            backgroundG.SetData(Neat.Keys.LAST_VALID, c.backgroundG);
        }
        if (backgroundB != null)
        {
            backgroundB.text = c.backgroundB.ToString();
            backgroundB.SetData(Neat.Keys.LAST_VALID, c.backgroundB);
        }
        if (backgroundPreview != null)
            backgroundPreview.color = c.backgroundColor;

        // target
        if (targetR != null)
        {
            targetR.text = c.targetR.ToString();
            targetR.SetData(Neat.Keys.LAST_VALID, c.targetR);
        }
        if (targetG != null)
        {
            targetG.text = c.targetG.ToString();
            targetG.SetData(Neat.Keys.LAST_VALID, c.targetG);
        }
        if (targetB != null)
        {
            targetB.text = c.targetB.ToString();
            targetB.SetData(Neat.Keys.LAST_VALID, c.targetB);
        }
        if (targetPreview != null)
            targetPreview.color = c.targetColor;

        // center
        if (centerR != null)
        {
            centerR.text = c.centerR.ToString();
            centerR.SetData(Neat.Keys.LAST_VALID, c.centerR);
        }
        if (centerG != null)
        {
            centerG.text = c.centerG.ToString();
            centerG.SetData(Neat.Keys.LAST_VALID, c.centerG);
        }
        if (centerB != null)
        {
            centerB.text = c.centerB.ToString();
            centerB.SetData(Neat.Keys.LAST_VALID, c.centerB);
        }
        if (centerPreview != null)
            centerPreview.color = c.centerColor;

        // tracking
        if (trackingR != null)
        {
            trackingR.text = c.trackingR.ToString();
            trackingR.SetData(Neat.Keys.LAST_VALID, c.trackingR);
        }
        if (trackingG != null)
        {
            trackingG.text = c.trackingG.ToString();
            trackingG.SetData(Neat.Keys.LAST_VALID, c.trackingG);
        }
        if (trackingB != null)
        {
            trackingB.text = c.trackingB.ToString();
            trackingB.SetData(Neat.Keys.LAST_VALID, c.trackingB);
        }
        if (trackingPreview != null)
            trackingPreview.color = c.trackingColor;
    }

    public void SetUIValidation(UnityAction endAction = null)
    {
        UIHelpers.SetInputColorValidation(backgroundR, backgroundPreview, 
            backgroundR, backgroundG, backgroundB, backgroundWarning, endAction);
        UIHelpers.SetInputColorValidation(backgroundG, backgroundPreview,
            backgroundR, backgroundG, backgroundB, backgroundWarning, endAction);
        UIHelpers.SetInputColorValidation(backgroundB, backgroundPreview,
            backgroundR, backgroundG, backgroundB, backgroundWarning, endAction);

        UIHelpers.SetInputColorValidation(targetR, targetPreview,
            targetR, targetG, targetB, targetWarning, endAction);
        UIHelpers.SetInputColorValidation(targetG, targetPreview,
            targetR, targetG, targetB, targetWarning, endAction);
        UIHelpers.SetInputColorValidation(targetB, targetPreview,
            targetR, targetG, targetB, targetWarning, endAction);

        UIHelpers.SetInputColorValidation(centerR, centerPreview,
            centerR, centerG, centerB, centerWarning, endAction);
        UIHelpers.SetInputColorValidation(centerG, centerPreview,
            centerR, centerG, centerB, centerWarning, endAction);
        UIHelpers.SetInputColorValidation(centerB, centerPreview,
            centerR, centerG, centerB, centerWarning, endAction);

        UIHelpers.SetInputColorValidation(trackingR, trackingPreview,
            trackingR, trackingG, trackingB, trackingWarning, endAction);
        UIHelpers.SetInputColorValidation(trackingG, trackingPreview,
            trackingR, trackingG, trackingB, trackingWarning, endAction);
        UIHelpers.SetInputColorValidation(trackingB, trackingPreview,
            trackingR, trackingG, trackingB, trackingWarning, endAction);
    }

    public void AddAllTooltips(Transform container, GameObject prefab)
    {
        SetContextTexts();

        AddTooltip(backgroundR.transform, container, backgroundText, prefab);
        AddTooltip(targetR.transform, container, targetText, prefab);
        AddTooltip(centerR.transform, container, centerText, prefab);
        AddTooltip(trackingR.transform, container, trackingText, prefab);
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
        string ex = "The RGB color values \n(0-255, 0-255, 0-255)\n" +
            "of the ";

        backgroundText = ex + "background fill.";
        targetText = ex + "targets.";
        centerText = ex + "spawn zone lines.";
        trackingText = ex + "targets while they are being successfully tracked.";
    }
}
