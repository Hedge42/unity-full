using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Guitar
{
    public class TimeMarker
    {
        public GameObject gameObject;
        public float time;

        public TimeMarker(GameObject gameObject, float time)
        {
            this.time = time;
            this.gameObject = gameObject;
        }
    }
}
