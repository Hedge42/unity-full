using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Tools
{
    // attributes only need declarations with optional constructors
    // so putting them in the same file makes sense
    public static partial class Functions
    {
        public static List<MemberInfo> FindAttributeMembers<T>(BindingFlags flags) where T : Attribute
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<MemberInfo> exposedMembers = new List<MemberInfo>();

            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    MemberInfo[] members = type.GetMembers(flags);
                    foreach (MemberInfo member in members)
                    {
                        // TESTME
                        if (member.CustomAttributes.ToArray().Length > 0)
                        {
                            T attribute = member.GetCustomAttribute<T>();
                            if (attribute != null)
                            {
                                exposedMembers.Add(member);
                                // do stuff
                                // fields.Add((FieldInfo)member);
                                // fieldNames.Add($"{member.ReflectedType}/{attribute.instance}");
                            }
                        }
                    }
                }
            }
            return exposedMembers;
        }
    }

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

    public class BeginDisabledGroupAttribute : PropertyAttribute { }
    public class EndDisabledGroupAttribute : PropertyAttribute { }

    // [AttributeUsage(AttributeTargets.)]
    public class DisabledAttribute : Attribute { }

    // [AttributeUsage(AttributeTargets.Field)]
    public class DisabledIfAttribute : Attribute
    {
        public string boolName { get; private set; }
        public DisabledIfAttribute(string boolName)
        {
            this.boolName = boolName;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class ButtonAttribute : PropertyAttribute
    {
        // public Action action;
        public string methodName;

        public ButtonAttribute(string methodName)
        {
            // could use reflection here to find the method...
            this.methodName = methodName;
        }
        public ButtonAttribute()
        {
        }
    }

    public class HideIfAttribute : Attribute
    {
        public string fieldName;
        public HideIfAttribute(string boolFieldName)
        {
            this.fieldName = boolFieldName;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SerializePropertyAttribute : Attribute { }

    public class InlineAttribute : PropertyAttribute
    {
        // for serialized classes, removes the dropdown and shows properties by default
        // for scriptableObjects and components, display the reference field
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class SidebarAttribute : PropertyAttribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class ExtendAttribute : PropertyAttribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public class ListAttribute : PropertyAttribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ToolbarAttribute : Attribute { }
}

