using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Neat.Tools
{
    [CustomPropertyDrawer(typeof(InlineAttribute))]
    public class InlineAttributePropertyDrawer : ExtendedPropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

        }
    }
}
