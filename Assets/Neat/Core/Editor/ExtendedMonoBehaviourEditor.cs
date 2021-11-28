using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Neat.Tools;
using System.Linq;
using UnityEngine.UIElements;

namespace Neat.Tools
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ExtendedMonoBehaviour))]
    public class ExtendedMonoBehaviourEditor : Editor
    {
        private void OnEnable()
        {
            var objs = Editor.FindSceneObjectsOfType(typeof(MonoBehaviour));


            hasMethods = false;
            Debug.Log($"{GetType()} Enabled", this);
        }

        bool hasMembers = false;
        bool hasMethods = false;

        MethodInfo[] buttons;
        Type _type;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!hasMethods)
            {
                buttons = GetMethods().ToArray();
                hasMethods = true;
            }

            foreach (var method in buttons)
            {
                if (GUILayout.Button(method.Name))
                {
                    method.Invoke(target, null);
                }
            }
        }
        

        private List<MethodInfo> GetMethods()
        {
            // any void method
            // ?
            List<MethodInfo> methods = new List<MethodInfo>();
            _type = target.GetType();

            foreach (var method in _type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                var attribute = method.GetCustomAttribute<ButtonAttribute>();
                Action a = () => method.Invoke(target, null);
                // attribute.action = a;

                var _params = method.GetParameters();

                if (_params.Length == 0 && attribute != null)
                {
                    methods.Add(method);
                }
            }
            return methods;
        }
        private void GetMembers()
        {
            // use reflection to find methods with button attributes
            Type _type = target.GetType();

            var members = _type.GetMembers(BindingFlags.Public);
            foreach (var member in members)
            {
                if (member.CustomAttributes.ToArray().Length > 0)
                {
                    var attribute = member.GetCustomAttribute<ButtonAttribute>();
                    if (attribute != null)
                    {
                        Debug.Log(member.ReflectedType);

                        // do stuff
                        //var name = $"{member.ReflectedType}/{attribute.instance}";
                    }
                }
            }
            Debug.Log("Got members");
            hasMembers = true;
        }
    }
}
