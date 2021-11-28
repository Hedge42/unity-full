using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Tools;
using Neat.Experimental.Tutorials;
using Neat.Tools;
using Neat.Tools;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    [Extend]
    public class AttributesDemo : MonoBehaviour
    {
        // to show off and test attributes
        [MinMax(0f, 10f)]
        public Vector2 float_range;

        [Range(1, 69)]
        public float f;

        [Range(69, 420)]
        public int myInt;

        [MinMax(0, 10)]
        public Vector2Int int_range;

        //[BeginDisabledGroup, MinMax(-1, 69)]
        [MinMax(-1, 69), Disabled]
        public Vector2Int disabled_range = new Vector2Int(4, 20);
        //[EndDisabledGroup]

        public bool isMute;

        public string name1;

        [DisabledIf("isMute")]
        public string muteDisabled;

        [List]
        public Object[] objs;


        public TestClass classObj;

        [List]
        public TestClass[] classArr;

        public static int staticInt;

        [SerializeProperty]
        public string getProperty => "nice";

        [SerializeProperty]
        public string autoProperty { get; set; }

        //[SerializeField, HideIf("isMute"), Button("SayHello")] private string dummyt;
        public void SayHello()
        {
            print("This works!");
        }

        [Button, DisabledIf("isMute")]
        public void Disable()
        {

        }
        [Button, HideIf("isMute")]
        public void Enable()
        {

        }

        [Serializable]
        public class TestClass : Object
        {
            [Range(0, 10)]
            public int a;
            [Disabled]
            public int b;
        }
    }

    [CreateAssetMenu(menuName = "Neat/AttributeDemoObject")]
    public class ScriptableObjectAttributesDemo : ScriptableObject
    {
        [MinMax(0f, 10f)]
        public Vector2 float_range;

        [MinMax(0, 10)]
        public Vector2Int int_range;

        public bool isDisabled;
    }


}


