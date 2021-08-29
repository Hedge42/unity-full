using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Extensions;
using Neat.FileManagement;

public class CapsuleExtender : MonoBehaviour
{
    public Transform anchor;
    public Transform head;
    public Transform body;

    public float length;
    public float radius;

    public bool isCenterOrigin;
    public bool includeEndsInLength;

    public Material mat;

    private void OnValidate()
    {
        var rot = transform.GetChild(0);
        if (anchor == null)
            anchor = rot.Find("Anchor");
        if (head == null)
            head = rot.Find("Head");
        if (body == null)
            body = rot.Find("Body");
        if (anchor == null || head == null || body == null)
            Debug.LogWarning("Capsule can't find components", gameObject);

        anchor.GetComponent<MeshRenderer>().material = mat;
        head.GetComponent<MeshRenderer>().material = mat;
        body.GetComponent<MeshRenderer>().material = mat;

        UpdateSize();   
    }

    public void Extend(Vector3 target)
    {
        length = Vector3.Distance(transform.position, target);
        UpdateSize();

        transform.LookAt(target);
    }
    void UpdateSize()
    {
        // length
        float yScale = length / 2;
        if (includeEndsInLength)
            yScale -= radius / 2;

        body.localScale = new Vector3(body.localScale.x, yScale, body.localScale.z);

        // width and depth
        head.localScale = new Vector3(radius, radius, radius);
        anchor.localScale = new Vector3(radius, radius, radius);
        body.localScale = new Vector3(radius, body.localScale.y, radius);

        // change position of head and body
        var headPos = length;
        if (includeEndsInLength)
            headPos -= radius;
        head.transform.localPosition = new Vector3(0, headPos, 0);
        body.transform.localPosition = new Vector3(0, yScale, 0);

        if (isCenterOrigin)
            transform.GetChild(0).localPosition = new Vector3(0, -yScale, 0);
        else
            transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
    }

    public void SetRadius(float _radius)
    {
        radius = _radius;
        UpdateSize();
    }
    public void SetLength(float _length)
    {
        length = _length;
        UpdateSize();
    }

    // used to get a position t% between the base and end
    public Vector3 GetInterpolatedPosition(float t)
    {
        Vector3 start = anchor.position;
        Vector3 end = head.position;

        return Vector3.Lerp(start, end, t);
    }

    public void SetColor(Color c)
    {
        body.gameObject.SetColor(c);
        head.gameObject.SetColor(c);
        anchor.gameObject.SetColor(c);
    }
}
