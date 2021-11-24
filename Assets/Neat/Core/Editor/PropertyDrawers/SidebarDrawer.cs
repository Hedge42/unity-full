using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Neat.Extensions;
using Neat.Tools;

namespace Neat.Attributes
{
    // WARNING: BROKE
    [CustomPropertyDrawer(typeof(SidebarAttribute))]
    public class SidebarDrawer : PropertyDrawer
    {
        string selectedPropertyPath;
        SerializedProperty selectedProperty;
        SerializedProperty array;


        //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        //{
        //    return base.GetPropertyHeight(property, label);
        //}

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            string path = property.propertyPath;
            path = path.Substring(0, path.LastIndexOf('.'));
            ////Your path should end with Array.data[x], so removing the last part will access the array path
            array = property.serializedObject.FindProperty(path);
            float height = EditorGUI.GetPropertyHeight(array);
            int count = array.arraySize;

            //            Debug.Log($"{height} ({count})");

            DrawPropertiesGUI(position, property, true);
        }
        public void DrawSidebarLayout(SerializedProperty prop)
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
                selectedProperty = prop.serializedObject.FindProperty(selectedPropertyPath);
            }
            EditorGUILayout.EndVertical();

            // draw selected property
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            if (selectedProperty != null)
            {
                ExtendedEditorWindow.DrawProperties(selectedProperty, true);
            }
            else
            {
                EditorGUILayout.LabelField("Select an item from the list");
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        public void DrawSidebar(Rect position, SerializedProperty prop)
        {
            var startPosition = position.position;

            Rect btnRect = new Rect(position);
            btnRect.width = 50f;

            Rect areaRect = new Rect(position);
            areaRect.width = position.width - btnRect.width;
            areaRect.position += Vector2.right * btnRect.width;
            // array
            // areaRect.height = GetPropertyHeight(100);

            if (GUI.Button(btnRect, "wtf"))
            {
                selectedPropertyPath = prop.propertyPath;
            }

            if (selectedPropertyPath.Equals(prop.propertyPath))
            {
                EditorGUI.PropertyField(areaRect, selectedProperty, GUIContent.none);
                // DrawPropertiesGUI(areaRect, prop, true);
            }

            //foreach (SerializedProperty p in prop)
            //{
            //    if (GUI.Button(btnRect, p.name))
            //    {
            //        selectedPropertyPath = p.propertyPath;
            //    }
            //    btnRect.position += Vector2.up * btnRect.height;
            //}

            //if (!string.IsNullOrEmpty(selectedPropertyPath))
            //{
            //    selectedProperty = prop.serializedObject.FindProperty(selectedPropertyPath);
            //}

            //// draw selected property
            //if (selectedProperty != null)
            //{

            //    EditorGUI.PropertyField(areaRect, selectedProperty, false);
            //    // DrawPropertiesGUI(areaRect, selectedProperty, false);
            //}
            //else
            //{
            //    EditorGUI.LabelField(areaRect, "Select an item from the list");
            //}
        }

        public static void DrawPropertiesGUI(Rect position, SerializedProperty prop, bool drawChildren)
        {
            // https://youtu.be/c_3DXBrH-Is?t=359
            string lastPropPath = string.Empty;
            foreach (SerializedProperty p in prop)
            {
                if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
                {
                    p.isExpanded = EditorGUI.Foldout(position, p.isExpanded, p.displayName);

                    if (p.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        DrawPropertiesGUI(position, p, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath))
                        continue;

                    lastPropPath = p.propertyPath;
                    EditorGUI.PropertyField(position, p, drawChildren);
                }
            }
        }
    }
}
