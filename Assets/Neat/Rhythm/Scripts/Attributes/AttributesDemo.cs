using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Neat.Attributes
{
    public class AttributesDemo : MonoBehaviour
    {
        // to show off and test attributes
        [MinMax(0f, 10f)]
        public Vector2 float_range;

        [MinMax(0, 10)]
        public Vector2Int int_range;

        [Disabled]
        public bool isDisabled;

        [Button]
        public void SayHello()
        {
            print("YOOOOO");
        }
    }
}
