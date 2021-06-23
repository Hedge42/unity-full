using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.Audio;
using Sirenix.OdinInspector;
using Neet.UI;
using Neet.Events;
using Neet.Data;

public class DoorSpawner : MonoBehaviour
{
    // generic spawner data
    public const string IS_TARGET_KEY = "isTarget";
    public const string TARGET_SPAWN_TIME_KEY = "spawnTime";

    public GameObject targetPrefab;
    public Color targetColor;
    public Color highlightColor;
    public Color activeColor;
    public Color inactiveColor;
    public float targetRadius;

    [Range(0, 75)] public float maxAngle = 20f;
    [Range(.5f, 5)] public float maxLifespan;

    private TargetScoreboard scoreboard;
    private SoundBank sb;

    private Coroutine current;

    [MinMaxSlider(.1f, 3f, true)] public Vector2 speedRange;
    [MinMaxSlider(0f, 5f, true)] public Vector2 delayRange;

    public bool highlightNextSpawner;


    private Motor playerMotor;
    private MouseRotator playerRotator;
    private List<SpawnerClass> activeSpawners;

    public SpawnerClass[] spClasses;
    private GameObject currentTarget;

    [System.Serializable]
    public class SpawnerClass
    {
        public CapsuleExtender capsule;
        public Direction direction;
        public bool active;
        public enum Direction
        {
            Left,
            Right,
            Both
        }
    }

    private void OnDrawGizmos()
    {
        // AddTrackers();
        foreach (var sc in spClasses)
        {
            var center = sc.capsule.GetInterpolatedPosition(.5f);
            var t = sc.capsule.transform;

            if (sc.direction == SpawnerClass.Direction.Left
                || sc.direction == SpawnerClass.Direction.Both)
                Gizmos.DrawLine(center, center - t.right);

            if (sc.direction == SpawnerClass.Direction.Right
                || sc.direction == SpawnerClass.Direction.Both)
                Gizmos.DrawLine(center, center + t.right);
        }
    }

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        var player = Player.main;
        activeSpawners = new List<SpawnerClass>();

        foreach (var sc in spClasses)
        {
            if (sc.active)
            {
                activeSpawners.Add(sc);
                sc.capsule.SetColor(activeColor);
            }
            else
            {
                sc.capsule.SetColor(inactiveColor);
            }


            sc.capsule.SetRadius(targetRadius * 2);
        }

        if (scoreboard == null)
            scoreboard = GetComponent<TargetScoreboard>();
    }

    private void Awake()
    {
        Init();
        UserEvent.SetStaticListener(Gun.SHOT_FIRED_KEY, ShotFired);
        sb = GetComponent<SoundBank>();
        Menu.onPause.AddListener(StopPlaying);
        // AddTrackers();
    }
    IEnumerator Start()
    {
        yield return null;
        if (!Menu.isPaused)
            StartPlaying();
    }

    private void AddTrackers()
    {
        foreach (var sp in spClasses)
            sp.capsule.GetOrAddComponent<Tracker>();
    }
    public void StartFreePlay()
    {
        scoreboard.StartFreePlay();
        StartPlaying();
    }
    public void StartChallenge()
    {
        scoreboard.StartChallenge();
        StartPlaying();
    }

    public void StartPlaying()
    {
        Menu.Resume();
        StartSpawnRoutine();
    }
    public void StopPlaying()
    {
        if (current != null)
        {
            StopCoroutine(current);
            current = null;
        }

        if (currentTarget != null)
            Destroy(currentTarget);
    }
    void StartSpawnRoutine()
    {
        if (current != null)
        {
            StopCoroutine(current);
            current = null;
        }

        current = StartCoroutine(DelaySpawn());
    }
    private IEnumerator DelaySpawn()
    {
        // get spawner to use (highlight NOW)
        var spc = GetRandomSpawner();
        if (highlightNextSpawner)
            HighlightCapsule(spc.capsule);

        var delay = Random.Range(delayRange.x, delayRange.y);
        yield return new WaitForSeconds(delay);
        var target = SpawnTarget(spc);
        currentTarget = target;

        yield return new WaitForSeconds(maxLifespan);
        if (target != null)
        {
            // TODO miss
            Destroy(target);
            currentTarget = null;

            StartSpawnRoutine();
        }
    }
    private SpawnerClass GetRandomSpawner()
    {
        return activeSpawners[Random.Range(0, activeSpawners.Count)];
    }
    private void HighlightCapsule(CapsuleExtender capsule)
    {
        foreach (var spc in spClasses)
        {
            spc.capsule.SetColor(activeColor);
        }
        capsule.SetColor(highlightColor);
    }
    GameObject SpawnTarget(SpawnerClass spc)
    {
        // spawn and set data
        var target = Instantiate(targetPrefab);
        target.SetColor(targetColor);
        target.transform.localScale = Vector3.one * targetRadius * 2;
        target.SetActive(true);
        target.SetData(IS_TARGET_KEY, true);
        target.SetData(TARGET_SPAWN_TIME_KEY, Time.time);

        // target position
        var t = Random.Range(0f, 1f);
        var pos = spc.capsule.GetInterpolatedPosition(t);
        target.transform.position = pos;

        // target velocity
        var rSpeed = Random.Range(speedRange.x, speedRange.y);
        var rAngle = Random.Range(-maxAngle, maxAngle);

        // get direction
        Vector3 vLR = Vector3.zero;
        if (spc.direction == SpawnerClass.Direction.Left)
            vLR = Vector3.left;
        else if (spc.direction == SpawnerClass.Direction.Right)
            vLR = Vector3.right;
        else // if (spc.direction == SpawnerClass.Direction.Both)
            vLR = Random.value < 0.5f ? Vector3.left : Vector3.right;

        // use direction to set velocity
        var dir = Quaternion.Euler(0, 0, rAngle) * vLR;
        var localDir = spc.capsule.GetComponent<Tracker>().transform.rotation * dir;
        var vel = localDir.normalized * rSpeed;

        target.GetComponent<Rigidbody>().velocity = vel;

        return target;
    }

    private void ShotFired(object sender, object receiver)
    {
        GameExtensions.Cast(sender, receiver, out Gun g, out GameObject target);

        if (target != null && target.GetData<bool>(IS_TARGET_KEY))
        {

            // should the scoreboard decide what happens when damage is taken?
            StartCoroutine(DelaySpawn());
            // scoreboard.Hit(target);
            sb.Play(0);

            Destroy(target);
        }
        else
        {
            // TODO miss
            // scoreboard.Miss();
            // scoreboard.Miss();
        }
    }
}
