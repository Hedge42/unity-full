using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Fighter
{
    public class DirRequirement
    {
        public int dir;
        public bool isHoldable;
        public bool isOptional;

        public DirRequirement(string s)
        {
            isHoldable = s.Contains("*");
            isOptional = s.Contains("?");

            foreach (char ch in s)
            {
                if (int.TryParse(ch.ToString(), out int result))
                    dir |= result;
            }
        }
    }
}