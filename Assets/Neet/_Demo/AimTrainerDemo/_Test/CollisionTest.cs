using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    public Rigidbody obj;
    public float speed;

    private Vector3 startPos;

    private void Start()
    {
        startPos = obj.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            obj.transform.position = startPos;
        }
    }

    private void FixedUpdate()
    {
        obj.transform.position += Vector3.right * speed * Time.fixedDeltaTime;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}
