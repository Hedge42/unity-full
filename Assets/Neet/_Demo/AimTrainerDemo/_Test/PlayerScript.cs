using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 input;
    private MouseRotator rotator;

    public GameObject target;

    private Camera cam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rotator = GetComponent<MouseRotator>();
        cam = GetComponentInChildren<Camera>();
    }
    private void Start()
    {
        rotator.Toggle(true);
    }

    private void Update()
    {
        GetInput();
        HandleRaycast();
    }
    private void FixedUpdate()
    {
        ApplyInput();
    }

    void ApplyInput()
    {
        var pos = transform.position;
        float speed = 500;
        rb.velocity = transform.rotation * input * speed;
        // print("moved " + Vector3.Distance(pos, transform.position));
    }

    void GetInput()
    {
        Vector3 _input = Vector3.zero;

        if (Input.GetKey(KeyCode.D))
            _input.x += 1;
        if (Input.GetKey(KeyCode.A))
            _input.x -= 1;

        input = _input;
    }

    void HandleRaycast()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Physics.Raycast(transform.position, transform.forward, out RaycastHit rh);
            var col = rh.collider;
            if (col != null)
                col.gameObject.SetColor(Color.red);
            else
                target.gameObject.SetColor(Color.white);
            Debug.DrawLine(transform.position, transform.forward * 1000, Color.cyan);
        }
    }
}
