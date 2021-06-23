using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class _ScoreboardHUD : MonoBehaviour
{
    public TextMeshProUGUI time;
    public TextMeshProUGUI accuracy;
    public TextMeshProUGUI targets;

    public AccuracyBar acc;

    public void UpdateText(ScoreProfile s, ChallengeProfile c, bool isChallenge)
    {
        // time remaining vs elapsed
        if (isChallenge && c.isTimeLimit)
            time.text = (c.timeLimit - s.timeElapsed).ToString("f1");
        else
            time.text = s.timeElapsed.ToString("f1");

        // targets remaining vs ratio
        if (isChallenge && c.isTargetLimit)
            targets.text = s.targetsAttempted + " / " + c.targetLimit.ToString();
        else
            targets.text = s.SuccessRatio;

        // same regardless of settings
        accuracy.text = s.SuccessRate;
    }
}
