[System.Serializable]
public class MovementProfile
{
    public const float SPEED_MIN = .01f;
    public const float ACCEL_MIN = 0f;
    public const float ACCURACY_MIN = 0f;
    public const float ACCURACY_MAX = 1f;
    public const float POW_MIN = 0f;

    public bool canMove = true;
    public float maxSpeed = 5f; // testme

    // time to reach max speed / full stop
    // 0 = instant
    public bool useAccel = true;
    public float accelRate = .2f;

    // 0=instant, 1=linear, >1=ramp
    public float accelPow = 1f;
    public float decelPow = 1f;

    // [0,1] -> percentage of full speed where the player is fully accurate
    // 0=only accurate when fully stopped
    // 1=accurate at all speeds
    public bool useAccuracyRate;
    public float accuracyRate;

    public float Acceleration
    {
        get
        {
            return maxSpeed / accelRate;
        }
    }
}
