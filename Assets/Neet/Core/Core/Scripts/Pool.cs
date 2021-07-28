using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Misc_Scripts
{
    public class Pool : MonoBehaviour
    {
        [Range(1, 100)]
        public int max;

        public GameObject prefab;
        public bool spawnOnStart;
        public bool increaseMaxOnNoneAvailable;

        private Queue<GameObject> available;
        private List<GameObject> active;

        private void Awake()
        {
            available = new Queue<GameObject>();
            active = new List<GameObject>();
        }

        public GameObject GetNext()
        {
            if (available.Count > 0)
            {
                GameObject next = available.Dequeue();
                active.Add(next);
                return next;
            }
            else
            {

            }

            return null;
        }
    }
}
