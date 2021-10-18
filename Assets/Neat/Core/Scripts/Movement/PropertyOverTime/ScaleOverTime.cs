using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOverTime : MonoBehaviour
{
    public bool cycleOnStart;

    public bool scaleX;
    public bool scaleY;
    public bool scaleZ;

    [Range(-1f, 10f)] public float amountToScale = 1f;
    [Range(.1f, 2f)] public float time = .5f;

    public bool reverse;
    public bool repeat;


    private Vector3 startScale;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        startScale = transform.localScale;
    }
    private void Start()
    {
        if (cycleOnStart)
            ForceStart();
    }
    public void ForceStart()
    {
        ForceStop();
        currentCoroutine = StartCoroutine(Cycle());
    }
    public void ForceStop()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
    }

    private IEnumerator Cycle()
    {
        float startTime = Time.time;
        Vector3 endScale = startScale + Vector3.one * amountToScale;
        while (Time.time < startTime + time)
        {
            float timePassed = Time.time - startTime;
            float ratio = timePassed / time;

            transform.localScale = Vector3.Lerp(startScale, endScale, ratio);
            yield return null;
        }

        if (reverse)
        {
            // scale down to startX
            float startTimeReverse = Time.time;
            while (Time.time < startTimeReverse + time)
            {
                float timePassed = Time.time - startTimeReverse;
                float ratio = timePassed / time;

                // startScale is the target...
                transform.localScale = Vector3.Lerp(endScale, startScale, ratio);
                yield return null;
            }
        }

        if (repeat)
        {
            StartCoroutine(Cycle());
        }
    }
    private IEnumerator CycleSystem()
    {
        Stopwatch s = new Stopwatch();
        float startTime = 0f;
        Vector3 endScale = startScale + Vector3.one * amountToScale;
        while (s.ElapsedMilliseconds / 1000 < startTime + time)
        {
            float timePassed = s.ElapsedMilliseconds;
            float ratio = timePassed / time;

            transform.localScale = Vector3.Lerp(startScale, endScale, ratio);
            yield return null;
        }

        if (reverse)
        {
            // scale down to startX
            float startTimeReverse = s.ElapsedMilliseconds;
            while (Time.time < startTimeReverse + time)
            {
                float timePassed = Time.time - startTimeReverse;
                float ratio = timePassed / time;

                // startScale is the target...
                transform.localScale = Vector3.Lerp(endScale, startScale, ratio);
                yield return null;
            }
        }

        if (repeat)
        {
            StartCoroutine(Cycle());
        }
    }

    private void CycleSystemTime()
    {
        // https://www.dotnetperls.com/stopwatch
        Stopwatch s = new Stopwatch();

        s.Start();
    }
}
