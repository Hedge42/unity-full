using System;
using System.Reflection;
using UnityEngine;

namespace Neat.Tools
{
    public abstract class GuiInspector : ScriptableObject
    {
        public abstract MemberInfo[] FindMembers();

        public static GuiInspector fromObjectType(Type targetType)
        {
            // using attribute map
            return GuiInspectorMap.instance[targetType];
        }
        public static GuiInspector fromInspectorType(Type inspectorType)
        {
            return GuiInspectorMap.instance[inspectorType];
        }

        public abstract void DrawLayout(object target, object value);
    }
}
