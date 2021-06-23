using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.UI;
using Neet.Data;
using Neet.Events;
using Neet.Audio;

public class TargetTrackPeek : TSpawner
{
    public GameObject targetPrefab;
    public Motor playerMotor;
    public Tracker tracker;
    public CapsuleExtender capsule;
    public GameObject spawnVolume;
    public Color targetColor;
    public Color trackerColor;

    [Range(.1f, 10f)]
    public float spawnHeight = 3f;

    // min-max sliders

    public bool showNextDirectionColor;
    public Color leftColor;
    public Color rightColor;

    public TargetScoreboard scoreboard;

    private TargetTrackPeekScoreboard tpScoreboard;
    private SoundBank sb;

    private Coroutine current;
    private GameObject currentTarget;

    private TargetSetting _setting;
    private TargetSetting setting
    {
        get
        {
            if (_setting == null)
                LoadSetting();
            return _setting;
        }
        set
        {
            _setting = value;
        }
    }

    private void OnValidate()
    {
        Init();
    }

    public override void Init()
    {
        if (playerMotor == null)
            playerMotor = GameObject.FindObjectOfType<Motor>();
        if (tracker == null)
            tracker = GameObject.FindObjectOfType<Tracker>();
        if (capsule == null)
            capsule = tracker.GetComponent<CapsuleExtender>();
        if (scoreboard == null)
            scoreboard = GetComponent<TargetScoreboard>();
        if (tpScoreboard == null)
            tpScoreboard = GetComponent<TargetTrackPeekScoreboard>();


        capsule.SetRadius(setting.radius * 2); // FIXME
        capsule.SetLength(spawnHeight);
        capsule.mat.color = trackerColor;
    }

    private void Awake()
    {
        Init();
        UserEvent.SetStaticListener(Gun.SHOT_FIRED_KEY, ShotFired);
        sb = GetComponent<SoundBank>();

        Menu.onPause.AddListener(StopPlay);
    }
    void Start()
    {
        LoadSetting();
    }

    public void StartPlay()
    {
        Menu.Resume();

        if (current != null)
        {
            StopCoroutine(current);
            current = null;
        }

        tpScoreboard.StartPlaying();
        current = StartCoroutine(DelaySpawn());
    }
    public void StopPlay()
    {
        Menu.Pause();
        tpScoreboard.StopPlaying();
    }

    public void StartFreePlay()
    {
        scoreboard.StartFreePlay();
        StartPlay();
    }
    public void StartChallenge()
    {
        scoreboard.StartChallenge();
        StartPlay();
    }

    IEnumerator DelaySpawn()
    {
        // set capsule color based on direction
        var vLR = Random.value < 0.5f ? Vector3.left : Vector3.right;
        if (showNextDirectionColor)
        {
            if (vLR.Equals(Vector3.left))
                capsule.SetColor(leftColor);
            else
                capsule.SetColor(rightColor);
        }

        var delay = Random.Range(setting.delayMin, setting.delayMax);
        yield return new WaitForSeconds(delay);
        var target = SpawnTarget(vLR);

        yield return new WaitForSeconds(setting.lifespanMax);
        if (target != null)
        {
            // TODO miss
            scoreboard.TargetTimeout();
            Destroy(target);
            current = StartCoroutine(DelaySpawn());
        }
    }
    GameObject SpawnTarget(Vector3 vLR)
    {
        // spawn and set data
        var target = Instantiate(targetPrefab);
        target.SetColor(targetColor);
        target.transform.localScale = Vector3.one * setting.radius * 2;
        target.SetActive(true);
        target.SetData(Target.IS_TARGET_KEY, true);
        target.SetData(Target.SPAWN_TIME_KEY, Time.time);

        var t = Random.Range(0f, 1f);
        var pos = capsule.GetInterpolatedPosition(t);
        target.transform.position = pos;

        // target velocity
        var rSpeed = Random.Range(setting.speedMin, setting.speedMax);
        var rAngle = Random.Range(-setting.angleMax, setting.angleMax);

        // var vLR = Random.value < 0.5f ? Vector3.left : Vector3.right;
        var dir = Quaternion.Euler(0, 0, rAngle) * vLR;
        var localDir = tracker.transform.rotation * dir;
        var vel = localDir * rSpeed;

        var LR = Random.value < 0.5f ? -1 : 1;
        target.GetComponent<Rigidbody>().velocity = vel;

        currentTarget = target;

        return target;
    }
    private void ShotFired(object sender, object receiver)
    {
        GameExtensions.Cast(sender, receiver, out Gun g, out GameObject target);

        if (target != null && target.GetData<bool>(Target.IS_TARGET_KEY))
        {

            // should the scoreboard decide what happens when damage is taken?
            StartCoroutine(DelaySpawn());
            scoreboard.TargetDestroyed(target);
            scoreboard.ShotFired(true);
            tpScoreboard.Hit(target);
            sb.Play(0);

            Destroy(target);
        }
        else
        {
            // TODO miss
            scoreboard.ShotFired(false);
            // scoreboard.Miss();
        }
    }

    public override TargetSetting GetSetting()
    {
        if (setting == null)
            setting = GetDefaultSetting();

        return setting;
    }
    public override void ApplySetting(TargetSetting s)
    {
        setting = s;
    }

    private TargetSetting GetDefaultSetting()
    {
        TargetSetting ts = new TargetSetting();
        ts.radius = 0.15f;
        ts.delayMin = 1f;
        ts.delayMax = 3f;
        ts.speedMin = 1f;
        ts.speedMax = 3f;
        ts.lifespanMax = 1f;
        ts.angleMax = 30f;
        ts.distanceMax = 5f;
        return ts;
    }

    private void LoadSetting()
    {
        var found = TargetSetting.LoadJson(this.GetType());
        if (found.Length < 1)
            _setting = GetDefaultSetting();
        else
            _setting = found[0];

        TargetSetting.current = _setting;
    }
}
