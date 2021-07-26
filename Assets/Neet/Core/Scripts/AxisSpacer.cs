using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisSpacer : MonoBehaviour
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    public float spacing;
    public Axis a;

    void OnValidate()
    {
        FixSpacing();
    }
    private void Awake()
    {
        FixSpacing();
    }

    public void FixSpacing()
    {
        Transform[] children = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }

        Vector3 baseVec = BaseVector();

        for (int i = 0; i < children.Length; i++)
        {
            children[i].localPosition = baseVec * i * spacing;
        }
    }

    private Vector3 BaseVector()
    {
        if (a == Axis.X)
            return Vector3.right;
        else if (a == Axis.Y)
            return Vector3.up;
        else
            return Vector3.forward;
    }
}
