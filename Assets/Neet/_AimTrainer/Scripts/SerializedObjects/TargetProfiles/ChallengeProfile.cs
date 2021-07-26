using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChallengeProfile
{
    // possible values
    public const int TIME_LIMIT_MIN = 5;
    public const int TIME_LIMIT_MAX = 600; // 10 minutes
    public const int TARGET_LIMIT_MIN = 1;
    public const int TARGET_LIMIT_MAX = 100;

    // data
    public int timeLimit = 30;
    public int targetLimit = 20;
    public bool isTimeLimit = true;
    public bool isTargetLimit = false;
}
