using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MinMaxAttribute : PropertyAttribute
    {
        public Vector2 range;
        public MinMaxAttribute(float min, float max)
        {
            range.x = min;
            range.y = max;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class DisabledAttribute : PropertyAttribute
    {
        void k()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : Attribute
    {
        public ButtonAttribute()
        {
            Debug.Log("Hello!");
        }
    }
}
