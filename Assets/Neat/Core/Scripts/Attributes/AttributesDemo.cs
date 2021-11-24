using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Extensions;
using System;
using Neat.Tutorials;

namespace Neat.Attributes
{
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

        [BeginDisabledGroup, MinMax(-1, 69)]
        public Vector2Int disabled_range = new Vector2Int(4, 20); 
        [EndDisabledGroup]

        public bool isMute;

        public string name1;
        public string name2;

        [SerializeField, HideIf("isMute"), Button("SayHello")] private string dummyt;
        public void SayHello()
        {
            print("This works!");
        }

        [Button("Disable")] public string dummy2;
        public void Disable()
        {
            
        }

        private void OnGUI()
        {
            // GUI.sli

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


