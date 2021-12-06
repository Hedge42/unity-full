using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomGUIInspectorAttribute : Attribute
    {
        private Type type;
        public CustomGUIInspectorAttribute(Type type)
        {
            this.type = type;
        }

        public static Dictionary<Type, Type> customInspectors = new Dictionary<Type, Type>()
        {
            { typeof(DebugConsole) , typeof(DebugConsoleGUIInspector) }
        };
    }

    public class GUIInspector : ScriptableObject
    {
        public static Type GetGUIInspectorType(Object target)
        {
            var type = target.GetType();
            var dick = CustomGUIInspectorAttribute.customInspectors;
            if (dick.TryGetValue(type, out Type t))
                return t;

            else
                return typeof(GUIInspector); // default
        }

        public static readonly BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        // private static readonly BindingFlags flags = BindingFlags.DeclaredOnly;


        public Object target;
        private MemberInfo[] members;

        public static GUIInspector from(Object target)
        {
            GUIInspector inspector;

            // target.GetType().GetCustomAttribute<CustomGUIInspectorAttribute>();

            // check custom inspectors
            if (target is DebugConsole)
                inspector = new DebugConsoleGUIInspector(target as DebugConsole);

            else
                inspector = new GUIInspector(target);

            return inspector;
        }
        public GUIInspector(Object target)
        {
            this.target = target;
            this.members = FindMembers();
        }

        protected virtual MemberInfo[] FindMembers()
        {

            List<MemberInfo> members = new List<MemberInfo>();
            var type = target.GetType();

            members.AddRange(type.GetFields(flags).Where(m => !m.Name.Contains("<"))); // i
            members.AddRange(type.GetProperties(flags));
            members.AddRange(type.GetMethods(flags).Where(method => method.GetCustomAttribute<ButtonAttribute>() != null));

            return members.Where(m => m.ValidMember(target)).ToArray();
            // return target.GetType().GetMembers(flags).Where(m => m.ValidMember(target)).ToArray();
        }

        // must be called from outside MonoBehaviour
        public virtual void OnGUI()
        {
            // Debug.Log($"Drawing {members.Length} GUI members for {target.GetType()}");
            foreach (var member in members)
            {
                GUIFunctions.DrawMemberLayout2(member, target);
            }
        }
    }
}
