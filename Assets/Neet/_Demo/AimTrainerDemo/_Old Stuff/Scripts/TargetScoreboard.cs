using Neet.UI;
using TMPro;
using UnityEngine;
using Neet.Data;

public class TargetScoreboard : MonoBehaviour
{
    // otherwise targetLimit
    public bool isTimeLimit;

    public Menu menu;
    public int timeLimit = 30;
    public int targetLimit = 50;

    [Range(1, 10)] public float accuracyPower = 5f;
    public bool shouldWaitForFirstHit;


    // misc stats
    private float unscaledScore;
    private float timeSinceLastHit;
    private Vector3 lastTargetPos;
    private float totalTTK;

    // shot accuracy
    private int totalShotsFired;
    private int shotsHit;

    // streaks
    private int streak;
    private int bestStreak;

    // success rate
    private int numDestroyed;
    private int numTimedOut;
    private int numTried;

    // live-stats text references
    public TextMeshProUGUI txtGameStrength;
    public TextMeshProUGUI statStatsStreak;
    public TextMeshProUGUI txtStatsAcc;
    public TextMeshProUGUI txtStatsKPS;
    public TextMeshProUGUI txtStatsTimer;

    // results screen references
    public TextMeshProUGUI txtResultsScore;
    public TextMeshProUGUI txtResultsStreak;
    public TextMeshProUGUI txtResultsAcc;
    public TextMeshProUGUI txtResultsKps;
    public TextMeshProUGUI txtResultsMps;
    public TextMeshProUGUI txtResultsAverageTTK;
    public TextMeshProUGUI txtResultsHitPercentage;
    public TextMeshProUGUI txtResultsNumDestroyed;
    public TextMeshProUGUI txtResultsTimeElapsed;

    // rules
    private Timer t;
    private bool waitingForFirstHit = true;
    private bool isFreePlay = true;

    private TargetSetting setting;

    // derived stats
    public float unscaledAccuracy
    {
        get
        {
            if (totalShotsFired == 0)
                return 0f;
            else
                return (float)shotsHit / totalShotsFired;
        }
    }
    public float killsPerSecond
    {
        get
        {
            if (t == null || t.timeElapsed.Equals(0))
                return 0f;

            return shotsHit / t.timeElapsed;
        }
    }
    private float missesPerSecond
    {
        get
        {
            if (t == null || t.timeElapsed <= 0)
                return 0f;

            return numMisses / t.timeElapsed;
        }
    }
    private float numMisses
    {
        get { return totalShotsFired - shotsHit; }
    }
    private float timeBonus
    {
        get
        {
            if (t == null || t.timeElapsed <= 0)
                return 0f;


            // 60s? 1 + 60 / 60 -> 1-1/2
            // 10s? 1 + 10 / 60 -> 1-1/1.1
            var timeFactor = 90;
            var x = 1 + t.timeElapsed / timeFactor;
            return 1 - 1 / (x);
        }
    }
    private float scaledScore
    {
        get
        {
            if (t == null || t.timeElapsed <= 0)
                return 0f;

            var s = unscaledScore * Mathf.Pow(unscaledAccuracy, 5) / t.timeElapsed;
            return s;
        }
    }
    private float targetSuccessRate
    {
        get
        {
            if (numTried == 0)
                return 0;
            else
                return numDestroyed / numTried;
        }
    }
    private float averageTTK
    {
        get
        {
            return totalTTK / numDestroyed;
        }
    }

    private void OnValidate()
    {
        Init();
    }

    public void Init()
    {
        if (menu == null)
            menu = GameObject.Find("TargetMenu").GetComponent<Menu>();
        

        if (timeLimit < 1)
            timeLimit = 1;
        if (targetLimit < 1)
            targetLimit = 1;
    } 

    private void Awake()
    {
        Init();

        SetupTimer();

        Menu.onPause.AddListener(delegate { FinishScoring(); });
    }

    private void Update()
    {
        t.UpdateTimer();

        UpdateStatsText();

        if (txtGameStrength != null)
            txtGameStrength.text = scaledScore.ToString("f0").Colorize("#4C9DE4");
    }

    private void SetupTimer()
    {
        t = new Timer();
        if (isTimeLimit)
            t.onTimerFinished += FinishScoring;
        t.onTimerTick += UpdateTimeSinceLastHit;
    }
    private void StartTime()
    {
        if (!isFreePlay && isTimeLimit)
            t.StartTimer(timeLimit);
        else
            t.StartStopwatch();
    }

    private void ReadSetting()
    {
        setting = GetComponent<TSpawner>().GetSetting();
        isTimeLimit = setting.isTimeLimit;
        timeLimit = setting.timeLimit;
        targetLimit = setting.targetLimit;
    }
    private void UpdateTimeSinceLastHit(float delta)
    {
        timeSinceLastHit += delta;
    }

    private void ResetStats()
    {
        unscaledScore = streak = bestStreak = 0;

        unscaledScore = 0;
        streak = 0;
        timeSinceLastHit = 0;
        lastTargetPos = Vector3.zero;
        totalShotsFired = 0;
        shotsHit = 0;
        bestStreak = 0;
        numDestroyed = 0;
        numTimedOut = 0;
        numTried = 0;
        totalTTK = 0;
    }
    public void StartChallenge()
    {
        isFreePlay = false;
        StartPlay();
    }
    public void StartFreePlay()
    {
        isFreePlay = true;
        StartPlay();
    }
    private void StartPlay()
    {
        // for shared code between freeplay and challenge
        ResetStats();
        waitingForFirstHit = shouldWaitForFirstHit;
        if (!shouldWaitForFirstHit)
            StartTime();

        Menu.Resume();
    }
    public void FinishScoring()
    {
        t.PauseTimer(true);

        Menu.Pause();

        UpdateResultsText();
    }
    public void PauseScoring(bool isPaused)
    {
        t.PauseTimer(isPaused);
    }
    public void ShotFired(bool success)
    {
        // don't process shot in this case
        if (waitingForFirstHit && !success)
            return;

        totalShotsFired++;

        if (success)
        {
            shotsHit++;
            streak++;
            if (streak > bestStreak)
                bestStreak = streak;

            if (waitingForFirstHit)
            {
                StartTime();
                waitingForFirstHit = false;
            }
        }
        else
        {
            streak = 0;
        }
    }

    public void TargetDestroyed(GameObject target)
    {
        var radius = target.transform.localScale.x / 2;
        var rawDist = Vector3.Distance(target.transform.position, lastTargetPos);
        var scaledDist = rawDist / (radius * 2);
        var rawSpeed = target.GetComponent<Rigidbody>().velocity.magnitude;
        var scaledSpeed = rawSpeed / (radius * 2);
        var timeValue = (scaledDist + scaledSpeed) / timeSinceLastHit;
        var ttk = Time.time - target.GetData<float>(Target.SPAWN_TIME_KEY);
        totalTTK += ttk;
        unscaledScore += timeValue;

        numDestroyed += 1;
        numTried += 1;

        timeSinceLastHit = 0;

        lastTargetPos = target.transform.position;

        if (numTried >= targetLimit
            && !isTimeLimit
            && !isFreePlay)
        {
            FinishScoring();
        }
    }
    public void TargetTimeout()
    {
        numTimedOut++;
        numTried++;
    }

    public void UpdateStatsText()
    {
        if (txtStatsKPS != null)
            txtStatsKPS.text = "KPS: " + killsPerSecond;
        if (statStatsStreak != null)
            statStatsStreak.text = "Streak: " + streak;
        if (txtStatsAcc != null)
            txtStatsAcc.text = "Acc: " + (unscaledAccuracy * 100).ToString("f0");
        if (txtStatsTimer != null)
            txtStatsTimer.text = t.timeRemaining.ToString("f1");

    }
    private void UpdateResultsText()
    {
        if (txtResultsScore != null)
            txtResultsScore.text = scaledScore.ToString("f2") + " pts";
        if (txtResultsAcc != null)
            txtResultsAcc.text = (unscaledAccuracy * 100).ToString("f0") + "%";
        if (txtResultsKps != null)
            txtResultsKps.text = killsPerSecond.ToString("f1");
        if (txtResultsMps != null)
            txtResultsMps.text = missesPerSecond.ToString("f1");
        if (txtResultsStreak != null)
            txtResultsStreak.text = bestStreak.ToString() + "x";
        if (txtResultsAverageTTK != null)
            txtResultsAverageTTK.text = (averageTTK * 1000).ToString("f0") + "ms";
        if (txtResultsHitPercentage != null)
            txtResultsHitPercentage.text = (targetSuccessRate * 100).ToString("f0") + "%";
        if (txtResultsNumDestroyed != null)
            txtResultsNumDestroyed.text = numDestroyed.ToString();
        if (txtResultsTimeElapsed != null)
            txtResultsTimeElapsed.text = t.timeElapsed.ToString("f2") + "s";
    }
}
