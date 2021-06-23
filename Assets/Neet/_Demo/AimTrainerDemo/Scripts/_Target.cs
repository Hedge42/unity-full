using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// to replace Target.cs ??
public class _Target : MonoBehaviour
{
    public int id;

    public bool isTracking;
    public bool isTrackSuccessful;
    public float successfulTrackTime;
    public float attemptedTrackTime;

    private void Update()
    {
        if (isTracking)
        {
            attemptedTrackTime += Time.deltaTime;

            if (isTrackSuccessful)
                successfulTrackTime += Time.deltaTime;
        }
    }

    public static _Target Create(GameObject prefab, Transform parent, int _id)
    {
        _Target t = Instantiate(prefab, parent).GetComponent<_Target>();
        t.isTracking = false;
        t.id = _id;

        return t;
    }

    public static bool IsTarget(GameObject g, out _Target t)
    {
        t = g.GetComponent<_Target>();
        return t != null;
    }
}
