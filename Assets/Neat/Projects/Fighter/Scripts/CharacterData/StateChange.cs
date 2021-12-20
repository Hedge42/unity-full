using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Demos.Fighter
{
    [System.Serializable]
    public class StateChange
    {
        public int frame;

        public bool ground;
        public bool stand;
        public bool attack;
        public bool stun;
        public bool knockdown;
        public bool guard;

        public StateChange()
        {
            frame = 0;
            attack = true;
        }
    }
}
