using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Audio.Music
{
    public class SnappingOverrider : MonoBehaviour
    {
        public enum Setting
        {
            Beat, // draw #div timings every bar
            Bar, // draw #div timings every bar
            Off // draws numerators, default
        }
        public Setting setting;
        [Range(1, 16)]
        public int div;

        void SetSnapping(Setting per, int count)
        {
            // value within accepted range?

            // what does this actually need to update?
            // 1. BeatDrawer

            if (per == Setting.Bar)
            {

            }
        }
    }
}
