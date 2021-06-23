using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject target;
    public Rigidbody player;
    public float playerSpeed = 100f;
    public float wallDistance = 600f;
    public float targetDistance = 30f;

    private MouseRotator rotator;
    private Vector3 movementInput;
    private Camera playerCam;
    private Rigidbody rb;

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody>();

        KeepDistance(wallDistance, player.transform);
        transform.LookAt(player.transform);
        SetTargetDistance();
    }

    private void Start()
    {
        rotator = player.GetComponent<MouseRotator>();
        rotator.Toggle(true);
        rb = GetComponent<Rigidbody>();
        playerCam = player.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        GetMovementInput();
        DebugRaycast();
    }
    private void FixedUpdate()
    {
        ApplyMovement();
        UpdateWall();
        StartCoroutine(_LateFixedUpdate());
    }
    private void LateUpdate()
    {
    }
    private void LateFixedUpdate()
    {
    }
    private IEnumerator _LateFixedUpdate()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime / 2f);
        LateFixedUpdate();
    }
    private void SetTargetDistance()
    {
        target.transform.position = player.transform.forward * targetDistance;

    }

    private void UpdateWall()
    {
        // var targetLocalPos = transform.InverseTransformPoint(target.transform.position);

        KeepDistance(wallDistance, player.transform);
        LookAt(player.transform);

        // var newTargetPos = transform.TransformPoint(targetLocalPos);
        // target.transform.position = newTargetPos;
        // target.GetComponent<Rigidbody>().position = newTargetPos; 
    }

    private void KeepDistance(float distance, Transform from)
    {
        // https://answers.unity.com/questions/292084/keeping-distance-between-two-gameobjects.html
        var newPos = (transform.position - from.position)
            .normalized * distance + from.position;

        transform.position = newPos;
        // rb.position = newPos;
    }
    private void LookAway(Transform from)
    {
        // https://answers.unity.com/questions/43001/looking-away-from-a-target.html
        Vector3 direction = transform.position - from.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
    private void LookAt(Transform at)
    {
        Vector3 dir = at.position - transform.position;

        transform.rotation = Quaternion.LookRotation(dir);
        // rb.rotation = Quaternion.LookRotation(dir);
    }

    private void GetMovementInput()
    {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
            input.x -= 1;
        if (Input.GetKey(KeyCode.D))
            input.x += 1;

        movementInput = input;
    }
    private void ApplyMovement()
    {
        //player.AddForce(player.rotation * movementInput * playerSpeed, Force);


        var velocity = player.rotation * movementInput * playerSpeed;
        var translation = velocity * Time.fixedDeltaTime;


        // player.velocity = velocity; 
        player.transform.position += translation;
    }

    private void DebugRaycast()
    {
        Physics.Raycast(playerCam.transform.position, playerCam.transform.forward,
            out RaycastHit rh);

        GameObject hit = rh.collider?.gameObject;

        if (hit != null)
        {
            target.SetColor(Color.red);
        }
        else
        {
            target.SetColor(Color.white);
        }
    }
}
