using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Neet.Audio;
using Neet.Events;

public class Gun : MonoBehaviour
{
    public const string PRIMARY_FIRE_KEY = "primary fire";
    public const string SHOT_FIRED_KEY = "shoot";
    public const string SHOT_MISSED_KEY = "miss";
    public const string ACTIVE_RAYCAST_KEY = "active raycast";
    public const string PASSIVE_RAYCAST_KEY = "passive raycast";

    [HideInInspector]
    public WeaponHandler weaponHandler;
    public GameObject model;
    public bool showModel;
    public Transform muzzle;
    public bool showBulletTracer;
    public bool showHitMarker;
    public CapsuleExtender bulletTracer;
    public bool automatic;
    [Range(.2f, 3f)]
    public float reloadTime = .5f;
    [Range(.05f, 2)]
    public float fireRate = .2f;

    [HideInInspector]
    public int currentClip;
    [HideInInspector]
    public int currentAmmo;
    public bool infiniteAmmo;
    [Range(1, 100)]
    public int maxClip = 10;
    [Range(1, 999)]
    public int maxAmmo = 90;

    [Range(.1f, 2f)]
    public float accuracyRecoveryRate;
    [Range(0, .3f)]
    public float initialInaccuracy;
    [Range(0, .6f)]
    public float accuracyPenalty;

    // events for the RESULTS of shots
    public event Action onShotFired;
    public event Action<RaycastHit> onRaycast;
    public event Action<Collider> onShotHit;
    public event Action onShotMissed;
    // public event Action<RaycastHit> onActiveRaycast;
    public event Action<RaycastHit> onPassiveRaycast;


    private Recoiler rc;
    private SoundBank sb;

    private float currentInaccuracy;
    private Coroutine currentShot;
    private int numShots = 0;

    public Transform raycastStart;

    private void OnValidate()
    {
        Init();
    }
    private void Init()
    {
        model.transform.GetChild(0).gameObject.SetActive(showModel);
    }
    private void Awake()
    {
        Init();
        rc = GetComponent<Recoiler>();
        sb = GetComponent<SoundBank>();

        onShotFired += delegate { rc?.StartRecoil(); };
        onShotFired += delegate { if (sb != null && sb.sounds.Length > 0) sb?.Play(0); };
    }
    private void Start()
    {
        currentClip = maxClip;
        currentAmmo = maxAmmo;

        currentInaccuracy = initialInaccuracy;
    }

    private void OnEnable()
    {
        Transform t = transform;
        while (weaponHandler == null && t != null)
        {
            weaponHandler = t.GetComponent<WeaponHandler>();
            t = t.parent;
        }
    }

    private void Update()
    {
        PassiveRaycast();
    }

    public void ClearExternalEvents()
    {
        foreach (Action<Collider> a in onShotHit.GetInvocationList())
            onShotHit -= a;
        foreach (Action<RaycastHit> a in onRaycast.GetInvocationList())
            onRaycast -= a;
        foreach (Action a in onShotMissed.GetInvocationList())
            onShotMissed -= a;
    }

    public void StartPrimaryFire(KeyCode[] keys)
    {
        // only start if the gun is not currently firing
        if (currentShot == null)
            currentShot = StartCoroutine(PrimaryFire(keys));
    }
    public void StartAlternateFire(KeyCode[] keys)
    {
        StartCoroutine(AlternateFire(keys));
    }
    public void StartReload(KeyCode[] keys)
    {
        StartCoroutine(Reload(keys));
    }
    public void StartActiveRaycast(KeyCode[] keys)
    {
        StartCoroutine(ActiveRaycast(keys));
    }

    private IEnumerator ActiveRaycast(KeyCode[] keys)
    {
        while (keys.GetKey())
        {
            var rh = Raycast();
            UserEvent.InvokeStaticEvent(ACTIVE_RAYCAST_KEY, this, rh.collider?.gameObject);
            yield return new WaitForEndOfFrame();
        }
    }
    public void PassiveRaycast()
    {
        onPassiveRaycast?.Invoke(Raycast());
    }

    private RaycastHit Raycast()
    {
        //Vector3 rand = UnityEngine.Random.insideUnitCircle;
        //rand *= currentInaccuracy;
        Ray r = new Ray(raycastStart.position,
            raycastStart.forward);

        Physics.Raycast(r, out RaycastHit rh);
        
        return rh;
    }

    private IEnumerator PrimaryFire(KeyCode[] keys)
    {
        do
        {
            if (!infiniteAmmo)
            {
                currentClip -= 1;
                currentAmmo -= 1;
            }


            Vector3 rand = UnityEngine.Random.insideUnitCircle;
            rand *= currentInaccuracy;
            //Ray r = new Ray(Camera.main.transform.position, Camera.main.transform.forward + rand);
            //Ray r = new Ray(weaponHandler.bulletStart.position, weaponHandler.bulletStart.forward + rand);
            Ray r = new Ray(raycastStart.position, raycastStart.forward);
            Physics.Raycast(r, out RaycastHit rh);

            // StartCoroutine(HandleInaccuracy());
            if (showBulletTracer)
                StartCoroutine(SpawnBulletTracer(rh, r));
            if (showHitMarker)
                StartCoroutine(SpawnHitMarker(rh));

            UserEvent.InvokeStaticEvent(SHOT_FIRED_KEY, this, rh.collider?.gameObject);
            if (rh.collider == null)
                UserEvent.InvokeStaticEvent(SHOT_MISSED_KEY, this);
            else
                rh.collider.gameObject.InvokeUserEvent(SHOT_FIRED_KEY, this, rh.collider.gameObject);


            onRaycast?.Invoke(rh);
            onShotFired?.Invoke();

            yield return new WaitForSeconds(fireRate);
            if (!automatic)
                break;
        }
        while (keys.GetKeyDown() && currentClip > 0);

        currentShot = null;
    }
    private IEnumerator AlternateFire(KeyCode[] keys)
    {

        yield return null;
    }
    private IEnumerator Reload(KeyCode[] keys)
    {
        bool canReload = currentClip < maxClip;

        int bulletsAvailable = currentAmmo < maxClip ? currentAmmo : maxClip;
        int bulletsRequested = maxClip - currentClip;
        int reloadAmount = bulletsRequested < bulletsAvailable ?
            bulletsRequested : bulletsAvailable;

        yield return new WaitForSeconds(reloadTime);

        currentClip = maxClip;
    }

    private IEnumerator SpawnBulletTracer(RaycastHit rh, Ray r)
    {
        if (bulletTracer == null)
            yield break;

        float tracerTime = .05f;
        float dist = 20;
        Vector3 target;
        if (rh.collider == null)
            target = Camera.main.transform.position
                + r.direction * dist;
        else
            target = rh.point;

        GameObject tracer =
            Instantiate(bulletTracer.gameObject, muzzle.position, Quaternion.identity);

        tracer.GetComponent<CapsuleExtender>().Extend(target);
        yield return new WaitForSeconds(tracerTime);
        Destroy(tracer);

    }
    private IEnumerator SpawnHitMarker(RaycastHit rh)
    {
        if (rh.collider == null)
            yield break;

        GameObject hit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hit.GetComponent<Renderer>().material.color = new Color(.6f, .3f, .3f, .3f);
        hit.transform.position = rh.point;
        hit.transform.localScale = .2f * Vector3.one;
        Destroy(hit.GetComponent<Collider>());

        float t = .2f;
        yield return new WaitForSeconds(t);
        Destroy(hit);
    }

    private IEnumerator HandleInaccuracy()
    {
        numShots += 1;
        int numFixedUpdates = Convert.ToInt32(accuracyRecoveryRate / Time.fixedDeltaTime);
        float perTick = accuracyPenalty / numFixedUpdates;

        currentInaccuracy += perTick * numFixedUpdates;

        for (int i = 0; i < numFixedUpdates; i++)
        {
            currentInaccuracy -= perTick;
            yield return new WaitForFixedUpdate();
        }
        numShots -= 1;
        if (numShots == 0)
            currentInaccuracy = initialInaccuracy;
    }

    // TODO ??
    public void StopPrimaryFire() { /**/ }
    public void StopAlternateFire() { /**/ }
    public void StopReload() { /**/ }
}
