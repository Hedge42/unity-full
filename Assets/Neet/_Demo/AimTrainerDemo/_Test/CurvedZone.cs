using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedZone : MonoBehaviour
{
    private LineRenderer lr;

    public float distance = 10f;
    public float otherDistance = 10f;

    public float _xAngle = 20f;
    public float _yAngle = 10f;

    public int numPoints = 10;

    public void DrawLines(float x, float y)
    {
        Transform cam = Camera.main.transform;

        transform.position = cam.position;
        transform.rotation = cam.rotation;

        lr = GetComponent<LineRenderer>();
        var posCount = numPoints * 2;
        lr.positionCount = posCount;
        lr.SetPositions(GetPositions(cam, x * 2, y * 2));
    }

    private Vector3[] GetPositions(Transform t, float xAngle, float yAngle)
    {
        List<Vector3> points = new List<Vector3>();

        var startRot = t.transform.rotation;

        // bottom right
        t.localEulerAngles = new Vector3(-xAngle / 2, yAngle / 2, 0);
        var delta = yAngle / numPoints;

        // to bottom left
        for (int i = 0; i < numPoints - 1; i++)
        {
            // add point at this point * distance
            points.Add(t.position + t.forward * distance);

            var a = t.localEulerAngles;
            t.localEulerAngles = new Vector3(a.x, a.y - delta, 0);
        }

        // bottom left
        t.localEulerAngles = new Vector3(-xAngle / 2, -yAngle / 2, 0);
        points.Add(t.position + t.forward * distance);

        // top left
        t.localEulerAngles = new Vector3(xAngle / 2, -yAngle / 2, 0);

        // to top right
        for (int i = 0; i < numPoints - 1; i++)
        {
            points.Add(t.position + t.forward * distance);

            var a = t.localEulerAngles;
            t.localEulerAngles = new Vector3(a.x, a.y + delta, 0);
        }

        // top right
        t.localEulerAngles = new Vector3(xAngle / 2, yAngle / 2, 0);
        points.Add(t.position + t.forward * distance);

        // back to top left
        // t.localEulerAngles = new Vector3(-xAngle / 2, )

        t.rotation = startRot;

        return points.ToArray();
    }
}
