using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    // referenced https://gist.github.com/ftvs/5822103

    public Transform targetOverride;
    private Transform target;
    public bool reduceShakeToZero = true;
    public float shakeDuration = .3f;
    public float positionalIntensity = 0.7f;
    public float rotationalIntensity = 10f;
    public Curve intensityCurve;
    public float singleShakeTime;

    private Vector3 originalPos;
    private Vector3 originalRot;
    private Quaternion originalRotQ;
    private Coroutine currentShake;

    private void Awake()
    {
        if (targetOverride == null)
            target = targetOverride = this.transform;
        else
            target = targetOverride;
    }

    void OnEnable()
    {
        originalPos = target.localPosition;
        originalRot = target.localEulerAngles;
        originalRotQ = target.localRotation;
    }

    private void Update()
    {
    }

    public void StartShaking()
    {
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float timeRemaining = shakeDuration;

        while (timeRemaining > 0)
        {
            // wait for currentShake to finish
            if (currentShake != null)
            {
                timeRemaining -= Time.deltaTime;
                yield return null;
                continue;
            }

            // calculate intensity
            float pintensity = positionalIntensity;
            float rintensity = rotationalIntensity;
            float t = 1 - (timeRemaining / shakeDuration);
            if (reduceShakeToZero)
            {
                if (intensityCurve != null)
                    t = intensityCurve.Ferp(t);

                pintensity = Mathf.Lerp(positionalIntensity, 0, t);
                rintensity = Mathf.Lerp(rotationalIntensity, 0, t);
            }

            // start shaking instance
            Vector3 targetPosition = originalPos + Random.insideUnitSphere * pintensity;
            Vector3 targetRotation = originalRot + Random.insideUnitSphere * rintensity;
            currentShake = StartCoroutine(Travel(targetPosition, targetRotation));

            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        // ensure smooth finish
        while (currentShake != null)
            yield return null;
        StartCoroutine(Travel(originalPos, originalRot));
    }

    IEnumerator Travel(Vector3 endLocation, Vector3 endRotation)
    {
        Vector3 startLocation = target.localPosition;
        Quaternion startRotQ = target.localRotation;
        Quaternion endRotQ = Quaternion.Euler(endRotation);

        float timeRemaining = singleShakeTime;
        while (timeRemaining > 0)
        {
            float t = 1 - (timeRemaining / singleShakeTime);
            target.localPosition = Vector3.Lerp(startLocation, endLocation, t);
            target.localRotation = Quaternion.Slerp(startRotQ, endRotQ, t);
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        target.localPosition = endLocation;
        target.localRotation = endRotQ;
        currentShake = null;
    }
}
