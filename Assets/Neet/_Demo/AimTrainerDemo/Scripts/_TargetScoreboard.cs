using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Neet.Data;
using UnityEngine.Events;
using Neet.UI;
using Neet.Audio;

public class _TargetScoreboard : MonoBehaviour
{
    public _ScoreboardHUD hud;
    public _ScoreResultsUI results;

    private PresetProfile profile => PresetProfile.current;
    private bool isTimeLimit => profile.challengeProfile.isTimeLimit;
    private int timeLimit => profile.challengeProfile.timeLimit;
    private int targetLimit => profile.challengeProfile.targetLimit;
    private ScoreProfile score;

    private Timer t;
    private bool isFreePlay = true;

    public UnityAction ShowResults;

    [HideInInspector]
    public SoundBank sb;

    private void Start()
    {
        // setup Timer
        t = new Timer();
        t.onTimerFinished += delegate { CompleteChallenge(); };
        t.onTimerTick += delegate { score.timeElapsed = t.timeElapsed; };
        sb = GetComponent<SoundBank>();
    }

    private void Update()
    {
        // might be better in a coroutine
        t.UpdateTimer();

        if (score != null)
            hud.UpdateText(score, profile.challengeProfile, !isFreePlay);
    }

    public void StartScoring(bool challenge)
    {
        // called on first target click
        isFreePlay = !challenge;
        score = new ScoreProfile();
        StartTime();
    }
    private void StartTime()
    {
        if (!isFreePlay && isTimeLimit)
            t.StartTimer(timeLimit);
        else
            t.StartStopwatch();
    }

    public void Stop()
    {
        t.PauseTimer(true);

        // would be called on scene load since pause happens at start
        if (score != null)
        {
            results.UpdateGUI(profile, score);
            ShowResults?.Invoke();
        }
    }
    public void SaveScore()
    {
        profile.scores.Add(score);
        score.datePlayed = System.DateTime.Now.ToString();
        PresetCollection.loaded.Save();
    }

    public void ClickFired(bool success)
    {
        score.clicksAttempted += 1;
        if (success)
        {
            sb.Play("click");
            score.clicksSuccessful += 1;
        }
    }
    public void TrackElapsed(float time)
    {
        score.trackTimeAttempted += time;
    }
    public void TrackSuccess(float time)
    {
        score.trackTimeSuccessful += time;
    }

    // target success methods
    public void ClickDestroyed(GameObject target)
    {
        // redundant method for consistency and flexibility
        TargetDestroyed(target);
    }
    public void TrackDestroyed(GameObject target)
    {
        score.tracksAttempted += 1;
        score.tracksSuccessful += 1;

        TargetDestroyed(target);
    }
    private void TargetDestroyed(GameObject target)
    {
        score.targetsSuccessful += 1;
        score.targetsAttempted += 1;

        score.totalDistance += target.GetData<float>(Target.PLAYER_DIST);

        // overrides click sound on clickDestroy
        sb.Play("destroy");

        if (TargetLimitReached())
            CompleteChallenge();
    }

    // handling timeouts / fail
    public void TargetFailed()
    {
        score.targetsAttempted += 1;
        sb.Play("fail");

        if (TargetLimitReached())
            CompleteChallenge();
    }
    public void ClickTimeout()
    {
        TargetFailed();
    }
    public void TrackTimeout()
    {
        score.tracksAttempted += 1;

        TargetFailed();
    }

    // handling challenge completion
    private void CompleteChallenge()
    {
        SaveScore();
        PauseListener.Pause();
    }
    private bool TargetLimitReached()
    {
        return score.targetsAttempted >= targetLimit
            && profile.challengeProfile.isTargetLimit
            && !isFreePlay;
    }
}
