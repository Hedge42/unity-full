using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Demos.Fighter
{
    public class BtnRequirement
    {
        public int btnDown;
        public int btnHoldable;

        public BtnRequirement(string btn)
        {
            bool holdable = false;

            for (int i = btn.Length - 1; i >= 0; i--)
            {
                if (btn[i] == '*')
                {
                    holdable = true;
                    continue;
                }
                else
                {
                    int btnValue = Fighter.BtnToBitValue(btn[i]);

                    if (holdable)
                        btnHoldable |= btnValue;
                    else
                        btnDown |= btnValue;
                }
            }
        }
    }
}
