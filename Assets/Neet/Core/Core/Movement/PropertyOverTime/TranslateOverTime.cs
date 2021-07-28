using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateOverTime : MonoBehaviour
{
    public Transform target;

    public bool reverseAtEnd;
    public bool playAutomatically;
    public bool loop;

    [Serializable]
    public class Translation
    {
        public Curve curve;
        public Vector3 direction;
        public bool useLocalRotation;
        [Range(0f, 2f)]
        public float time;
    }
    public Translation[] translations;

    public Vector3 awakePos { get; private set; }

    private Coroutine currentRoutine = null;

    // Monobehaviors
    private void Awake()
    {
        if (target == null)
            target = transform;

        awakePos = target.localPosition;
    }
    private void OnEnable()
    {
        if (playAutomatically)
            StartTranslating();
    }
    private void OnDrawGizmos()
    {
        // this may cause performance issues, but only in the editor
        Vector3[] points = GetPoints();
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 start = points[i];
            Vector3 end = points[i + 1];
            Debug.DrawLine(start, end, Color.green);
        }

        if (!reverseAtEnd)
            Debug.DrawLine(points[points.Length - 1], points[0], Color.green);
    }

    public void StartTranslating()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(TranslateRoutine());
    }
    private Vector3[] GetPoints()
    {
        if (target == null)
            target = transform;

        List<Vector3> points = new List<Vector3>();

        // startPoint reference is dependent on playing-state
        Vector3 startPoint;
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
            startPoint = awakePos;
        else
            startPoint = target.position;
#else 
        // TODO this needs testing
        startPoint = awakePos;
#endif
        points.Add(startPoint);

        // Draw line for each translation...
        if (translations != null)
        {
            for (int i = 0; i < translations.Length; i++)
            {
                Vector3 translation = translations[i].useLocalRotation ? target.localRotation * translations[i].direction : translations[i].direction;
                Vector3 endPoint = startPoint + translation;

                Debug.DrawLine(startPoint, endPoint, Color.green);

                startPoint = endPoint;
                points.Add(startPoint);
            }
        }

        return points.ToArray();
    }

    // Functions
    private IEnumerator TranslateRoutine()
    {
        // There is nowhere to move...
        // ...if there is not at least 1 translation
        if (translations == null || translations.Length == 0)
            // https://forum.unity.com/threads/breaking-out-of-a-coroutine.2893/
            yield break;

        // This is okay here, because it's only being called once...
        // ...every few seconds. Shit.

        Vector3[] points = GetPoints();
        for (int i = 0; i < translations.Length; i++)
        {
            float _startTime = Time.time;

            // points should have 1 more elements than translations
            Vector3 _start = points[i];
            Vector3 _end = points[i + 1];

            while (Time.time < _startTime + translations[i].time)
            {
                float ratio = (Time.time - _startTime) / translations[i].time;
                Translate(_start, _end, ratio, translations[i]);

                yield return null;
            }
        }

        if (reverseAtEnd)
        {
            // do the exact same thing as above, but in reverse...

            // loop backward...
            for (int i = translations.Length - 1; i >= 0; i--)
            {
                float _startTime = Time.time;

                // points should have 1 more elements than translations
                Vector3 _start = points[i + 1];
                Vector3 _end = points[i];

                while (Time.time < _startTime + translations[i].time)
                {
                    float ratio = (Time.time - _startTime) / translations[i].time;
                    Translate(_start, _end, ratio, translations[i]);
                    yield return null;
                }
            }
        }

        if (loop)
            StartCoroutine(TranslateRoutine());
    }

    private void Translate(Vector3 a, Vector3 b, float t, Translation T)
    {
        if (T.curve != null)
            //transform.localPosition = Vector3.Lerp(a, b, T.curve.Ferp(t));
            target.localPosition = T.curve.Verp(a, b, t);
        else
            target.localPosition = Vector3.Lerp(a, b, t);
    }
}
