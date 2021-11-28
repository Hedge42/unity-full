using UnityEditor;
using UnityEngine;
using Neat.Tools;
using UnityEditor.Callbacks;
using Neat.Experimental.Tutorials;

namespace Neat.Tools
{
    public class ExtendedEditorWindow : EditorWindow
    {
        protected SerializedObject serializedObject;
        protected SerializedProperty currentProperty;
        protected string selectedPropertyPath;
        protected SerializedProperty selectedProperty;
        protected Editor inspector;
        protected bool gotInspector;

        public virtual void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            DrawAllProperties();
            //DrawInspector();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }


            EditorUtility.SetDirty(serializedObject.targetObject);
        }

        protected void DrawAllProperties()
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
        protected void DrawAllProperties2()
        {
            SerializedProperty prop = serializedObject.GetIterator();
            prop.NextVisible(true);
            // prop.NextVisible(false);
            // prop.NextVisible(true);

            // header
            //prop.NextVisible(true); // move to script component
            //EditorGUI.BeginDisabledGroup(true);
            //DrawProperties(prop, false);
            ////EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
            //EditorGUI.EndDisabledGroup();

            while (prop.NextVisible(false))
                DrawProperties(prop, false);
            //    //EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
        }
        public static void DrawProperties(SerializedProperty prop, bool drawChildren)
        {
            // https://youtu.be/c_3DXBrH-Is?t=359
            string lastPropPath = string.Empty;
            foreach (SerializedProperty p in prop)
            {
                if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUILayout.BeginHorizontal();
                    p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                    EditorGUILayout.EndHorizontal();

                    if (p.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        DrawProperties(p, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath))
                        continue;

                    lastPropPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        }
        protected void DrawSidebar(SerializedProperty prop)
        {
            // get selected property
            // where the property is a collection
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
            foreach (SerializedProperty p in prop)
            {
                if (GUILayout.Button(p.displayName))
                {
                    selectedPropertyPath = p.propertyPath;
                }
            }

            if (!string.IsNullOrEmpty(selectedPropertyPath))
            {
                selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
            }
            EditorGUILayout.EndVertical();

            // draw selected property
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            if (selectedProperty != null)
            {
                DrawProperties(selectedProperty, true);
            }
            else
            {
                EditorGUILayout.LabelField("Select an item from the list");
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        protected void DrawInspector()
        {
            if (!gotInspector)
            {
                gotInspector = true;
                inspector = Editor.CreateEditor(serializedObject.targetObject);
            }

            inspector.OnInspectorGUI();
        }


        public static void Open<T>(T obj) where T : Object
        {
            var _type = obj.GetType();
            ExtendedEditorWindow window = GetWindow<ExtendedEditorWindow>($"{_type} EditorWindow");
            window.serializedObject = new SerializedObject(obj);
        }
        public static void Open(Object obj)
        {
            var _type = obj.GetType();
            ExtendedEditorWindow window = GetWindow<ExtendedEditorWindow>($"{_type} EditorWindow");
            window.serializedObject = new SerializedObject(obj);
        }
    }
}
