using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformIgnorer : MonoBehaviour
{
    public bool position;
    public bool rotation;
    public bool scale;

    private void Update()
    {
        Fix();
    }

    void Fix()
    {
        if (position)
            transform.localPosition = transform.parent.localPosition * -1;
        if (rotation)
        {
            //transform.localRotation = Quaternion.Inverse(transform.parent.parent.parent.localRotation);
            transform.localRotation = Quaternion.Inverse(Camera.main.transform.rotation);
        }
        if (scale)
        {
            Vector3 p = transform.parent.localScale;
            transform.localScale = new Vector3(1 / p.x, 1 / p.y, 1 / p.z);
        }
    }
}
