using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Neat.Guitar
{
    public class LaneObject
    {
        public GameObject gameObject;

        public int fret;
        public float time;
        public float length;

        public LaneObject(GameObject g, int fret, float time, float length)
        {
            this.gameObject = g;
            this.fret = fret;
            this.time = time;
            this.length = length;
        }
    }
}
