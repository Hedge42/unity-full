using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour
{
    private LineRenderer lr;
    public Transform t;

    public float distance = 10f;

    public float xAngle = 20f;
    public float yAngle = 10f;

    public int numPoints = 10;

    public void DrawLines(float x, float y)
    {
        xAngle = x * 2;
        yAngle = y * 2;

        transform.position = Camera.main.transform.position;
        transform.rotation = Camera.main.transform.rotation;

        lr = GetComponent<LineRenderer>();
        var posCount = numPoints * 2;
        lr.positionCount = posCount;
        lr.SetPositions(GetPositions());
    }

    private Vector3[] GetPositions()
    {
        List<Vector3> points = new List<Vector3>();


        // bottom right
        t.localEulerAngles = new Vector3(-xAngle / 2, yAngle / 2, 0);
        var delta = yAngle / numPoints;

        // to bottom left
        for (int i = 0; i < numPoints - 1; i++)
        {
            // add point at this point * distance
            points.Add(t.forward * distance);

            var a = t.localEulerAngles;
            t.localEulerAngles = new Vector3(a.x, a.y - delta, 0);
        }

        // bottom left
        t.localEulerAngles = new Vector3(-xAngle / 2, -yAngle / 2, 0);
        points.Add(t.forward * distance);

        // top left
        t.localEulerAngles = new Vector3(xAngle / 2, -yAngle / 2, 0);

        // to top right
        for (int i = 0; i < numPoints - 1; i++)
        {
            points.Add(t.forward * distance);

            var a = t.localEulerAngles;
            t.localEulerAngles = new Vector3(a.x, a.y + delta, 0);
        }

        // top right
        t.localEulerAngles = new Vector3(xAngle / 2, yAngle / 2, 0);
        points.Add(t.forward * distance);

        // back to top left
        // t.localEulerAngles = new Vector3(-xAngle / 2, )

        t.localEulerAngles = Vector3.zero;

        return points.ToArray();
    }
}
