using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Microsoft.CSharp;

namespace Neat.Tools
{
    public class ExtendedPropertyDrawer : PropertyDrawer
    {
        // this class is a workaround
        // for not being able to put more than one propertydrawer on a field

        protected SerializedProperty conditionalProp;
        protected bool gotConditional;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CacheSerializedPropertyConditional(property);

            if (conditionalProp != null && conditionalProp.boolValue)
            {
                return 0f;
            }
            else
            {
                return EditorGUI.GetPropertyHeight(property);
            }
        }

        protected void CacheSerializedPropertyConditional(SerializedProperty property)
        {
            //var type = property.serializedObject.GetType();
            //var attrs = type.GetCustomAttributes();
            //var e = new Editor();

            if (!gotConditional)
            {
                conditionalProp = GetSerializedPropertyConditional(property);
                gotConditional = true;
            }
        }

        private SerializedProperty GetSerializedPropertyConditional(SerializedProperty sender)
        {
            SerializedProperty _prop = null;
            var attr = fieldInfo.GetCustomAttribute<HideIfAttribute>();
            if (attr != null)
            {
                // this will only get unity-serialized fields, not properties
                _prop = sender.serializedObject.FindProperty(attr.fieldName);
            }
            return _prop;
        }
    }
}
