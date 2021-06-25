using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTest : MonoBehaviour
{
    public GameObject target;

    public float size;

    [Range(2, 300)]
    public float distance;

    private void OnValidate()
    {
        target.transform.localScale = Vector3.one * size;
        target.transform.position = transform.position + transform.forward * distance;
    }
}
