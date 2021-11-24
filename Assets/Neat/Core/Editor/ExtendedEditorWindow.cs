using UnityEditor;
using UnityEngine;

namespace Neat.Extensions
{
    using UnityEditor;

    public class ExtendedEditorWindow : EditorWindow
    {
        // https://www.youtube.com/watch?v=c_3DXBrH-Is
        protected SerializedObject serializedObject;
        protected SerializedProperty currentProperty;

        private string selectedPropertyPath;
        protected SerializedProperty selectedProperty;

        public static void DrawProperties(SerializedProperty prop, bool drawChildren)
        {
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
        }
        protected void DrawField(string propName, bool relative)
        {
            if (relative && currentProperty != null)
            {
                EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative(propName), true);
            }
            else if (serializedObject != null)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(propName), true);
            }
        }
        protected void DrawSelectedPropertiesPanel()
        {
            currentProperty = selectedProperty;

            EditorGUILayout.BeginHorizontal("box");
            // DrawField("name", true);
            DrawField("title", true);
            EditorGUILayout.EndHorizontal();
        }

        protected void Apply()
        {
            serializedObject.ApplyModifiedProperties();
        }

    }
}