using UnityEngine;
using Neat.Extensions;

namespace Neat.AimTrainer
{
    public class Target
    {
        public int id;
        public float spawnTime;
        public bool isTrackingSuccessful;
        public bool isTracking;
        public float trackStartTime;
        public float trackSuccessTime;
        public float trackAttemptTime;
        public float distance;
        public float playerDistanceMoved;

        // clicks attempted
        // clicks successful

        public static bool IsTarget(GameObject g)
        {
            return g != null && g.GetData<Target>() != null;
        }
        public static bool IsTarget(GameObject g, out Target target)
        {
            target = g.GetData<Target>();
            return target != null;
        }
    }
}