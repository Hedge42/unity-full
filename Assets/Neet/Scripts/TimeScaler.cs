using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    [Range(.1f, 2f)]
    public float timeScale;
    public bool playOnAwake;

    private void Awake()
    {
        if (playOnAwake)
            Scale();
    }

    public void Scale()
    {
        Time.timeScale = timeScale;
    }

    public void Stop()
    {
        Time.timeScale = 0f;
    }

    public void Normal()
    {
        Time.timeScale = 1f;
    }
}
