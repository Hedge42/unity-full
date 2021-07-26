using System;
using System.Collections.Generic;
using UnityEngine;

// Adapated from java source by Herman Tulleken
// http://www.luma.co.za/labs/2008/02/27/poisson-disk-sampling/

// The algorithm is from the "Fast Poisson Disk Sampling in Arbitrary Dimensions" paper by Robert Bridson
// http://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf

// code taken from 
// http://theinstructionlimit.com/fast-uniform-poisson-disk-sampling-in-c

public static class UniformPoissonDiskSampler
{
    public const int DefaultPointsPerIteration = 30;

    static readonly float SquareRootTwo = (float)Math.Sqrt(2);

    public static State currentState;
    public static Settings currentSettings;

    public struct Settings
    {
        public Vector2 TopLeft, LowerRight, Center;
        public Vector2 Dimensions;
        public float? RejectionSqDistance;
        public float MinimumDistance;
        public float MaxDistance;
        public float CellSize;
        public int GridWidth, GridHeight;
    }

    public class State
    {
        public Vector2?[,] Grid;
        public List<Vector2> ActivePoints, Points, bufferPoints, newPoints;
    }

    public static List<Vector2> SampleCircle(Vector2 center, float radius, float minimumDistance)
    {
        return SampleCircle(center, radius, minimumDistance, DefaultPointsPerIteration);
    }
    public static List<Vector2> SampleCircle(Vector2 center, float radius, float minimumDistance, int pointsPerIteration)
    {
        //return Sample(center - new Vector2(radius), center + new Vector2(radius), radius, minimumDistance, pointsPerIteration);
        return Sample(center - Vector2.one * radius, center + Vector2.one * radius, radius, minimumDistance, minimumDistance * 2, pointsPerIteration);
    }

    public static List<Vector2> SampleRectangle(Vector2 topLeft, Vector2 lowerRight, float minimumDistance, float maxDistance)
    {
        return SampleRectangle(topLeft, lowerRight, minimumDistance, maxDistance, DefaultPointsPerIteration);
    }
    public static List<Vector2> SampleRectangle(Vector2 topLeft, Vector2 lowerRight, float minimumDistance, int pointsPerIteration)
    {
        return Sample(topLeft, lowerRight, null, minimumDistance, minimumDistance * 2, pointsPerIteration);
    }
    public static List<Vector2> SampleRectangle(Vector2 topLeft, Vector2 lowerRight, float minimumDistance, float maximumDistance, int pointsPerIteration, int numPoints = 0, State existing = null)
    {
        return Sample(topLeft, lowerRight, null, minimumDistance, maximumDistance, pointsPerIteration, numPoints, existing);
    }

    static List<Vector2> Sample(Vector2 topLeft, Vector2 lowerRight, float? rejectionDistance, float minimumDistance, float maxDistance, int pointsPerIteration, int numPoints = 0, State existing = null)
    {
        var settings = new Settings
        {
            TopLeft = topLeft,
            LowerRight = lowerRight,
            Dimensions = lowerRight - topLeft,
            Center = (topLeft + lowerRight) / 2,
            CellSize = minimumDistance / SquareRootTwo,
            MinimumDistance = minimumDistance,
            MaxDistance = maxDistance,
            RejectionSqDistance = rejectionDistance == null ? null : rejectionDistance * rejectionDistance
        };
        settings.GridWidth = (int)(settings.Dimensions.x / settings.CellSize) + 1;
        settings.GridHeight = (int)(settings.Dimensions.y / settings.CellSize) + 1;

        var state = new State
        {
            Grid = new Vector2?[settings.GridWidth, settings.GridHeight],
            ActivePoints = new List<Vector2>(),
            Points = new List<Vector2>(),
            bufferPoints = new List<Vector2>(),
            newPoints = new List<Vector2>()
        };

        if (existing == null)
        {
            AddFirstPoint(ref settings, ref state);
        }
        else
        {
            AddExisting(existing.Points, ref settings, ref state);
            // AddNextPoint(existing.Points[existing.Points.Count - 1], ref settings, ref state, existing);
        }


        while (state.ActivePoints.Count != 0)
        {
            if (numPoints > 0 && state.newPoints.Count >= numPoints)
                break;

            var listIndex = RandomHelper.Random.Next(state.ActivePoints.Count);

            var point = state.ActivePoints[listIndex];
            var found = false;

            for (var k = 0; k < pointsPerIteration; k++)
            {
                found |= AddNextPoint(point, ref settings, ref state, existing);

                if (found && numPoints > 0 && state.newPoints.Count >= numPoints)
                    break;
            }

            if (!found)
                state.ActivePoints.RemoveAt(listIndex);
        }

        currentState = state;
        currentSettings = settings;


        return state.newPoints;
    }

    static void AddFirstPoint(ref Settings settings, ref State state)
    {
        var added = false;
        while (!added)
        {
            var d = RandomHelper.Random.NextDouble();
            var xr = settings.TopLeft.x + settings.Dimensions.x * d;

            d = RandomHelper.Random.NextDouble();
            var yr = settings.TopLeft.y + settings.Dimensions.y * d;

            var p = new Vector2((float)xr, (float)yr);
            if (settings.RejectionSqDistance != null && Mathf.Pow(Vector2.Distance(settings.Center, p), 2) > settings.RejectionSqDistance)
                continue;
            added = true;

            var index = Denormalize(p, settings.TopLeft, settings.CellSize);

            state.Grid[(int)index.x, (int)index.y] = p;
            state.ActivePoints.Add(p);
            state.Points.Add(p);
        }
    }

    static void AddExisting(List<Vector2> points, ref Settings settings, ref State state)
    {
        foreach (var p in points)
        {
            var index = Denormalize(p, settings.TopLeft, settings.CellSize);
            state.Grid[(int)index.x, (int)index.y] = p;
            state.ActivePoints.Add(p);
            state.Points.Add(p);
            state.bufferPoints.Add(p);
        }
    }

    static bool AddNextPoint(Vector2 point, ref Settings settings, ref State state, State bufferState = null)
    {
        var found = false;
        var q = GenerateRandomAround(point, settings.MinimumDistance, settings.MaxDistance);

        if (q.x >= settings.TopLeft.x && q.x < settings.LowerRight.x &&
            q.y > settings.TopLeft.y && q.y < settings.LowerRight.y &&
            (settings.RejectionSqDistance == null || Mathf.Pow(Vector2.Distance(settings.Center, q), 2) <= settings.RejectionSqDistance))
        {
            var qIndex = Denormalize(q, settings.TopLeft, settings.CellSize);
            bool invalid = invalidPoint(settings, state, q, qIndex, bufferState);

            if (!invalid)
            {
                found = true;
                state.ActivePoints.Add(q);
                state.Points.Add(q);
                state.Grid[(int)qIndex.x, (int)qIndex.y] = q;
                state.newPoints.Add(q);
            }
        }
        return found;
    }

    private static bool invalidPoint(Settings settings, State state, Vector2 q, Vector2 qIndex, State bufferState = null)
    {
        var invalid = false;

        for (var i = (int)Math.Max(0, qIndex.x - 2); i < Math.Min(settings.GridWidth, qIndex.x + 3) && !invalid; i++)
            for (var j = (int)Math.Max(0, qIndex.y - 2); j < Math.Min(settings.GridHeight, qIndex.y + 3) && !invalid; j++)
            {
                if (state.Grid[i, j].HasValue && Vector2.Distance(state.Grid[i, j].Value, q) < settings.MinimumDistance)
                    invalid = true;
                else if (state.Grid[i, j].HasValue && Vector2.Distance(state.Grid[i, j].Value, q) > settings.MaxDistance)
                    invalid = true;

                if (bufferState != null)
                {
                    if (bufferState.Grid[i, j].HasValue && Vector2.Distance(bufferState.Grid[i, j].Value, q) < settings.MinimumDistance)
                        invalid = true;
                    else if (bufferState.Grid[i, j].HasValue && Vector2.Distance(bufferState.Grid[i, j].Value, q) > settings.MaxDistance)
                        invalid = true;
                }
            }

        return invalid;
    }

    static Vector2 GenerateRandomAround(Vector2 center, float minimumDistance, float maxDistance)
    {
        var d = RandomHelper.Random.NextDouble();
        var radius = Mathf.Lerp(minimumDistance, maxDistance, (float)d);

        d = RandomHelper.Random.NextDouble();
        var angle = MathHelper.TwoPi * d;

        var newX = radius * Math.Sin(angle);
        var newY = radius * Math.Cos(angle);

        return new Vector2((float)(center.x + newX), (float)(center.y + newY));
    }

    public static Vector2 Denormalize(Vector2 point, Vector2 origin, double cellSize)
    {
        return new Vector2((int)((point.x - origin.x) / cellSize), (int)((point.y - origin.y) / cellSize));
    }
}

public static class RandomHelper
{
    public static readonly System.Random Random = new System.Random();
}

public static class MathHelper
{
    public const float Pi = (float)Math.PI;
    public const float HalfPi = (float)(Math.PI / 2);
    public const float TwoPi = (float)(Math.PI * 2);
}
