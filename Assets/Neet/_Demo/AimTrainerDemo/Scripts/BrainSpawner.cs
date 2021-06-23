using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.Audio;
using Sirenix.OdinInspector;
using Neet.Events;
using Neet.Data;
using Neet.UI;

public class BrainSpawner : MonoBehaviour
{
    // generic spawner data
    public const string IS_TARGET_KEY = "isTarget";
    public const string TARGET_SPAWN_TIME_KEY = "spawnTime";

    public GameObject spawnerPrefab;
    public GameObject targetPrefab;

    public Color targetColor;
    public Color leftColor;
    public Color rightColor;

    public float spawnerHeight;
    public float targetRadius;

    [Range(0, 75)] public float maxAngle = 20f;
    [Range(.5f, 5)] public float maxLifespan = .7f;

    private SoundBank sb;

    [MinMaxSlider(.1f, 3f, true)] public Vector2 speedRange;
    [MinMaxSlider(0f, 5f, true)] public Vector2 delayRange;

    private Motor playerMotor;
    private MouseRotator playerRotator;





    private void Awake()
    {
        UserEvent.SetStaticListener(Gun.SHOT_FIRED_KEY, ShotFired);
        sb = GetComponent<SoundBank>();
    }
    IEnumerator Start()
    {
        yield return null;
        if (!Menu.isPaused)
            StartPlay();
    }

    public void StartPlay()
    {
        Menu.onResume.Invoke();

        StartSpawnRoutine();
    }
    void StartSpawnRoutine()
    {
        //if (current != null)
        //{
        //    StopCoroutine(current);
        //    current = null;
        //}

        //current = StartCoroutine(DelaySpawn());
    }

    private IEnumerator DelaySpawn()
    {
        //// get spawner to use (highlight NOW)
        //var spc = GetRandomSpawner();
        //if (highlightNextSpawner)
        //    HighlightCapsule(spc.capsule);

        //var delay = Random.Range(delayRange.x, delayRange.y);
        //yield return new WaitForSeconds(delay);
        //var target = SpawnTarget(spc);

        //yield return new WaitForSeconds(maxLifespan);
        //if (target != null)
        //{
        //    // TODO miss
        //    Destroy(target);

        //    StartSpawnRoutine();
        //}

        yield return null;
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
        }
    }
}
