using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.Audio;
using Neet.UI;
using Neet.Data;
using Neet.Events;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Neet.File;

public class FOVSpawner : MonoBehaviour
{
    public GameObject targetPrefab;

    private PresetProfile profile => PresetProfile.current;

    // colors
    private Color targetColor;
    private Color dummyColor;
    private Color backgroundColor;
    private Color centerColor;

    public Image centerVertical;
    public Image centerHorizontal;

    public float targetRadius;
    public float targetDistance;

    private SoundBank sb;
    private Transform player;
    private bool waitingForFirstHit;
    private int targetNum = 0;
    private GameObject currentTarget;
    private TargetScoreboard scoreboard;

    public float hfov = 103;
    public float minHorizontalAngle;
    public float maxHorizontalAngle;

    public float minVerticalAngle;
    public float maxVerticalAngle;

    public float spawnDelay;

    public bool canTimeout;
    public float timeoutTime;
    public bool resetToCenter;
    public bool startOnFirstHit;

    private bool isFreePlay;
    private bool lastWasCenter = false;
    

    private void Awake()
    {
        Init();
        Menu.onPause.AddListener(StopPlaying);
        UserEvent.SetStaticListener(Gun.SHOT_FIRED_KEY, ShotFired);
    }
    private void OnValidate()
    {
        Init();
    }

    public void Init()
    {
        if (player == null)
        {
            //player = FindObjectOfType<MouseRotator>().yRotTarget;
            player = Camera.main.transform;
        }
        if (sb == null)
            sb = GetComponent<SoundBank>();
        if (scoreboard == null)
            scoreboard = GetComponent<TargetScoreboard>();

        if (targetDistance < 1f)
            targetDistance = 1f;
        if (targetRadius < .1f)
            targetRadius = .1f;

        if (maxHorizontalAngle > hfov / 2f)
            maxHorizontalAngle = hfov / 2f;
        else if (maxHorizontalAngle < minHorizontalAngle)
            maxHorizontalAngle = minHorizontalAngle;
        if (minHorizontalAngle < 0f)
            minHorizontalAngle = 0f;
        else if (minHorizontalAngle > maxHorizontalAngle)
            minHorizontalAngle = maxHorizontalAngle;

        var vfov = hfov * 9f / 16f;
        if (maxVerticalAngle > vfov / 2f)
            maxVerticalAngle = vfov / 2f;
        else if (maxVerticalAngle < minVerticalAngle)
            maxVerticalAngle = minVerticalAngle;
        if (minVerticalAngle < 0f)
            minVerticalAngle = 0f;
        else if (minVerticalAngle > maxVerticalAngle)
            minVerticalAngle = maxVerticalAngle;

        if (timeoutTime < .15f)
            timeoutTime = .15f;

        if (spawnDelay < 0)
            spawnDelay = 0;
        else if (spawnDelay > 1f)
            spawnDelay = 1f;

        LoadSettings();
    }

    private void LoadSettings()
    {
        var colorProfile = profile.colorProfile;
        var aimProfile = profile.aimProfile;
        var timingProfile = profile.timingProfile;

        // loading
        targetColor = colorProfile.targetColor;
        backgroundColor = colorProfile.backgroundColor;
        centerColor = colorProfile.centerColor;

        // applying
        Camera.main.backgroundColor = backgroundColor;
        centerVertical.color = centerColor;
        centerHorizontal.color = centerColor;

    }

    public void StartFreePlay()
    {
        StartPlaying();
        isFreePlay = true;
    }
    public void StartChallenge()
    {
        StartPlaying();
        isFreePlay = false;
    }
    public void StartPlaying()
    {
        Menu.Resume();
        waitingForFirstHit = true;
        StartCoroutine(DelayRoutine());
        waitingForFirstHit = startOnFirstHit;
    }
    public void StopPlaying()
    {
        Menu.Pause();
        if (currentTarget != null)
            Destroy(currentTarget);
        StopAllCoroutines();
    }
    private void SpawnTarget()
    {
        // to prevent a double spawn
        if (currentTarget != null)
            return;

        var target = Instantiate(targetPrefab, this.transform);
        target.SetActive(true);
        target.transform.localScale = Vector3.one * targetRadius * 2;
        target.SetColor(targetColor);
        SetTargetPosition(target);

        target.SetData(Target.ID_KEY, targetNum++);
        target.SetData(Target.IS_TARGET_KEY, true);
        target.SetData(Target.SPAWN_TIME_KEY, Time.time);

        currentTarget = target;

        if (canTimeout && !waitingForFirstHit)
            StartCoroutine(TimeoutRoutine(target));
    }
    private IEnumerator DelayRoutine()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnTarget();
    }
    private IEnumerator TimeoutRoutine(GameObject target)
    {
        var startTime = Time.time;
        while (Time.time < startTime + timeoutTime)
            yield return null;

        yield return null;
        if (target != null)
        {
            Destroy(target);
            currentTarget = null;
            scoreboard.TargetTimeout();
            StartCoroutine(DelayRoutine());
            //SpawnTarget();
        }
    }
    private void SetTargetPosition(GameObject target)
    {
        if ((!lastWasCenter && resetToCenter) || waitingForFirstHit)
        {
            target.transform.position = player.position + Vector3.forward * targetDistance;
            lastWasCenter = true;
        }
        else
        {
            // get horizontal angle
            var hRandAngle = Random.Range(minHorizontalAngle, maxHorizontalAngle);
            var hRandRad = hRandAngle * Mathf.Deg2Rad;

            // get vertical angle
            var vRandAngle = Random.Range(minVerticalAngle, maxVerticalAngle);
            var vRandRad = vRandAngle * Mathf.Deg2Rad;

            // get horizontal coords
            int LR = Random.Range(0f, 1f) < .5f ? -1 : 1;
            var hRandZ = Mathf.Cos(hRandRad);
            var hRandX = Mathf.Sin(hRandRad);

            // get vertical coords
            int UD = Random.Range(0f, 1f) < .5f ? -1 : 1;
            var vRandZ = Mathf.Cos(vRandRad);
            var vRandX = Mathf.Sin(vRandRad);

            //var randUnit = new Vector3(hRandX * LR, 0, hRandZ).normalized;
            var randUnit = new Vector3(hRandX * LR, vRandX * UD, hRandZ).normalized;

            var relativePos = randUnit * targetDistance;
            var absolutePos = player.position + relativePos;

            target.transform.position = absolutePos;
            lastWasCenter = false;
        }
    }
    private void ShotFired(object sender, object receiver)
    {
        GameExtensions.Cast(sender, receiver, out Gun g, out GameObject target);

        if (target != null && target.GetData<bool>(Target.IS_TARGET_KEY))
        {
            if (waitingForFirstHit)
            {
                waitingForFirstHit = false;

                if (isFreePlay)
                    scoreboard.StartFreePlay();
                else
                    scoreboard.StartChallenge();
            }
            else
            {
                scoreboard.ShotFired(true);
                scoreboard.TargetDestroyed(target);
            }

            sb.Play(0);
            Destroy(target);
            currentTarget = null;
            StartCoroutine(DelayRoutine());
            //SpawnTarget();
        }
        else
        {
            scoreboard.ShotFired(false);
        }
    }
}
