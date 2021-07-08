using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.Audio;
using Neet.UI;
using Neet.Data;
using Neet.Events;

public class BigTurnSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Color targetColor;
    public float targetRadius;
    public float targetDistance;
    public GameObject turnIndicatorRotator;
    public bool showIndicator;
    public bool allowBackward;
    public bool only90s;
    public bool only180s;

    private SoundBank sb;
    private Transform player;
    private bool waitingForFirstHit;
    private int targetNum = 0;
    private GameObject currentTarget;
    private TargetScoreboard scoreboard;


    private bool isPlaying;
    private bool isFreePlay;

    private int lastLR = -1;



    private Vector3[] directions = new Vector3[]
    {
        // WASD
        Vector3.forward,
        Vector3.left,
        Vector3.back,
        Vector3.right
    };
    private int lastDirection = -1;

    private void Awake()
    {
        Menu.onPause.AddListener(StopPlaying);
        UserEvent.SetStaticListener(Gun.SHOT_FIRED_KEY, ShotFired);
    }
    private void OnValidate()
    {
        Init();
    }

    private void Init()
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
        if (only180s)
        {
            allowBackward = true;
            only90s = false;
        }
        if (only90s)
        {
            only180s = false;
        }

        if (targetDistance < 1f)
            targetDistance = 1f;
        if (targetRadius < .1f)
            targetRadius = .1f;
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (turnIndicatorRotator != null)
            {
                RotateIndicator();
            }
        }
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
        lastDirection = -1;
        SpawnTarget();
        if (showIndicator)
            turnIndicatorRotator?.SetActive(true);
        isPlaying = true;
    }
    public void StopPlaying()
    {
        Menu.Pause();
        if (currentTarget != null)
            Destroy(currentTarget);
        turnIndicatorRotator?.SetActive(false);
        isPlaying = false;
    }
    private void SpawnTarget()
    {
        var target = Instantiate(targetPrefab, this.transform);
        target.SetActive(true);
        target.transform.localScale = Vector3.one * targetRadius * 2;
        target.SetColor(targetColor);
        SetTargetPosition(target);

        target.SetData(Target.ID_KEY, targetNum++);
        target.SetData(Target.IS_TARGET_KEY, true);
        target.SetData(Target.SPAWN_TIME_KEY, Time.time);

        currentTarget = target;
    }
    private void SetTargetPosition(GameObject target)
    {
        Vector3 pos;

        // first position is straight forward
        if (lastDirection < 0)
        {
            pos = transform.position + directions[0] * targetDistance;
            lastDirection = 0;
        }
        // just make sure it's not the same direction as before
        else
        {
            var dir = Random.Range(0, 4);
            while (!IsValidDir(dir))
                dir = Random.Range(0, 4);

            pos = transform.position + directions[dir] * targetDistance;


            if (dir % 2 == 1)
                lastLR = dir;
            lastDirection = dir;
        }

        target.transform.position = pos;
    }
    private bool IsValidDir(int dir)
    {
        if (dir == lastDirection)
            return false;

        // if there was a last LR and the last direction was backward
        //if (lastLR > 0 && lastDirection == 2)
        //{
        //    // dir is left of right
        //    if (dir % 2 == 1)
        //    {
        //        // dir cannot be other LR
        //        if (lastLR != dir)
        //            return false;
        //    }
        //}
        if (!allowBackward && dir == 2)
            return false;

        if (only90s)
        {
            if (Mathf.Abs(dir + lastDirection) % 2 != 1)
                return false;
        }
        if (only180s)
        {
            if (Mathf.Abs(dir + lastDirection) % 2 != 0)
                return false;
        }

        return true;
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
            SpawnTarget();
        }
        else
        {
            scoreboard.ShotFired(false);
        }
    }

    private void RotateIndicator()
    {
        // https://answers.unity.com/questions/1038431/make-ui-compass-point-to-3d-object.html
        var targetPosLocal = player.InverseTransformPoint(currentTarget.transform.position);
        var targetAngle = -Mathf.Atan2(targetPosLocal.x, targetPosLocal.z) * Mathf.Rad2Deg;
        turnIndicatorRotator.transform.eulerAngles = new Vector3(0, 0, targetAngle);
    }
}
