using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    // miss?
    Gun g;

    private void Awake()
    {
        g = GetComponentInParent<Gun>();

        g.onRaycast += ShotFiredHandler;
    }
    void ShotFiredHandler(RaycastHit rh)
    {
        Collider c = rh.collider;
        Vector3 target;
        if (c == null)
        {
            // get the location 10 units in front of the face
            target = transform.position;
        }
        else
        {
            // get the point of impact
            target = rh.point;
        }
    }
}
