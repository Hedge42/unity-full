using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreProfile
{


    public float timeElapsed;
    public int clicksSuccessful;
    public int clicksAttempted;
    public int tracksSuccessful;
    public int tracksAttempted;
    public int targetsSuccessful;
    public int targetsAttempted;
    public float trackTimeSuccessful;
    public float trackTimeAttempted;
    public float totalDistance;
    public string datePlayed;

    public string SuccessRatio
    {
        get
        {
            return targetsSuccessful.ToString() + " / " + targetsAttempted.ToString();
        }
    }
    public string OverallAccuracyString
    {
        get
        {
            return OverallAccuracy.ToString("f1") + "%";
        }
    }
    public float OverallAccuracy
    {
        get
        {
            var value = (float)targetsSuccessful / (float)targetsAttempted;
            if (float.IsNaN(value))
                value = 0f;
            return (value * 100f);
        }
    }

    public string ClickRatio
    {
        get
        {
            return clicksSuccessful.ToString() + " / " + clicksAttempted.ToString();
        }
    }
    public string ClickAccuracyString
    {
        get
        {
            return ClickAccuracy.ToString("f1") + "%";
        }
    }
    public float ClickAccuracy
    {
        get
        {
            var value = (float)clicksSuccessful / (float)clicksAttempted;
            if (float.IsNaN(value))
                value = 0f;
            return (value * 100f);
        }
    }

    public string TrackSuccessRatio
    {
        get
        {
            return tracksSuccessful.ToString() + " / " + tracksAttempted.ToString();
        }
    }
    public string TrackSuccessString
    {
        get
        {
            return TrackSuccessRate.ToString("f1") + "%";
        }
    }
    public float TrackSuccessRate
    {
        get
        {
            var value = (float)tracksSuccessful / (float)tracksAttempted;
            if (float.IsNaN(value))
                value = 0f;
            return (value * 100f);
        }
    }

    public string TrackRateString
    {
        get
        {
            return TrackRate.ToString("f1") + "%";
        }
    }
    public float TrackRate
    {
        get
        {
            var value = trackTimeSuccessful / trackTimeAttempted;
            if (float.IsNaN(value))
                value = 0f;

            return (value * 100f);
        }
    }
}
