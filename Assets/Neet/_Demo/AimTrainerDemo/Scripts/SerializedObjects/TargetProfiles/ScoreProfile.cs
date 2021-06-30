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
    public string SuccessRate
    {
        get
        {
            var value = (float)targetsSuccessful / (float)targetsAttempted;
            if (float.IsNaN(value))
                value = 0f;
            return (value * 100f).ToString("f1") + "%";
        }
    }
    public string FlickSuccessRatio
    {
        get
        {
            return clicksSuccessful.ToString() + " / " + clicksAttempted.ToString();
        }
    }
    public string FlickSuccessRate
    {
        get
        {
            var value = (float)clicksSuccessful / (float)clicksAttempted;
            if (float.IsNaN(value))
                value = 0f;
            return (value * 100f).ToString("f1") + "%";
        }
    }
    public string TrackSuccessRatio
    {
        get
        {
            return tracksSuccessful.ToString() + " / " + tracksAttempted.ToString();
        }
    }
    public string TrackSuccessRate
    {
        get
        {
            var value = (float)tracksSuccessful / (float)tracksAttempted;
            if (float.IsNaN(value))
                value = 0f;
            return (value * 100f).ToString("f1") + "%";
        }
    }
    public string TrackRate
    {
        get
        {
            var value = trackTimeSuccessful / trackTimeAttempted;
            if (float.IsNaN(value))
                value = 0f;

            return (value * 100f).ToString("f1") + "%";
        }
    }

}
