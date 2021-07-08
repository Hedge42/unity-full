using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.File;
using Neet.UI;

public class TrackerSpawner : TSpawner
{
    [HideInInspector]
    public TargetSetting setting;
    public TargetScoreboard scoreboard;
    public Transform targetContainer;

    public GameObject targetPrefab;
    private GameObject target;

    public Color trackingColor;
    public Color missingColor;
    public float radius;
    public float distance;

    public float angleMin;
    public float angleMax;

    public float accelMin;
    public float accelMax;
    public float speedMin;
    public float speedMax;

    public float xAngleMax;
    public float yAngleMax;

    public float secondsPerTurnMin;
    public float secondsPerTurnMax;

    private Camera cam;

    private void Awake()
    {
        Init();
        Menu.onPause.AddListener(delegate { StopPlaying(); });
    }
    private void OnValidate()
    {
        Init();
    }

    public override void Init()
    {
        if (scoreboard == null)
            scoreboard = GetComponent<TargetScoreboard>();
        if (cam == null)
            cam = Camera.main;

        // radius
        if (radius < .1f)
            radius = .1f;
        if (radius > 50)
            radius = 50;

        // dist
        if (distance < 1)
            distance = 1;
        if (distance > 50)
            distance = 50;

        // angle
        if (angleMin < 0)
            angleMin = 0;
        else if (angleMin > 90)
            angleMin = 0;
        if (angleMax < angleMin)
            angleMax = angleMin;
        else if (angleMax > 90)
            angleMax = 90;

        // accel
        if (accelMin < .1f)
            accelMin = .1f;
        else if (accelMin > 50)
            accelMin = 50;
        if (accelMax < accelMin)
            accelMax = accelMin;
        else if (accelMax > 50)
            accelMax = 50;

        // speed
        if (speedMin < .1f)
            speedMin = .1f;
        else if (speedMin > 100)
            speedMin = 100;
        if (speedMax < speedMin)
            speedMax = speedMin;
        else if (speedMax > 100)
            speedMax = 100;

        // turns
        if (secondsPerTurnMin < 0.1f)
            secondsPerTurnMin = 0.1f;
        else if (secondsPerTurnMax > 4f)
            secondsPerTurnMin = 4f;
        if (secondsPerTurnMax < secondsPerTurnMin)
            secondsPerTurnMax = secondsPerTurnMin;
        else if (secondsPerTurnMax > 4f)
            secondsPerTurnMax = 4f;

        // bounds
        if (xAngleMax < 10)
            xAngleMax = 10;
        if (yAngleMax < 5)
            yAngleMax = 5;

        MakeSetting();
    }
    public override TargetSetting GetSetting()
    {
        if (setting == null)
            MakeSetting();

        return setting;
    }
    public override void ApplySetting(TargetSetting s)
    {
        setting = s;
        missingColor = setting.color;
        trackingColor = setting.trackingColor;
        radius = setting.radius;
        distance = setting.distance;
        angleMin = setting.angleMin;
        angleMax = setting.angleMax;
        accelMin = setting.accelMin;
        accelMax = setting.accelMax;
        speedMin = setting.speedMin;
        speedMax = setting.speedMax;
        secondsPerTurnMin = setting.secondsPerTurnMin;
        secondsPerTurnMax = setting.secondsPerTurnMax;
    }
    TargetSetting MakeSetting()
    {
        setting = new TargetSetting();
        setting.accelMin = accelMin;
        setting.accelMax = accelMax;
        setting.speedMin = speedMin;
        setting.speedMax = speedMax;
        setting.secondsPerTurnMin = secondsPerTurnMin;
        setting.secondsPerTurnMax = secondsPerTurnMax;
        setting.angleMin = angleMin;
        setting.angleMax = angleMax;
        setting.xAngleMax = xAngleMax;
        setting.yAngleMin = yAngleMax;
        setting.radius = radius;
        setting.distance = distance;
        return setting;
    }

    public void StartFreePlay()
    {
        StartPlaying();
    }
    public void StartChallenge()
    {
        StartPlaying();
    }
    public void StartPlaying()
    {
        Menu.Resume();
        target = Instantiate(targetPrefab, targetContainer);
        target.SetActive(true);
        target.transform.localPosition = Vector3.forward * distance;
        target.transform.localScale = Vector3.one * radius;
        StartCoroutine(PlayRoutine(target));

    }
    public void StopPlaying()
    {
        StopAllCoroutines();
        Destroy(target);
        Menu.Pause();
    }
    private IEnumerator PlayRoutine(GameObject target)
    {
        var rb = target.GetComponent<Rigidbody>();
        var turnTime = Random.Range(secondsPerTurnMin, secondsPerTurnMax);

        // if target is outside of the bounds,
        // set direction toward local origin

        Vector3 dir = default;
        target.transform.LookAt(cam.transform);

        // check if target is outside of the bounding box
        var relativePos = target.transform.position - cam.transform.position;
        var xAngle = 90 - (Mathf.Atan2(relativePos.z, relativePos.x) * Mathf.Rad2Deg);
        var yAngle = 90 - (Mathf.Atan2(relativePos.z, relativePos.y) * Mathf.Rad2Deg);
        xAngle = Mathf.Abs(xAngle);
        yAngle = Mathf.Abs(yAngle);

        if (xAngle > xAngleMax || yAngle > yAngleMax)
        {
            var origin = cam.transform.position + Vector3.forward * distance;
            dir = origin - target.transform.position;
            dir = target.transform.rotation * dir.normalized;
        }
        else
        {
            // get random accelleration vector
            var angle = Random.Range(angleMin, angleMax);
            var LR = Random.Range(0f, 1f) < .5f ? -1f : 1f;
            var UD = Random.Range(0f, 1f) < .5f ? -1f : 1f;
            var x = Mathf.Cos(angle * Mathf.Deg2Rad) * LR;
            var y = Mathf.Sin(angle * Mathf.Deg2Rad) * UD;
            dir = new Vector3(x, y, 0);
        }

        dir = dir.normalized;
        var accel = Random.Range(accelMin, accelMax);
        var accel_vec = dir * accel;

        var startTime = Time.time;
        while (Time.time < startTime + turnTime)
        {
            target.transform.LookAt(cam.transform);
            //Vector3 rot = transform.rotation.eulerAngles;
            //target.transform.localRotation = Quaternion.Euler(rot.x, rot.y, 0);

            // add accelleration, fix velocity
            rb.AddRelativeForce(accel_vec, ForceMode.Acceleration);
            if (rb.velocity.magnitude > speedMax)
                rb.velocity = rb.velocity.normalized * speedMax;

            // fix distance
            target.transform.position = cam.transform.position
                + (target.transform.position - cam.transform.position)
                .normalized * distance;

            // raycast from camera
            Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit rh);
            if (rh.collider != null && rh.collider.gameObject == target)
            {
                target.SetColor(trackingColor);
            }
            else
            {
                target.SetColor(missingColor);
            }

            yield return null;
        }

        StartCoroutine(PlayRoutine(target));
    }
}
