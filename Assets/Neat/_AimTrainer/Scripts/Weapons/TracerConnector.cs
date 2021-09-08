using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerConnector : MonoBehaviour
{
    // stretch to last tracer
    private void OnValidate()
    {
        // GetComponent<Renderer>().sharedMaterial.color = transform.parent.GetComponent<Renderer>().sharedMaterial.color;
    }

    public void StretchTo(Vector3 position)
    {
        Vector3 difference = position - transform.position;
        float distance = Vector3.Distance(transform.parent.position, position);
        transform.localScale = 
            new Vector3(transform.localScale.x, distance, transform.localScale.z);
        transform.parent.LookAt(position);
        transform.position = (transform.position + position) / 2;
    }
}
