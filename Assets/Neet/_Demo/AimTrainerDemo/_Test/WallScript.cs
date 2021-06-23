using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.Data;
using Neet.Events;

public class WallScript : MonoBehaviour
{
    private float wallDistance;
    public GameObject targetPrefab;
    public GameObject currentTarget;

    private PresetProfile profile => PresetProfile.current;
    private AimProfile aim => profile.aimProfile;
    private TrackingProfile tracking => profile.trackingProfile;
    private ColorProfile colors => profile.colorProfile;
    private TimingProfile timing => profile.timingProfile;
    private Motor motor;
    private MouseRotator rotator;
    private Transform cam;

    private Rigidbody rb;

    private const float targetDistance = 26.48f; // ???

    private Vector3 targetLocalPos;
    private Rigidbody rbCam;


    private void OnValidate()
    {
        // KeepDistance(distance, cam);
    }

    private void Start()
    {
        if (PresetProfile.current == null)
            PresetCollection.Load();

        rb = GetComponent<Rigidbody>();
        motor = Player.main.GetComponent<Motor>();
        cam = CameraController.active.transform;
        rbCam = cam.GetComponent<Rigidbody>();
        rotator = Player.main.GetComponent<MouseRotator>();
        motor.ReadMovementProfile(profile.movementProfile);
        motor.SetDefaultKeys();
        motor.IsInputActive = true;
        rotator.Toggle(true);
        wallDistance = transform.position.z;

        // targetLocalPos = transform.InverseTransformPoint(currentTarget.transform.position);
    }

    private void Update()
    {
        motor.ProcessProfile(profile.movementProfile);

        UpdateWall();
        // UpdateWallRB();

        HandleRaycast();
    }
    private void FixedUpdate()
    {
        // UpdateWall();
        // UpdateWallRB();
    }
    private void SpawnTarget()
    {
        // to prevent a double spawn
        if (currentTarget != null)
            return;

        var target = Instantiate(targetPrefab, transform);
        target.SetActive(true);
        target.transform.localScale = Vector3.one * aim.radius * 2;
        target.SetColor(colors.targetColor);
        SetTargetPosition(target);
        currentTarget = target;

        target.SetData(Target.IS_TARGET_KEY, true);
    }
    private void SetTargetPosition(GameObject target)
    {
        target.transform.position = cam.transform.position + Vector3.forward * targetDistance;

    }

    private void UpdateWall()
    {
        LookAway(cam);
        KeepDistance(wallDistance, cam);
    }
    private void KeepDistance(float distance, Transform from)
    {
        // https://answers.unity.com/questions/292084/keeping-distance-between-two-gameobjects.html
        transform.position = (transform.position - from.position)
            .normalized * distance + from.position;
    }
    private void LookAway(Transform from)
    {
        // https://answers.unity.com/questions/43001/looking-away-from-a-target.html
        Vector3 direction = transform.position - from.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void UpdateWallRB()
    {
        KeepDistanceRB(wallDistance, rbCam);
        LookAwayRB(rbCam);
    }
    private void KeepDistanceRB(float distance, Rigidbody from)
    {
        // https://answers.unity.com/questions/292084/keeping-distance-between-two-gameobjects.html
        rb.position = (rb.position - from.position)
            .normalized * distance + from.position;
    }
    private void LookAwayRB(Rigidbody from)
    {
        // https://answers.unity.com/questions/43001/looking-away-from-a-target.html
        Vector3 direction = rb.position - from.position;
        rb.rotation = Quaternion.LookRotation(direction);
    }

    void HandleRaycast()
    {
        Physics.Raycast(rbCam.position, rbCam.transform.forward, out RaycastHit rh);
        var col = rh.collider;
        if (col != null)
            col.gameObject.SetColor(Color.red);
        else
            currentTarget.SetColor(Color.white);
        Debug.DrawLine(transform.position, transform.forward * 1000, Color.cyan);
    }
}
