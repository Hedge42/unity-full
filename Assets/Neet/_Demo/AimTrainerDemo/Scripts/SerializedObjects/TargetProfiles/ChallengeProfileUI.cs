using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChallengeProfileUI : MonoBehaviour, ISettingUI<ChallengeProfile>
{
    public TMP_InputField timeLimit;
    public TMP_InputField targetLimit;

    public Toggle isTimeLimit;
    public Toggle isTargetLimit;

    private GameObject timeLimitWarning;
    private GameObject targetLimitWarning;

    public void Apply(ref ChallengeProfile profile)
    {
        profile.timeLimit = int.Parse(timeLimit.text);
        profile.targetLimit = int.Parse(targetLimit.text);

        profile.isTargetLimit = isTargetLimit.isOn;
        profile.isTimeLimit = isTimeLimit.isOn;
    }

    public void CreateWarnings(GameObject warningPrefab, Transform container)
    {
        targetLimitWarning = UIHelpers.CreateWarning(warningPrefab, 
            targetLimit.gameObject, container, "Target Limit must be in range [" 
            + ChallengeProfile.TARGET_LIMIT_MIN.ToString() + "," 
            + ChallengeProfile.TARGET_LIMIT_MAX.ToString() + "]");

        timeLimitWarning = UIHelpers.CreateWarning(warningPrefab,
            timeLimit.gameObject, container, "Time Limit must be in range ["
            + ChallengeProfile.TIME_LIMIT_MIN.ToString() + ","
            + ChallengeProfile.TIME_LIMIT_MAX.ToString() + "]");
    }

    public void LoadFields(ChallengeProfile profile)
    {
        timeLimit.text = profile.timeLimit.ToString();
        targetLimit.text = profile.targetLimit.ToString();

        isTimeLimit.isOn = profile.isTimeLimit;
        isTargetLimit.isOn = profile.isTargetLimit;

        // because interactability wasn't set initially
        isTimeLimit.onValueChanged.Invoke(profile.isTimeLimit);
        isTargetLimit.onValueChanged.Invoke(profile.isTargetLimit);
    }

    public void SetUIValidation(UnityAction endAction = null)
    {
        UIHelpers.SetInputValidation(timeLimit, timeLimitWarning, 
            ChallengeProfile.TIME_LIMIT_MIN, ChallengeProfile.TIME_LIMIT_MAX, endAction);

        UIHelpers.SetInputValidation(targetLimit, targetLimitWarning,
            ChallengeProfile.TARGET_LIMIT_MIN, ChallengeProfile.TARGET_LIMIT_MAX, endAction);


        isTimeLimit.onValueChanged.AddListener(delegate(bool value)
        {
            isTargetLimit.isOn = targetLimit.interactable = !value;

            isTargetLimit.isOn = !value;
            targetLimit.interactable = !value;

            
            endAction.Invoke();
        });

        isTargetLimit.onValueChanged.AddListener(delegate(bool value)
        {
            isTimeLimit.isOn = !value;
            timeLimit.interactable = !value;
            endAction.Invoke();
        });

        //isTargetLimit.onValueChanged.Invoke(isTargetLimit.isOn);
        //isTimeLimit.onValueChanged.Invoke(isTimeLimit.isOn);
    }
}
