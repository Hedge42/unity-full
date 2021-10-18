using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformScaler : MonoBehaviour
{
    public enum LockType
    {
        XY,
        YZ,
        XZ,
        XYZ,
        none,
    }

    public LockType lockType;
    public float uniformScale;

    private void OnValidate()
    {
        SetScale();
    }

    private void Start()
    {
        SetScale();
    }

    private void SetScale()
    {
        if (lockType == LockType.XY)
            transform.localScale = new Vector3(uniformScale, uniformScale, transform.localScale.z);
        else if (lockType == LockType.YZ)
            transform.localScale = new Vector3(transform.localScale.x, uniformScale, uniformScale);
        else if (lockType == LockType.XZ)
            transform.localScale = new Vector3(uniformScale, transform.localScale.y, uniformScale);
        else if (lockType == LockType.XYZ)
            transform.localScale = new Vector3(uniformScale, uniformScale, uniformScale);
        else
        {
            // locktype.none
        }
    }
}
