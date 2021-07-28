using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.Extensions;
public class PointGenerator
{
    public Vector2 regionSize;
    public float minDistance;
    public float maxDistance;
    public int maxPoints = 10;
    public int maxTries = 100;
    public int avoidCount;
    public float avoidDistance;
    public float centerBias = 0f;
    public bool centerOrigin = true;

    public float maxAngle;
    public float minAngle;



    List<Vector2> points;

    private int outsideRegionCount = 0;
    private int tooCloseCount = 0;

    public List<Vector2> GeneratePoints(List<Vector2> existing)
    {
        if (existing == null)
            existing = new List<Vector2>();
        points = existing;

        for (int i = 0; i < points.Count; i++)
            points[i] -= regionSize / 2;

        if (points.Count == 0)
            points.Add(Vector2.zero);


        int numGenerated = 0;
        while (points.Count < maxPoints)
        {
            if (NextPoint(out Vector2 point))
            {
                points.Add(point);
                numGenerated += 1;
            }
            else
            {
                Debug.LogWarning("Couldn't find valid points after " 
                    + outsideRegionCount + " points outside region and "
                    + tooCloseCount + " points too close to others");
                tooCloseCount = outsideRegionCount = 0;
                break;
            }
        }

        if (centerOrigin)
            for (int i = 0; i < points.Count; i++)
                points[i] += regionSize / 2;

        return points;
    }
    private bool NextPoint(out Vector2 point)
    {
        int tries = 0;
        while (true)
        {
            // NEW
            var rAngle = Random.Range(minAngle, maxAngle);

            // should already be normalized?
            var dir = new Vector2(Mathf.Cos(rAngle), Mathf.Sin(rAngle));
            if (Random.Range(0f, 1f) <= .5f)
                dir = new Vector2(-dir.x, dir.y);
            if (Random.Range(0f, 1f) <= .5f)
                dir = new Vector2(dir.x, -dir.y);
            var localPoint = dir.normalized * Random.Range(minDistance, maxDistance);
            point = points.Last() + localPoint;


            //if (!float.Equals(0, centerBias))
            //{
            //    float distToCenter = point.magnitude;
            //    Vector2 bias = distToCenter * point * centerBias * -1;
            //    rv = (rv + bias).Clamp(minDistance, maxDistance);
            //    point = points.Last() + rv;
            //}


            if (isValid(point))
                return true;

            tries += 1;
            if (tries > maxTries)
            {
                point = Vector2.zero;
                return false;
            }
        }
    }
    private bool isValid(Vector2 point)
    {
        // is the point outside of the region?
        if (point.x.Abs() > regionSize.x / 2 || point.y.Abs() > regionSize.y / 2)
        {
            outsideRegionCount += 1;
            return false;
        }

        // is the point too close to previous points?
        for (int i = points.LastIndex(); i >= points.LastIndex() - avoidCount && i >= 0; i--)
        {
            if (point.DistanceTo(points[i]) < avoidDistance)
            {
                tooCloseCount += 1;
                return false;
            }
        }
        return true;
    }


    public bool NextPoint(out Vector2 point, List<Vector2> existing)
    {
        if (existing == null)
            existing = new List<Vector2>();
        if (existing.Count == 0)
        {
            point = Vector2.zero;
            return true;
        }

        int tries = 0;
        while (true)
        {
            var rAngle = Random.Range(minAngle, maxAngle);
            var dir = new Vector2(Mathf.Cos(rAngle), Mathf.Sin(rAngle));
            if (Random.Range(0f, 1f) <= .5f) dir = new Vector2(-dir.x, dir.y);
            if (Random.Range(0f, 1f) <= .5f) dir = new Vector2(dir.x, -dir.y);
            var localPoint = dir.normalized * Random.Range(minDistance, maxDistance);
            point = existing.Last() + localPoint;

            if (isValid(point, existing))
                return true;

            tries += 1;
            if (tries > maxTries)
            {
                point = Vector2.zero;
                return false;
            }
        }
    }
    private bool isValid(Vector2 point, List<Vector2> existing)
    {
        // is the point outside of the region?
        if (point.x.Abs() > regionSize.x / 2 || point.y.Abs() > regionSize.y / 2)
        {
            outsideRegionCount += 1;
            return false;
        }

        // is the point too close to previous points?
        //for (int i = points.LastIndex(); i >= points.LastIndex() - avoidCount && i >= 0; i--)
        foreach (var v in existing)
        {
            if (point.DistanceTo(v) < avoidDistance)
            {
                tooCloseCount += 1;
                return false;
            }
        }
        return true;
    }
}
