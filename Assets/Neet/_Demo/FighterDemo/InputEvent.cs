using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Fighter
{
    public class InputEvent
    {
        public int value;
        public int valueDown;

        public InputEvent(int value = 0, int valueDown = 0)
        {
            this.value = value;
            this.valueDown = valueDown;
        }
    }
}
