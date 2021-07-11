using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreProfile
{
    // overall stats
    public float timeElapsed;
    public string datePlayed;
    public int targetsSuccessful;
    public int targetsAttempted;

    // click stats
    public float totalClickTime;
    public int clicksSuccessful;
    public int clicksAttempted;
    public int clickCyclesAttempted;
    public int clicksMissed;
    public int clicksTimedOut;

    // tracking stats
    public int tracksSuccessful;
    public int tracksAttempted;
    public float trackTimeSuccessful;
    public float trackTimeAttempted;

    // movement stats
    public float distanceSuccessTotal;

    /// <summary>
    /// Returns string "successfully completed targets / attempted completed targets"
    /// </summary>
    public string OverallRatio
    {
        get
        {
            return targetsSuccessful.ToString() + "/" + targetsAttempted.ToString();
        }
    }

    /// <summary>
    /// Returns per-100 rate of successfully completed targets / attempted completed targets
    /// </summary>
    public float OverallRate
    {
        get
        {
            var value = (float)targetsSuccessful / (float)targetsAttempted;
            if (float.IsNaN(value))
                value = 0f;
            return (value * 100f);
        }
    }

    /// <summary>
    /// Returns string "successful clicks / attempted clicks"
    /// </summary>
    public string ClickRatio
    {
        get
        {
            return clicksSuccessful.ToString() + "/" + clicksAttempted.ToString();
        }
    }

    /// <summary>
    /// Returns per-100 rate of successful clicks / attempted clicks
    /// </summary>
    public float ClickRate
    {
        get
        {
            var value = (float)clicksSuccessful / (float)clicksAttempted;
            if (float.IsNaN(value))
                value = 0f;
            return (value * 100f);
        }
    }

    /// <summary>
    /// Returns per-100 rate of miss clicks to attempted clicks
    /// </summary>
    public float ClicksMissedRate
    {
        get
        {
            var value = (float)clicksMissed / (float)clicksAttempted;
            if (float.IsNaN(value))
                value = 0f;
            return (value * 100f);
        }
    }

    /// <summary>
    /// Returns string "clicksMissed / clicksAttempted"
    /// </summary>
    public string ClicksMissedRatio
    {
        get
        {
            return clicksMissed + "/" + clicksAttempted;
        }
    }

    /// <summary>
    /// Returns sum of successful click times / successful clicks
    /// </summary>
    public float ClickTimeAverage
    {
        get
        {
            float value = totalClickTime / (float)targetsSuccessful;
            if (float.IsNaN(value))
                value = 0f;
            return value;
        }
    }

    /// <summary>
    /// Returns per-100 rate of click timeouts / click cycles
    /// </summary>
    public float ClickTimeoutRate
    {
        get
        {
            var value = (float)clicksTimedOut / (float)clickCyclesAttempted;
            if (float.IsNaN(value))
                value = 0f;
            return value * 100f;
        }
    }

    /// <summary>
    /// Returns string "click timeouts / click cycles"
    /// </summary>
    public string ClickTimeoutRatio
    {
        get
        {
            return clicksTimedOut + "/" + clickCyclesAttempted;
        }
    }

    /// <summary>
    /// Returns string "# of successful tracks / # of attempted tracks"
    /// </summary>
    public string TrackSuccessRatio
    {
        get
        {
            return tracksSuccessful.ToString() + "/" + tracksAttempted.ToString();
        }
    }

    /// <summary>
    /// Returns per-100 rate of successfully completed tracks / attempted completed tracks
    /// </summary>
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

    /// <summary>
    /// Returns per-100 rate of successful tracking time / possible tracking time
    /// </summary>
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
    
    /// <summary>
    /// Returns string "track time successful / track time attempted"
    /// </summary>
    public string TrackRatio
    {
        get
        {
            return trackTimeSuccessful.ToString("f1") + "/" 
                + trackTimeAttempted.ToString("f1");
        }
    }

    public float DistancePerSuccessfulTarget
    {
        get
        {
            var value = distanceSuccessTotal / (float)targetsSuccessful;
            if (float.IsNaN(value))
                value = 0f;
            return value;
        }
    }
}
