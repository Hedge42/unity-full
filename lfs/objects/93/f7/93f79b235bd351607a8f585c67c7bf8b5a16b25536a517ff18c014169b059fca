using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IKJointType
{
    Hip,
    Spine,
    Head,
    LeftHand,
    RightHand,
    LeftFoot,
    RightFoot,
}

public class IKTarget : MonoBehaviour
{
    public IKJointType targetJoint;

    public float radius;
    public Color color;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
