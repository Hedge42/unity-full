using Neat.DDD;
using Neat.InputHelpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Neat.DDD.FPSController))]
public class FPSController_Move : MonoBehaviour
{
    private FPSController controller;

    private Keybinds.FPSActions controls;
    private Rigidbody rb => controller.rb;

    [Range(0f, 100f)]
    public float moveScale;

    [Range(0f, 50f)]
    public float runSpeed;

    private Vector3 input;

    private void Awake()
    {
        controller = GetComponent<FPSController>();
        controls = GetComponent<KeybindsComponent>().keybinds.FPS;
    }
    private void Update()
    {
        input = controls.Move3D.ReadValue<Vector3>();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        var forceVector = input * moveScale;
        rb.AddRelativeForce(forceVector, ForceMode.Acceleration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, runSpeed);
    }
}
