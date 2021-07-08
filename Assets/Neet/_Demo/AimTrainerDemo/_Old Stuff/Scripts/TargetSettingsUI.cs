using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TargetSettingsUI : MonoBehaviour
{
    public FOVSpawner spawner;

    public TargetScoreboard scoreboard;

    public TMP_InputField challengeTargetLimit;
    public TMP_InputField sphereSize;
    public TMP_InputField delayTime;
    public TMP_InputField timeoutTime;
    public TMP_InputField xAngleMin;
    public TMP_InputField xAngleMax;
    public TMP_InputField yAngleMin;
    public TMP_InputField yAngleMax;
    public Toggle canTimeout;
    public Toggle resetToCenter;
    public GameObject centerLines;

    private void Awake()
    {
        ReadSettings();
        if (xAngleMin != null)
            xAngleMin.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (xAngleMax != null)
            xAngleMax.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (yAngleMin != null)
            yAngleMin.onEndEdit.AddListener(delegate { WriteSettings(); });
        if (yAngleMax != null)
            yAngleMax.onEndEdit.AddListener(delegate { WriteSettings(); });
    }


    public void WriteSettings()
    {
        if (spawner != null)
        {

            if (canTimeout != null)
                spawner.canTimeout = canTimeout.isOn;

            if (resetToCenter != null)
                spawner.resetToCenter = resetToCenter.isOn;

            if (delayTime != null &&
                float.TryParse(delayTime.text, out float dt))
                spawner.spawnDelay = dt;

            if (timeoutTime != null &&
                float.TryParse(timeoutTime.text, out float t))
                spawner.timeoutTime = t;

            if (xAngleMin != null &&
                float.TryParse(xAngleMin.text, out float minH))
                spawner.minHorizontalAngle = minH;

            if (xAngleMax != null &&
                float.TryParse(xAngleMax.text, out float maxH))
                spawner.maxHorizontalAngle = maxH;

            if (yAngleMin != null &&
                float.TryParse(yAngleMin.text, out float minV))
                spawner.minVerticalAngle = minV;

            if (yAngleMax != null &&
                float.TryParse(yAngleMax.text, out float maxV))
                spawner.maxVerticalAngle = maxV;

            if (sphereSize != null &&
                float.TryParse(sphereSize.text, out float s))
                spawner.targetRadius = s;

            spawner.Init();
        }
        if (scoreboard != null)
        {
            if (challengeTargetLimit != null &&
                int.TryParse(challengeTargetLimit.text, out int t))
                scoreboard.targetLimit = t;

            scoreboard.Init();
        }

        ReadSettings();
    }
    void ReadSettings()
    {
        if (spawner != null)
        {
            canTimeout.isOn = spawner.canTimeout;
            resetToCenter.isOn = spawner.resetToCenter;
            timeoutTime.text = spawner.timeoutTime.ToString();
            delayTime.text = spawner.spawnDelay.ToString();
            xAngleMin.text = spawner.minHorizontalAngle.ToString();
            xAngleMax.text = spawner.maxHorizontalAngle.ToString();
            yAngleMin.text = spawner.minVerticalAngle.ToString();
            yAngleMax.text = spawner.maxVerticalAngle.ToString();
            sphereSize.text = spawner.targetRadius.ToString();
        }
        if (scoreboard != null)
        {
            challengeTargetLimit.text = scoreboard.targetLimit.ToString();

        }
    }
}
