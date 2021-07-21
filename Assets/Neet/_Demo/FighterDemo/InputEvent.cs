using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Fighter
{
    public class InputEvent
    {
        public int dir { get; set; }
        public int dirBit { get; set; }

        public int dirDown { get; set; }
        public int dirDownBit { get; set; }

        public int btn { get; set; }
        public int btnDown { get; set; }

        public InputEvent()
        {
            dir = 5;
        }
    }
}
