using UnityEngine;
using Neet.Data;

public class Target
{
    public const string IS_TARGET_KEY = "isTarget";
    public const string ID_KEY = "num";
    public const string MOVEMENT_KEY = "movement";
    public const string TIMEOUT_KEY = "timeout";
    public const string SPAWN_TIME_KEY = "spawnTime";
    // public const string CLICK_COUNT_KEY = "clickCount";

    public const string TRACK_ACTIVE = "isTracking";
    public const string TRACK_START = "trackStartTime";
    public const string TRACK_SUCCESS_TIME = "trackSuccessTime";
    public const string TRACK_ATTEMPT_TIME_KEY = "trackAttemptTime";
    public const string TRACK_SUCCESS_NOW = "being tracked";
    public const string DISTANCE = "dist";

    public const string PLAYER_DIST = "player dist";

    public int id;
    public float spawnTime;
    public bool isTrackingSuccessful;
    public bool isTracking;
    public float trackStartTime;
    public float distance;
    public float playerDistanceMoved;

    // clicks attempted
    // clicks successful

    // can make other code easier
    // but still would probably be better as a component
    // this system is convoluted
    public static float IncrementSuccessfulTrackTime(GameObject g, float delta)
    {
        var current = g.GetData<float>(TRACK_SUCCESS_TIME);
        var time = current + delta;
        g.SetData(TRACK_SUCCESS_TIME, time);
        return time;
    }
    public static float IncrementAttemptedTrackTime(GameObject g, float delta)
    {
        var current = g.GetData<float>(TRACK_ATTEMPT_TIME_KEY);
        var time = current + delta;
        g.SetData(TRACK_ATTEMPT_TIME_KEY, time);
        return time;
    }

    public static bool IsTarget(GameObject g)
    {
        return g != null && (g.GetData<bool>(IS_TARGET_KEY) || g.GetData<Target>() != null);
    }
}
