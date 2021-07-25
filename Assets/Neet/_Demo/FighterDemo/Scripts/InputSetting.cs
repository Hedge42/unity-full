using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Fighter
{
    [System.Serializable]
    public class InputSetting
    {
        public KeyCode punch;
        public KeyCode kick;
        public KeyCode special;
        public KeyCode guard;

        public KeyCode up;
        public KeyCode down;
        public KeyCode left;
        public KeyCode right;

        public InputSetting()
        {
            punch = KeyCode.J;
            kick = KeyCode.N;
            special = KeyCode.K;
            guard = KeyCode.M;

            up = KeyCode.Space;
            down = KeyCode.S;
            left = KeyCode.A;
            right = KeyCode.D;
        }
    }
}
