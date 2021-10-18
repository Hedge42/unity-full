using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    public Motor m;
    public bool active;
    public float speed;

    [Space]
    public KeyCode xPos;
    public KeyCode xNeg;
    public KeyCode yPos;
    public KeyCode yNeg;
    public KeyCode zPos;
    public KeyCode zNeg;

    private void Awake()
    {
        if (m == null)
            m = GetComponent<Motor>();
    }

    private void Update()
    {
        if (!active)
            return;

        Vector3 input = Vector3.zero;
        if (Input.GetKey(xPos))
            input += Vector3.right;
        if (Input.GetKey(xNeg))
            input -= Vector3.right;
        if (Input.GetKey(yPos))
            input += Vector3.up;
        if (Input.GetKey(yNeg))
            input -= Vector3.up;
        if (Input.GetKey(zPos))
            input += Vector3.forward;
        if (Input.GetKey(zNeg))
            input -= Vector3.forward;
        input = input.normalized;
    }
}
