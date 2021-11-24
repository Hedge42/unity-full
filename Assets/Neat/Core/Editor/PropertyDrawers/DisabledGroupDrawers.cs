using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Neat.Attributes
{
    [CustomPropertyDrawer(typeof(BeginDisabledGroupAttribute))]
    public class BeginDisabledGroupDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return 0;
        }
        public override void OnGUI(Rect position)
        {
            //GUI.enabled = false;
            EditorGUI.BeginDisabledGroup(true);
        }
    }

    [CustomPropertyDrawer(typeof(EndDisabledGroupAttribute))]
    public class EndDisabledGroupDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return 0;
        }
        public override void OnGUI(Rect position)
        {
            //GUI.enabled = true;
            //EditorGUI.BeginDisabledGroup(false);
            EditorGUI.EndDisabledGroup();

        }
    }


}
