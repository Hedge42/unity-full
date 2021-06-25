[System.Serializable]
public class AimProfile
{
    // range of possible values
    // public const float RADIUS_MIN = .05f;
    // public const float RADIUS_MAX = 2f;
    public const float X_MIN = 0f;
    public const float X_MAX = 180f;
    public const float Y_MIN = 0f;
    public const float Y_MAX = 90f;

    public const float DIST_MIN = 2f;
    public const float DIST_MAX = 300f;

    public const float SPAWN_ROTATE_MIN = 0f;
    public const float SPAWN_ROTATE_MAX = 180f;

    // public float radius = .5f;
    public bool useDistRange = true;
    public float distMin = 10f;
    public float distMax = 30f;
    public float xMin = 1f;
    public float xMax = 20f;
    public float yMin = 0f;
    public float yMax = 5f;
    public bool showCenterLines = true;
    public bool failTargetOnMissClick;

    public bool canSpawnRotate = false;
    public float spawnRotate = 45;
}
