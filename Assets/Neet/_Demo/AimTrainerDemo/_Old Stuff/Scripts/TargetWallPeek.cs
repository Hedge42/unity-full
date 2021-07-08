using Neet.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.UI;
using Neet.Data;

public class TargetWallPeek : MonoBehaviour
{
    private const string IS_TARGET_KEY = "isTarget";

    public GameObject targetPrefab;
    public GameObject leftVolume;
    public GameObject rightVolume;
    public Color targetColor;
    public float targetRadius;

    public float centerWallWidth;
    public Vector3 regionSize;

    private List<GameObject> activeTargets;

    bool spawnLeft;

    private void Awake()
    {
        activeTargets = new List<GameObject>();
        Menu.onPause.AddListener(DestroyActive);
        UserEvent.SetStaticListener(Gun.SHOT_FIRED_KEY, ShotFired);
    }
    void DestroyActive()
    {
        foreach (GameObject g in activeTargets)
            Destroy(g);
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

        spawnLeft = Random.value < .5f;
        SpawnTarget();
    }

    void SpawnTarget()
    {
        var spawnVolume = spawnLeft ? leftVolume : rightVolume;

        var target = Instantiate(targetPrefab);
        target.SetColor(targetColor);
        target.transform.localScale = Vector3.one * targetRadius * 2;
        target.SetActive(true);
        target.SetData(IS_TARGET_KEY, true);

        var xRange = spawnVolume.transform.lossyScale.x / 2;
        var yRange = spawnVolume.transform.lossyScale.y / 2;
        var rx = Random.Range(-xRange, xRange);
        var ry = Random.Range(-yRange, yRange);

        target.transform.position = spawnVolume.transform.position + new Vector3(rx, ry);
    }

    private void ShotFired(object sender, object receiver)
    {
        GameExtensions.Cast(sender, receiver, out Gun g, out GameObject target);

        if (target != null && target.GetData<bool>(IS_TARGET_KEY))
        {
            Destroy(target);
            spawnLeft = !spawnLeft;
            SpawnTarget();
        }
        else
        {
            // scoreboard.Miss();
        }
    }

    

}
