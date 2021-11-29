using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using Neat.Tools.Extensions;
using Object = UnityEngine.Object;


namespace Neat.Tools
{
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public partial class ExtendedEditor : Editor
    {
        private MemberInfo[] members;
        private FieldInfo[] fields;
        private PropertyInfo[] properties;
        private MethodInfo[] methods;
        private Dictionary<MemberInfo, SerializedWrapper> dick;

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
            // dick = new Dictionary<MemberInfo, SerializedWrapper>();
            // CreateSerializedProperties();
        }
        public override void OnInspectorGUI()
        {
            if (extended)
            {
                //DrawSerializedProperties(serializedObject);
                //foldout = EditorGUIL.Foldout(new Rect(0, 0, 20, 16), foldout, "");

                toolbar = InspectorToolbar();
                //if (toolbar == 0)
                //{
                //    //base.OnInspectorGUI();
                //    DrawSerializedProperties(serializedObject);
                //}
                //else if (toolbar == 1)
                //{
                //    DrawMembers();
                //}

                serializedObject.ApplyModifiedProperties();

            }
            else
            {
                base.OnInspectorGUI();
            }
        }

        protected void CopycatInspector(SerializedObject serializedObject)
        {
            MakeWindowButtons();

            // https://gist.github.com/rutcreate/d550aa1ae4052e0a0b37
            SerializedProperty prop = serializedObject.GetIterator();
            CopycatHeader(prop);

            while (prop.NextVisible(false))
                //DrawProperties(prop, false);
                EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
        }

        private void CopycatHeader(SerializedProperty prop)
        {
            // header
            prop.NextVisible(true); // move to script component

            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
            EditorScriptHeader();
            GUI.enabled = true;
        }
        private void EditorScriptHeader()
        {
            var script = MonoScript.FromScriptableObject(this);
            bool wasEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Editor Script");
            EditorGUILayout.ObjectField(script, typeof(MonoScript), true);
            EditorGUILayout.EndHorizontal();
            GUI.enabled = wasEnabled;
        }


        // *****
        bool _memberFoldout;
        protected void GUIInspector()
        {
            _memberFoldout = EditorGUILayout.Foldout(_memberFoldout, $"View {members.Length} members");
            // EditorGUILayout.InspectorTitlebar
            var level = EditorGUI.indentLevel;

            if (_memberFoldout)
            {
                EditorGUI.indentLevel += 1;
                Extensions.Functions.ViewTargetMembers(target, members);
                EditorGUI.indentLevel -= 1;
            }
        }
        void DrawExtendedProperty(MemberInfo member)
        {
            var attributes = member.GetCustomAttributes();

            if (IsHidden(attributes))
                return;

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

        int toolbar;
        int InspectorToolbar()
        {
            string[] options =
            {
                "Base Inspector",
                "Copycat Inspector",
                "GUI Inspector"
            };

            toolbar = GUILayout.Toolbar(toolbar, options);

            if (toolbar == 0)
            {
                base.OnInspectorGUI();
            }
            else if (toolbar == 1)
            {
                CopycatInspector(serializedObject);
            }
            else if (toolbar == 2)
            {
                GUIInspector();
            }
            return toolbar;
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
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Open in Editor Window"))
            {
                ExtendedEditorWindow.Open(target as AttributesDemo);
            }
            else if (GUILayout.Button("Open in GUI Window"))
            {
                GUIWindowDrawer.instance.Open(target as AttributesDemo);
            }
            GUILayout.EndHorizontal();

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
