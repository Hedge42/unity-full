using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using Neat.Tools;
using Object = UnityEngine.Object;


namespace Neat.Tools
{
    // [CustomEditor(typeof(object), true), CanEditMultipleObjects]
    public class MonoEditor : Editor
    {
        private MemberInfo[] members;
        private bool enabled;
        private int toolbar;

        [SerializeField] private string[] options;
        [SerializeField] private Editor[] _subEditors;

        private Editor[] subEditors
        {
            get
            {
                if (_subEditors == null)
                    _subEditors = GetSubEditors(out options);
                return _subEditors;
            }
        }
        public override void OnInspectorGUI()
        {
            // GUIInspector.Instantiate()
            if (subEditors.Length > 1)
            {
                DrawSubEditors();
            }
            else
            {
                base.OnInspectorGUI();
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        protected Editor[] GetSubEditors(out string[] options)
        {
            var p = PerformanceElement.Start("Getting sub editors...");
            var list = new List<Editor>();
            var strs = new List<string>();

            list.Add(CopycatEditor.CreateInstance(target)); // base
            strs.Add("Base Editor");

            var type = target.GetType();
            var guiAttribute = type.GetCustomAttribute<GUIInspectorAttribute>();
            if (guiAttribute != null)
            {
                var guiEditor = GUIInspectorEditor.CreateInstance(target);
                list.Add(guiEditor);
                strs.Add("GUI Editor");
            }

            var extendedAttribute = type.GetCustomAttribute<ExtendedInspectorAttribute>();
            if (extendedAttribute != null)
            {
                // var exEditor = ExtendedEditor.GetEditor();
                list.Add(null);
                strs.Add("Copycat Inspector");
            }

            //DebugEditors(list);

            p.Stop();
            options = strs.ToArray();
            return list.ToArray();
        }

        private static void DebugEditors(List<Editor> list)
        {
            Debug.Log($"Has: {list.Count} editors");
            foreach (var e in list)
            {
                Debug.Log($"{e.GetType()}");
            }
        }

        protected void DrawSubEditors()
        {
            toolbar = GUILayout.Toolbar(toolbar, options);

            if (toolbar > 0)
            {
                subEditors[toolbar].OnInspectorGUI();
            }
            else
            {
                base.OnInspectorGUI();
            }
        }

        private void DrawList(MemberInfo member)
        {
            throw new System.NotImplementedException();
            // create a serializedObject for the array

            //var _type = member.GetValueType();
            //var isCollection = _type.IsEnumerableType();
            var value = member.GetValue(target);
            var property = CreateSerializedProperty(member, value);

            //   property;


            // DrawSerializedProperty(value);

            // Debug.Log($"list type: {value.GetType()}");
            // Debug.Log($"is object? {value is Object}");
            // TryWithWrapper(value);
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




            //EditorGUILayout.Separator();
        }
        private void CreateSerializedProperties()
        {
            foreach (MemberInfo member in members)
            {
                var attr = member.GetCustomAttribute<SerializePropertyAttribute>();

                if (attr != null)
                {
                    var value = member.GetValue(target);
                    var prop = CreateSerializedProperty(member, member.GetValue(target));
                }
            }
        }
        private SerializedProperty CreateSerializedProperty(MemberInfo info, object value)
        {
            // instantiates a serializable, scriptableObject
            // which stores a reference to a member

            var scriptableObject = CreateInstance<SerializedWrapper>(); // create ScriptableObject data
            scriptableObject.member = info; // give ScriptableObject a reference to the Member
            // dick[info] = scriptableObject; // give the Member a reference to the ScriptableObject

            var serializedObject = new SerializedObject(scriptableObject);
            serializedObject.Draw();

            SerializedProperty prop = null;
            if (value is Object)
            {
                scriptableObject.obj = (value as Object);
                prop = serializedObject.FindProperty("obj");

                EditorGUILayout.PropertyField(prop, GUIContent.none, true);
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

            return prop;
        }
    }

    public class SerializedWrapper : ScriptableObject
    {
        // should use reflection to get and set members
        public MemberInfo member;

        [SerializeField]
        public object value;

        public static SerializedWrapper Instantiate(MemberInfo member, object value)
        {
            var scriptableObject = CreateInstance<SerializedWrapper>();
            scriptableObject.value = value;
            return scriptableObject;
        }

        [SerializeReference]
        public Object obj;

        [SerializeReference]
        public Object[] arr;
    }
}
