using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ReTargetSettingsUI : MonoBehaviour
{
    public TargetScoreboard scoreboard;

    public TSpawner spawner;
    private TargetSetting setting;

    public TMP_InputField presetName;

    // challenge
    public TMP_InputField challengeTargetLimit;
    public TMP_InputField challengeTimeLimit;
    public TMP_InputField challengeLimit;
    public Button challengeType;

    // size
    public TMP_InputField radius;

    // timings
    public TMP_InputField delay;
    public TMP_InputField delayMin;
    public TMP_InputField delayMax;

    public TMP_InputField timeoutTime;

    // angles
    public TMP_InputField angleMin;
    public TMP_InputField angleMax;
    public TMP_InputField xAngleMin;
    public TMP_InputField xAngleMax;
    public TMP_InputField yAngleMin;
    public TMP_InputField yAngleMax;

    // movement
    public TMP_InputField accelMin;
    public TMP_InputField accelMax;
    public TMP_InputField speedMin;
    public TMP_InputField speedMax;
    public TMP_InputField secondsPerTurnMin;
    public TMP_InputField secondsPerTurnMax;

    // bools
    public Toggle canTimeout;
    public Toggle showCenterLines;
    public Toggle resetToCenter;
    public Toggle showTurnIndicator;

    public Button saveButton;
    public GameObject centerLines;

    private void OnValidate()
    {
        GetSetting();
    }
    private void Start()
    {
        GetSetting();
        ReadSettings();
        SetEvents();
    }

    private void GetSetting()
    {
        if (spawner != null)
            setting = spawner.GetSetting();
        else
            Debug.LogError("No spawner selected!");
    }

    private void SetEvents()
    {
        if (challengeType != null)
            challengeType.onClick.AddListener(delegate
            {
                ToggleChallengeLimitType();
                ReadSettings();
            });

        if (saveButton != null)
            saveButton.onClick.AddListener(SaveSettings);

        if (presetName != null)
            presetName.onEndEdit.AddListener(delegate { WriteSettings(); });

        // bool
        if (showCenterLines != null)
            showCenterLines.onValueChanged.AddListener(ToggleCenterLines);
        if (canTimeout != null)
            canTimeout.onValueChanged.AddListener(delegate { WriteSettings(); });
        if (resetToCenter != null)
            resetToCenter.onValueChanged.AddListener(delegate { WriteSettings(); });
        if (showTurnIndicator != null)
            showTurnIndicator.onValueChanged.AddListener(delegate { WriteSettings(); });

        // angles
        if (angleMin != null)
            angleMin.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (angleMax != null)
            angleMax.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (xAngleMin != null)
            xAngleMin.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (xAngleMax != null)
            xAngleMax.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (yAngleMin != null)
            yAngleMin.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (yAngleMax != null)
            yAngleMax.onEndEdit.AddListener(delegate { WriteSettings(); });

        // movement
        if (accelMin != null)
            accelMin.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (accelMax != null)
            accelMax.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (speedMin != null)
            speedMin.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (speedMax != null)
            speedMax.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (secondsPerTurnMin != null)
            secondsPerTurnMin.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (secondsPerTurnMax != null)
            secondsPerTurnMax.onEndEdit.AddListener(delegate { WriteSettings(); });

        if (timeoutTime != null)
            timeoutTime.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (delay != null)
            delay.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (radius != null)
            radius.onEndEdit.AddListener(delegate { WriteSettings(); });
    }
    private void ToggleCenterLines(bool value)
    {
        centerLines.SetActive(value);
    }
    private void ToggleChallengeLimitType()
    {
        if (scoreboard != null)
            scoreboard.isTimeLimit = !scoreboard.isTimeLimit;
        if (setting != null)
            setting.isTimeLimit = !setting.isTimeLimit;
    }
    private void WriteSettings()
    {
        // take data from text fields
        // and apply it to setting

        if (setting != null)
        {
            if (presetName != null)
                setting.name = presetName.text;

            // challenge
            if (challengeType != null)
                setting.isTimeLimit = challengeType
                    .GetComponentInChildren<TextMeshProUGUI>()
                    .text.Equals("Seconds");
            if (challengeLimit != null &&
                int.TryParse(challengeLimit.text, out int ct))
                setting.challengeLimit = ct;

            // boolean
            if (canTimeout != null)
                setting.canTimeout = canTimeout.isOn;
            if (resetToCenter != null)
                setting.resetToCenter = resetToCenter.isOn;
            if (showCenterLines != null)
                setting.showCenterLines = showCenterLines.isOn;
            if (showTurnIndicator != null)
                setting.showTurnIndicator = showTurnIndicator.isOn;

            // time
            if (delay != null &&
                float.TryParse(delay.text, out float dt))
                setting.delay = dt;
            if (delayMin != null &&
                float.TryParse(delayMin.text, out float dmin))
                setting.delayMin = dmin;
            if (delayMax != null &&
                float.TryParse(delayMax.text, out float dmax))
                setting.delayMax = dmax;
            if (timeoutTime != null &&
                float.TryParse(timeoutTime.text, out float t))
                setting.timeoutTime = t;

            // size
            if (radius != null &&
                float.TryParse(radius.text, out float s))
                setting.radius = s;

            // angles
            if (angleMin != null &&
                float.TryParse(angleMin.text, out float amin))
                setting.angleMin = amin;
            if (angleMax != null &&
                float.TryParse(angleMax.text, out float amax))
                setting.angleMax = amax;
            if (xAngleMin != null &&
                float.TryParse(xAngleMin.text, out float minH))
                setting.xAngleMin = minH;
            if (xAngleMax != null &&
                float.TryParse(xAngleMax.text, out float maxH))
                setting.xAngleMax = maxH;
            if (yAngleMin != null &&
                float.TryParse(yAngleMin.text, out float minV))
                setting.yAngleMin = minV;
            if (yAngleMax != null &&
                float.TryParse(yAngleMax.text, out float maxV))
                setting.yAngleMax = maxV;

            // movement
            if (accelMin != null &&
                float.TryParse(accelMin.text, out float acmin))
                setting.accelMin = acmin;
            if (accelMax != null &&
                float.TryParse(accelMax.text, out float acmax))
                setting.accelMax = acmax;
            if (speedMin != null &&
                float.TryParse(speedMin.text, out float smin))
                setting.speedMin = smin;
            if (speedMax != null &&
                float.TryParse(speedMax.text, out float smax))
                setting.speedMax = smax;
            if (secondsPerTurnMin != null &&
                float.TryParse(secondsPerTurnMin.text, out float sptmin))
                setting.secondsPerTurnMin = sptmin;
            if (secondsPerTurnMax != null &&
                float.TryParse(secondsPerTurnMax.text, out float sptmax))
                setting.secondsPerTurnMax = sptmax;

            spawner.Init();
            scoreboard.Init();
        }
        if (scoreboard != null)
        {
            // TODO the scoreboard should have access to the setting

            if (challengeTargetLimit != null &&
                int.TryParse(challengeTargetLimit.text, out int targetLimit))
                scoreboard.targetLimit = targetLimit;
            if (challengeTimeLimit != null &&
                int.TryParse(challengeTimeLimit.text, out int timeLimit))
                scoreboard.timeLimit = timeLimit;

            scoreboard.isTimeLimit = setting.isTimeLimit;
            scoreboard.timeLimit = setting.challengeLimit;
            scoreboard.targetLimit = setting.challengeLimit;

            scoreboard.Init();
        }

        ReadSettings();
    }
    private void ReadSettings()
    {
        // take data from settings
        // and apply it to text fields

        if (setting != null)
        {
            if (presetName != null)
                presetName.text = setting.name;
            if (challengeType != null)
                challengeType.GetComponentInChildren<TextMeshProUGUI>()
                    .text = setting.isTimeLimit ?
                    "Seconds" : "Targets";

            if (challengeLimit != null)
                challengeLimit.text = setting.challengeLimit.ToString();

            // bool
            if (canTimeout != null)
                canTimeout.isOn = setting.canTimeout;
            if (resetToCenter != null)
                resetToCenter.isOn = setting.resetToCenter;
            if (showTurnIndicator != null)
                showTurnIndicator.isOn = setting.showTurnIndicator;
            if (showCenterLines != null)
                showCenterLines.isOn = setting.showCenterLines;

            // timing
            if (timeoutTime != null)
                timeoutTime.text = setting.timeoutTime.ToString();
            if (delay != null)
                delay.text = setting.delay.ToString();
            if (delayMin != null)
                delayMin.text = setting.delayMin.ToString();
            if (delayMax != null)
                delayMax.text = setting.delayMax.ToString();

            // angles
            if (angleMin != null)
                angleMin.text = setting.angleMin.ToString();
            if (angleMax != null)
                angleMax.text = setting.angleMax.ToString();
            if (xAngleMin != null)
                xAngleMin.text = setting.xAngleMin.ToString();
            if (xAngleMax != null)
                xAngleMax.text = setting.xAngleMax.ToString();
            if (yAngleMin != null)
                yAngleMin.text = setting.yAngleMin.ToString();
            if (yAngleMax != null)
                yAngleMax.text = setting.yAngleMax.ToString();

            // size
            if (radius != null)
                radius.text = setting.radius.ToString();

            // movement
            if (accelMin != null)
                accelMin.text = setting.accelMin.ToString();
            if (accelMax != null)
                accelMax.text = setting.accelMax.ToString();
            if (speedMin != null)
                speedMin.text = setting.speedMin.ToString();
            if (speedMax != null)
                speedMax.text = setting.speedMax.ToString();
            if (secondsPerTurnMin != null)
                secondsPerTurnMin.text = setting.secondsPerTurnMin.ToString();
            if (secondsPerTurnMax != null)
                secondsPerTurnMax.text = setting.secondsPerTurnMax.ToString();
        }
        else
        {
            Debug.LogError("Setting was null");
        }

        if (scoreboard != null)
        {

            if (challengeTargetLimit != null)
                challengeTargetLimit.text = scoreboard.targetLimit.ToString();
            if (challengeTimeLimit != null)
                challengeTimeLimit.text = scoreboard.timeLimit.ToString();

            if (challengeLimit != null)
            {
                if (setting.isTimeLimit)
                    challengeLimit.text = scoreboard.timeLimit.ToString();
                else
                    challengeLimit.text = scoreboard.targetLimit.ToString();
            }
        }
    }

    private void SaveSettings()
    {
        if (setting != null)
            setting.SaveJson();
    }
    public void LoadSetting(TargetSetting s)
    {
        setting = s;
        spawner.ApplySetting(s);
    }
}
