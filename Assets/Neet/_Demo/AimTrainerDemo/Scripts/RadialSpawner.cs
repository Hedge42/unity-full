using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.Events;
using Neet.Data;

public class RadialSpawner : MonoBehaviour
{
    private const string IS_TARGET_KEY = "isTarget";
    private const string TARGET_NUM_KEY = "num";
    private const string TARGET_MOVEMENT_KEY = "movement";

    public GameObject targetPrefab;
    public Transform outerTransform;
    public Transform innerTransform;

    public float targetRadius;
    public float startSpeed;
    public float targetAccelleration;
    public float spawnerScale;
    public float despawnerScale;

    public float spawnRate;

    public bool isPlaying;
    public bool hideReticleOnRaycast;

    private GameObject reticle;
    private MouseRotator rotator;

    private Transform cam;

    private IEnumerator Start()
    {
        Init();
        reticle = GameObject.FindObjectOfType<GunUI>().gameObject;
        rotator = FindObjectOfType<MouseRotator>();
        cam = Camera.main.transform;

        UserEvent.SetStaticListener(Gun.SHOT_FIRED_KEY, ShotFired);

        var me = innerTransform.GetOrAddComponent<MonoEvent>();
        yield return null;
        me.onTriggerEnter.AddListener(OnDespawnerCollision);

        yield return new WaitForSeconds(1);
        StartPlaying();
    }
    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        outerTransform.localScale = new Vector3(spawnerScale, .15f, spawnerScale);
        innerTransform.localScale = new Vector3(despawnerScale, .1f, despawnerScale);
    }

    private void Update()
    {
        if (!hideReticleOnRaycast)
            return;

        // raycast forward from the mouseRotator
        Physics.Raycast(new Ray(cam.position, cam.forward), out RaycastHit rh);
        if (rh.collider != null)
        {
            if (rh.collider.gameObject.transform == innerTransform)
            {
                reticle.SetActive(false);
            }
            else
            {
                reticle.SetActive(true);
            }
        }
    }

    public void StartPlaying()
    {
        isPlaying = true;
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (isPlaying)
        {
            var g = SpawnTarget();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void OnDespawnerCollision(Collider other)
    {
        if (other.gameObject.GetData<bool>(IS_TARGET_KEY))
        {
            // other miss logic...
            Destroy(other.gameObject);
        }
    }

    private GameObject SpawnTarget()
    {
        GameObject target = Instantiate(targetPrefab);
        Vector3 pos = Random.insideUnitCircle.normalized * spawnerScale / 2;
        // translate xy to xz
        pos = new Vector3(
            outerTransform.position.x + pos.x,
            outerTransform.position.y + pos.y, 
            outerTransform.position.z + pos.z);

        target.transform.position = pos;
        target.SetData(IS_TARGET_KEY, true);
        target.SetActive(true);

        target.transform.localScale = Vector3.one * targetRadius * 2;

        var rb = target.GetComponent<Rigidbody>();
        var dir = (innerTransform.position - target.transform.position).normalized;
        rb.velocity = dir * startSpeed;
        return target;
    }

    private void ShotFired(object sender, object receiver)
    {
        GameExtensions.Cast(sender, receiver, out Gun g, out GameObject target);

        if (target != null && target.GetData<bool>(IS_TARGET_KEY))
        {
            Destroy(target);
        }
    }
}
