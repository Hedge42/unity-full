using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class ExtendedEditor : Editor
    {
        private MemberInfo[] members;
        private FieldInfo[] fields;
        private PropertyInfo[] properties;
        private MethodInfo[] methods;

        bool extended;

        private bool foldout;

        private void OnEnable()
        {
            var _type = target.GetType();
            extended = _type.GetCustomAttribute<ExtendAttribute>() != null;

            // only members specifically declared in the type
            members = _type.GetMembers().Where(m => m.DeclaringType == _type).ToArray();

            fields = _type.GetFields();
            properties = _type.GetProperties();
            methods = _type.GetMethods();


            //Debug.Log($"Editor Enable, {target.GetType()}");
        }
        public override void OnInspectorGUI()
        {
            if (extended)
            {
                DrawSerializedProperties();
                //foldout = EditorGUIL.Foldout(new Rect(0, 0, 20, 16), foldout, "");

                foldout = EditorGUILayout.Foldout(foldout, "Extensions");
                if (foldout)
                {
                    MakeWindowButtons();
                    EditorGUILayout.Separator();

                    DrawMembers();
                }

                serializedObject.ApplyModifiedProperties();

            }
            else
            {
                base.OnInspectorGUI();
            }
        }

        protected void DrawSerializedProperties()
        {
            // https://gist.github.com/rutcreate/d550aa1ae4052e0a0b37
            SerializedProperty prop = serializedObject.GetIterator();

            // header
            prop.NextVisible(true); // move to script component
            EditorGUI.BeginDisabledGroup(true);
            //DrawProperties(prop, false);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
            EditorGUI.EndDisabledGroup();

            while (prop.NextVisible(false))
                //DrawProperties(prop, false);
                EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
        }

        protected void DrawMembers()
        {
            GUILayout.Label($"Drawing {members.Length} members...");
            foreach (var member in members)
            {
                var attributes = member.GetCustomAttributes();

                if (IsHidden(attributes))
                    continue;

                EditorGUI.BeginDisabledGroup(IsDisabled(attributes));

                foreach (var attr in attributes)
                {
                    if (attr is ButtonAttribute)
                    {
                        //var method = member as MethodInfo;
                        if (GUILayout.Button($"{member.Name}"))
                        {
                            Debug.Log($"valid? {member.IsEzMethod()}");
                        }
                    }
                    else if (attr is ListAttribute)
                    {
                        DrawList(member);
                    }
                }

                EditorGUI.EndDisabledGroup();
            }
        }
        private void DrawList(MemberInfo member)
        {
            var _type = member.GetValueType();
            var isCollection = IsEnumerableType(_type);
            var value = member.GetValue(target);
            Wrap(value);

            // Debug.Log($"list type: {value.GetType()}");
            // Debug.Log($"is object? {value is Object}");
            // TryWithWrapper(value);
        }
        public static bool DrawFromSerializedObject(MemberInfo member, SerializedObject obj)
        {
            // try to find serialized property on given object
            // return successful

            var so = obj.FindProperty(member.Name);
            if (so != null)
            {
                foreach (SerializedProperty prop in so)
                    EditorGUILayout.PropertyField(prop);

                return true;
            }
            else
            {
                return false;
            }
        }
        private bool GetConditional(string memberName)
        {
            var member = members.FirstOrDefault(m => m.Name.Equals(memberName));
            if (member == null)
            {
                Debug.LogError($"No member \"{memberName}\" found");
                return false;
            }
            else
            {
                if (member is PropertyInfo)
                {
                    var info = (member as PropertyInfo);
                    if (info.PropertyType == typeof(bool))
                        return (bool)info.GetValue(target);
                    else
                        Debug.LogError($"Property \"{info.Name}\" " +
                            $"type returned {info.PropertyType}" +
                            $"but needed bool");
                }
                else if (member is FieldInfo)
                {
                    var info = (member as FieldInfo);
                    if (info.FieldType == typeof(bool))
                        return (bool)info.GetValue(target);
                    else
                        Debug.LogError($"Field \"{info.Name}\" " +
                            $"type returned {info.FieldType}" +
                            $"but needed bool");
                }
                else if (member is MethodInfo && member.IsEzMethod())
                {
                    var info = (member as MethodInfo);
                    if (info.ReturnType == typeof(bool))
                        return (bool)info.Invoke(target, null);
                    else
                        Debug.LogError($"Method \"{info.Name}\" " +
                            $"type returned {info.ReturnType}" +
                            $"but needed bool");

                    return (bool)(member as MethodInfo).Invoke(target, null);
                }
                else
                    Debug.LogError("???");
            }

            return false;
        }
        private bool IsHidden(IEnumerable<Attribute> attributes)
        {
            // TODO optimize me
            var attr = attributes.FirstOrDefault(a => a is HideIfAttribute) as HideIfAttribute;
            if (attr != null)
                return GetConditional(attr.fieldName);

            return false;
        }
        private bool IsDisabled(IEnumerable<Attribute> attributes)
        {
            var attr = attributes.FirstOrDefault(a => a is DisabledAttribute) as DisabledAttribute;
            if (attr != null)
                return true;

            var ifAttr = attributes.FirstOrDefault(a => a is DisabledIfAttribute) as DisabledIfAttribute;
            if (ifAttr != null)
                return GetConditional(ifAttr.boolName);

            return false;
        }

        private void HandlePropertyAccessors(PropertyInfo info)
        {
            bool hasGetter = info.GetGetMethod() != null;
            bool hasSetter = info.GetSetMethod() != null;

            Debug.Log($"{info.Name}: get({hasGetter}), set({hasSetter})");
        }
        private void MakeWindowButtons()
        {
            if (GUILayout.Button("Open in Editor Window"))
            {
                ExtendedEditorWindow.Open(target as AttributesDemo);
            }
            else if (GUILayout.Button("Open in GUI Window"))
            {
                GUIWindowDrawer.instance.Open(target as AttributesDemo);
            }
        }

        // make these extensions
        

        public static bool IsEnumerableType(Type type)
        {
            return (type.GetInterface(nameof(System.Collections.IEnumerable)) != null);
        }
        

        private void Wrap(object value)
        {
            var scriptableObject = CreateInstance<SerializedWrapper>();
            var serializedObject = new SerializedObject(scriptableObject);
            SerializedProperty prop;
            if (value is Object)
            {
                scriptableObject.obj = (value as Object);
                prop = serializedObject.FindProperty("obj");

                throw new System.NotImplementedException();
            }
            else if (value is Object[])
            {
                scriptableObject.arr = (value as Object[]);
                prop = serializedObject.FindProperty("arr");

                if (prop != null)
                {
                    EditorGUILayout.PropertyField(prop, GUIContent.none, true);
                    //foreach (SerializedProperty p in prop)
                        //EditorGUILayout.PropertyField(p);
                }
                else
                {
                    Debug.LogError($"not found");
                }
            }
        }
    }

    public class SerializedWrapper : ScriptableObject
    {
        // should use reflection to get and set members

        [SerializeReference]
        public Object obj;

        [SerializeReference]
        public Object[] arr;
    }
}
