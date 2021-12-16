using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    /// <summary>
    /// then uses UnityEngine.GUILayout to draw the 
    /// Finds and displays methods of an object
    /// </summary>
    public class GUIInspector : ScriptableObject
    {
        public static readonly BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        public Object target;
        private MemberInfo[] members;


        /// <summary>
        /// Create a new inspector instance
        /// </summary>
        public static GUIInspector from(Object target)
        {
            GUIInspector inspector;

            var attribute = target.GetType().GetCustomAttribute<CustomGUIInspectorAttribute>();
            if (attribute != null)
            {
                if (!attribute.type.IsSubclassOf(typeof(GUIInspector)))
                    throw new ArgumentException("Custom GUIInspector class must inherit from base GUIInspector class");

                inspector = CreateInstance(attribute.type) as GUIInspector;
            }

            else
            {
                inspector = CreateInstance<GUIInspector>();
            }

            inspector.target = target;
            inspector.members = inspector.FindMembers();

            return inspector;
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
                // GUIFunctions.DrawMemberLayout(member, target);
                GUIFunctions.DrawMemberLayout2(member, target);
            }
        }
    }
}
