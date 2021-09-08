[System.Serializable]
public class TrackingProfile
{
    // value ranges
    public const float ACCEL_MIN = 0f;
    public const float ACCEL_MAX = 100f;
    public const float SPEED_MIN = 0f;
    public const float SPEED_MAX = 100f;
    public const float TICK_RATE_MIN = .1f;
    public const float TICK_RATE_MAX = 3f;
    public const float TIME_MIN = .1f;
    public const float TIME_MAX = 10f;

    // data
    public float accelMin = 1f;
    public float accelMax = 3f;
    public float speedMin = 10f;
    public float speedMax = 50f;
    public float tickRate = .3f;

    public bool canMove = false;
    public bool isMoveInstant = true;
    public bool canTrack = false;
    public bool isTrackInstant;
    public bool canTrackTimeout = false;
    public bool canTrackDestroy = false;

    public float timeToDestroy = 1f;
    public float trackTimeout = 2f;
}
