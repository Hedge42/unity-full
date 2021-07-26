[System.Serializable]
public class TimingProfile
{
    // range of posssible values
    public const float TIMEOUT_MIN = .1f;
    public const float TIMEOUT_MAX = 2f;
    public const float DELAY_MIN = 0f;
    public const float DELAY_MAX = 10f;

    // data and defaults
    public float timeout = .5f;
    public float delayMin = .5f;
    public float delayMax = 1f;
    public bool canClickTimeout = true;
    public bool canDelay = true;
}
