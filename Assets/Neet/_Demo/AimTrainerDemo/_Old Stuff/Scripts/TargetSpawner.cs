using System.Collections.Generic;
using UnityEngine;
using Neet.Events;
using Neet.Data;
using System;
using Sirenix.OdinInspector;
using Neet.Audio;

public class TargetSpawner : MonoBehaviour
{
    // references
    public GameObject targetPrefab;
    public Transform targetContainer;
    public CapsuleExtender connectorPrefab;
    public Transform connectorContainer;
    public TargetScoreboard scoreboard;

    // spawn settings
    public Vector2 regionSize = Vector2.one;
    [HideInInspector] public float minDistance;
    [HideInInspector] public float maxDistance;
    [HideInInspector] public float minAngle;
    [HideInInspector] public float maxAngle;
    public float minNonconsecutiveDistance = .1f;
    public int spawnCount = 3;
    public int minPoints = 3;
    public int maxPoints = 10;
    public bool generateOnValidate = true;
    public bool shouldTimeout;
    [Range(.3f, 1f)] public float timeoutTime = .75f;
    public bool shouldDelay;
    [MinMaxSlider(0, 3)] public Vector2 delayRange = new Vector2(.2f, 1f);

    // color settings
    [HideInInspector] public Color gizmoStartColor = Color.red;
    [HideInInspector] public Color gizmoEndColor = Color.blue;
    [HideInInspector] public List<Color> comboColors;
    public bool shouldColorLerp;
    public int swapColorEvery;

    // target settings
    [Range(.01f, 2f)] public float targetRadius = 1;
    public float capsuleTargetHeight = 1f;
    public float targetAccelleration;
    public float targetStartSpeed;
    public float targetMaxSpeed;
    public bool distanceWrapping;

    // data
    [HideInInspector] public List<GameObject> activeTargets;
    [HideInInspector] public List<Vector2> points;
    private PointGenerator pg;
    private List<CapsuleExtender> connectors;
    private int sphereNum = 0;

    public event Action<GameObject> onFirstHit;
    private bool waitingForFirstHit;

    private SoundBank sb;
    private Transform centerWrap;

    private bool isTargetCapsule;

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        ValidateReferences();

        isTargetCapsule = targetPrefab.GetComponent<CapsuleExtender>() != null;
    }

    void OnDrawGizmos()
    {
        // https://medium.com/nicholasworkshop/how-to-rotate-gizmos-to-fit-a-game-object-in-unity-fadc97e1e9de
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(Vector3.zero, (Vector3)regionSize);
        Gizmos.color = gizmoStartColor;
    }
    private void Awake()
    {
        Init();
        // StartFreePlay();
        UserEvent.SetStaticListener(Gun.SHOT_FIRED_KEY, ShotFired);

        // centerWrap = Player.main.GetComponent<CapsuleExtender>().head;
        centerWrap = FindObjectOfType<Motor>().GetComponent<CapsuleExtender>().head;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Pressed");
            var cc = new CoroutineChainer(this);
            cc.AddRoutine(testRoutine());
            cc.AddRoutine(testRoutine());
            cc.AddRoutine(testRoutine());
            cc.Start();
        }
    }
    private System.Collections.IEnumerator testRoutine()
    {
        yield return new WaitForSeconds(1.5f);
    }

    private void ValidateReferences()
    {
        // TODO automate this?
        if (targetContainer == null)
            // only line that needs customization
            targetContainer = GameObject.Find("TargetContainer")?.transform;

        if (connectorContainer == null)
            connectorContainer = GameObject.Find("ConnectorContainer")?.transform;

        if (scoreboard == null)
            scoreboard = GameObject.FindObjectOfType<TargetScoreboard>();

        if (sb == null)
            sb = GetComponent<SoundBank>();
    }

    public void Generate()
    {
        if (pg == null)
            pg = new PointGenerator();

        pg.regionSize = regionSize;
        pg.maxPoints = maxPoints;
        pg.minDistance = minDistance;
        pg.maxDistance = maxDistance;
        //pg.variance = new Vector2(xVariance, yVariance);
        pg.avoidCount = spawnCount;
        pg.minAngle = minAngle;
        pg.maxAngle = maxAngle;
        pg.avoidDistance = minNonconsecutiveDistance;


        points = pg.GeneratePoints(points);
    }
    public void ReGeneratePoints()
    {
        points = null;
        Generate();
    }
    private void ShotFired(object sender, object receiver)
    {
        GameExtensions.Cast(sender, receiver, out Gun g, out GameObject target);

        if (isTargetCapsule)
        {
            Transform parent = null;
            if (target != null)
                parent = target.transform.parent;
            if (parent != null)
                parent = parent.transform.parent;
            if (parent != null)
                target = parent.gameObject;
        }

        if (target != null && target.GetData<bool>(Target.IS_TARGET_KEY))
        {
            if (waitingForFirstHit)
            {
                waitingForFirstHit = false;
                onFirstHit.Invoke(target);
            }

            scoreboard.ShotFired(true);
            scoreboard.TargetDestroyed(target);
            sb.Play(0);

            // SpawnConnectors();
            // StartCoroutine(HitMarker(target));

            // NextTarget(target);

            StartCoroutine(TargetRoutine(target));
        }
        else
        {
            scoreboard.ShotFired(false);
        }
    }

    private void UpdateTarget(GameObject target)
    {
        // move to back of list
        activeTargets.Remove(target);
        activeTargets.Add(target);

        // handle internal data
        target.SetData(Target.ID_KEY, sphereNum++);
        StopCoroutine(target.GetData<Coroutine>(Target.MOVEMENT_KEY));
        target.SetData(Target.MOVEMENT_KEY, StartCoroutine(Gravitate(target)));
        target.SetData(Target.SPAWN_TIME_KEY, Time.time);

        // set object data
        UpdateColor(target);
        target.transform.localPosition = ConsumePoint();

        if (distanceWrapping)
        {
            var center = targetContainer.transform.position;

            var r = Vector3.Magnitude(center - centerWrap.position);
            var relativeTargetPos = target.transform.position - center;
            var xz = Mathf.Cos(relativeTargetPos.x / r) * r;
            var yx = Mathf.Sin(relativeTargetPos.x / r) * r;
            relativeTargetPos = new Vector3(yx, relativeTargetPos.y, xz);
            target.transform.position = centerWrap.position + relativeTargetPos;
        }

        if (shouldTimeout)
            target.SetData(Target.TIMEOUT_KEY, StartCoroutine(TimeoutRoutine(target)));
    }

    private void StartGravitation()
    {
        foreach (var t in activeTargets)
        {
            if (t != null)
            {
                t.SetData(Target.MOVEMENT_KEY, StartCoroutine(Gravitate(t)));
            }
        }
    }

    private System.Collections.IEnumerator TargetRoutine(GameObject target)
    {
        if (shouldDelay)
        {
            // wait for the delayRoutine to enable the target
            StartCoroutine(DelayRoutine(target));
            while (!target.activeSelf)
                yield return null;
        }

        UpdateTarget(target);
    }
    private System.Collections.IEnumerator Gravitate(GameObject target)
    {
        var rb = target.GetComponent<Rigidbody>();

        var rd = UnityEngine.Random.insideUnitSphere.normalized;
        rb.velocity = rd * targetStartSpeed;

        while (target != null)
        {
            var dir = (targetContainer.position - target.transform.position).normalized;
            rb.AddForce(dir * targetAccelleration, ForceMode.Acceleration);

            if (rb.velocity.magnitude > targetMaxSpeed)
                rb.velocity = rb.velocity.normalized * targetMaxSpeed;

            yield return null;
        }
    }
    private System.Collections.IEnumerator HitMarker(GameObject target)
    {
        var hitobj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hitobj.GetRenderer().material = new Material(target.GetMaterial());
        hitobj.transform.position = target.transform.position;
        hitobj.transform.localScale = target.transform.localScale;

        float animTime = .4f;
        float scaleMultiplier = 2f;

        var scaleA = hitobj.transform.localScale;
        var scaleB = hitobj.transform.localScale * scaleMultiplier;
        var colorA = target.GetColor();
        var colorB = new Color(colorA.r, colorA.g, colorA.b, 0);

        var timeRemaining = animTime;
        var startTime = Time.time;
        while (timeRemaining > 0)
        {
            var ratio = 1 - timeRemaining / animTime;

            hitobj.SetColor(GameExtensions.LerpPlus(colorA, colorB, ratio));
            hitobj.transform.localScale = Vector3.Lerp(scaleA, scaleB, ratio);
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        Destroy(hitobj);
    }
    private System.Collections.IEnumerator TimeoutRoutine(GameObject target)
    {
        float startTime = Time.time;
        int t_num = target.GetData<int>(Target.ID_KEY);

        while (Time.time < startTime + timeoutTime)
            yield return null;

        bool sameTarget = target.GetData<int>(Target.ID_KEY) == t_num;
        if (sameTarget)
        {
            var co = target.GetData<Coroutine>(Target.TIMEOUT_KEY);
            StopCoroutine(co);

            StartCoroutine(TargetRoutine(target));
            // NextTarget(target);

            scoreboard.TargetTimeout();
        }
    }
    private System.Collections.IEnumerator DelayRoutine(GameObject target)
    {
        target.SetActive(false);

        float startTime = Time.time;
        float delayTime = UnityEngine.Random.Range(delayRange.x, delayRange.y);
        while (Time.time < startTime + delayTime)
            yield return null;

        target.SetActive(true);
    }

    public void StartFreePlay()
    {
        ResetTargets();
        scoreboard.StartFreePlay();
        StartGravitation();
    }
    public void StartChallenge()
    {
        ResetTargets();
        scoreboard.StartChallenge();
        StartGravitation();
    }

    public void ResetTargets()
    {
        ReGeneratePoints();
        ReSpawnTargets();
        SpawnConnectors();
    }
    public void ReSpawnTargets()
    {
        sphereNum = 0;

        targetContainer.DestroyChildren();


        if (targetPrefab != null)
        {
            // targetPrefab.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", targetStartColor);
            targetPrefab.SetActive(false);

            activeTargets = new List<GameObject>();

            // spawn targets
            for (int i = 0; i < spawnCount; i++)
            {
                SetupTarget();
            }
        }
    }

    private void SetupTarget()
    {
        // GameObject g = (GameObject)PrefabUtility.InstantiatePrefab(targetPrefab);
        GameObject g = Instantiate(targetPrefab, targetContainer);
        g.SetActive(true);
        activeTargets.Add(g);
        g.transform.localPosition = ConsumePoint();

        if (isTargetCapsule)
        {
            var ce = g.GetComponent<CapsuleExtender>();
            ce.SetRadius(targetRadius);
            ce.SetLength(capsuleTargetHeight);
        }
        else
        {
            g.transform.localScale = Vector3.one * targetRadius * 2;
        }

        g.SetData(Target.ID_KEY, sphereNum);
        g.SetData(Target.IS_TARGET_KEY, true);
        g.SetData(Target.SPAWN_TIME_KEY, Time.time);
        UpdateColor(g);
        sphereNum += 1;

        // var movementRoutine = Gravitate(g);
        // g.SetData(TARGET_MOVEMENT_KEY, movementRoutine);
    }

    public void SpawnConnectors()
    {
        if (connectorPrefab == null)
            return;

        if (connectors == null)
            connectors = new List<CapsuleExtender>();

        int numConnectors = activeTargets.Count - 1;

        ClearConnectors();
        for (int i = 0; i < numConnectors; i++)
        {
            CapsuleExtender c = Instantiate(connectorPrefab, connectorContainer);
            connectors.Add(c);
            c.transform.position = activeTargets[i].transform.position;
            c.Extend(activeTargets[i + 1].transform.position);

            Color lineColor = new Color(1, 1, 1, .2f);
            // Color col = Color.Lerp(playStartColor, playEndColor, ((float)i + 1) / numConnectors);
            SetColorRecursive(c.transform, lineColor);
        }
    }

    private void SetColorRecursive(Transform t, Color c)
    {
        t.gameObject.SetColor(c);
        foreach (var child in t.GetChildren())
            SetColorRecursive(child, c);
    }
    private void ClearConnectors()
    {
        // destroy gameobject
        Transform connectorContainer = transform.GetChild(1);
        if (connectorContainer.childCount > 0)
        {
            GameObject[] tempArr = new GameObject[connectorContainer.childCount];
            for (int i = 0; i < connectorContainer.childCount; i++)
                tempArr[i] = connectorContainer.GetChild(i).gameObject;
            foreach (GameObject g in tempArr)
                if (Application.isPlaying)
                    Destroy(g);
                else
                    DestroyImmediate(g);
        }
        // clear list references
        if (connectors != null)
            connectors.Clear();
    }
    private void UpdateLines()
    {
        if (connectors == null || connectors.Count == 0)
            return;

        int numConnectors = activeTargets.Count - 1;
        Transform connectorContainer = transform.GetChild(1);

        for (int i = 0; i < numConnectors; i++)
        {
            connectors[i].transform.position = activeTargets[i].transform.position;
            connectors[i].Extend(activeTargets[i + 1].transform.position);
        }
    }
    private Vector2 ConsumePoint()
    {
        Vector2 p = points[0] - regionSize / 2;
        points.RemoveAt(0);
        if (points.Count < minPoints)
        {
            Generate();

        }
        return p;
    }

    private void UpdateColor(GameObject target)
    {
        int colorIndex = (target.GetData<int>(Target.ID_KEY)
                       % (swapColorEvery * comboColors.Count)) / swapColorEvery;
        Color c = comboColors[colorIndex];

        if (isTargetCapsule)
        {
            target.GetComponent<CapsuleExtender>().SetColor(c);
        }
        else
        {
            target.SetColor(c);
        }
    }
}