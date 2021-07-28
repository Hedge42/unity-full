using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    [System.Serializable]
    public class Rotation
    {
        public Vector3 rotation;
        [Range(0, 2f)]
        public float time;
        public Curve curve;
    }

    public Transform target;
    public bool reverseAtEnd;
    public bool playAutomatically;
    public bool loop;
    public Rotation[] rotations;

    public Vector3 awakeRot { get; private set; }

    private Coroutine currentRoutine = null;

    // Monobehaviors
    private void Awake()
    {
        if (target == null)
            target = transform;

        awakeRot = target.localRotation.eulerAngles;
    }
    private void OnEnable()
    {
        if (playAutomatically)
            StartRotating();
    }

    public void StartRotating()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(RotateRoutine());
    }
    private Vector3[] GetRots()
    {
        List<Vector3> rots = new List<Vector3>();

        // startPoint reference is dependent on playing-state
        Vector3 startRot = awakeRot;

        rots.Add(startRot);

        // Draw line for each translation...
        if (rotations != null)
        {
            for (int i = 0; i < rotations.Length; i++)
            {
                Vector3 rotation = rotations[i].rotation;
                Vector3 endRot = startRot + rotation;
                rots.Add(endRot);

                startRot = endRot;
            }
        }

        return rots.ToArray();
    }

    private IEnumerator RotateRoutine()
    {
        // There is nowhere to move...
        // ...if there is not at least 1 translation
        if (rotations == null || rotations.Length == 0)
            // https://forum.unity.com/threads/breaking-out-of-a-coroutine.2893/
            yield break;

        Vector3[] rots = GetRots();
        for (int i = 0; i < rotations.Length; i++)
        {
            float _startTime = Time.time;

            // points should have 1 more elements than translations
            Vector3 _start = rots[i];
            Vector3 _end = rots[i + 1];

            while (Time.time < _startTime + rotations[i].time)
            {
                float ratio = (Time.time - _startTime) / rotations[i].time;
                Rerp(_start, _end, ratio, rotations[i]);

                yield return null;
            }
        }

        // reverse or circle around...
        if (reverseAtEnd)
        {
            // do the exact same thing as above, but in reverse...

            // loop backward...
            for (int i = rotations.Length - 1; i >= 0; i--)
            {
                float _startTime = Time.time;

                // points should have 1 more elements than translations
                Vector3 _start = rots[i + 1];
                Vector3 _end = rots[i];

                while (Time.time < _startTime + rotations[i].time)
                {
                    float ratio = (Time.time - _startTime) / rotations[i].time;
                    Rerp(_start, _end, ratio, rotations[i]);
                    yield return null;
                }
            }
        }

        if (loop)
            StartCoroutine(RotateRoutine());
    }

    private void Rerp(Vector3 a, Vector3 b, float t, Rotation R)
    {
        if (R.curve != null)
            //transform.localPosition = Vector3.Lerp(a, b, T.curve.Ferp(t));
            target.localRotation = Quaternion.Euler(R.curve.Verp(a, b, t));
        else
            target.localRotation = Quaternion.Euler(Vector3.Lerp(a, b, t));
    }
}
