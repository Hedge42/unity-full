using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Neat.Attributes
{
    public class ExtendedPropertyDrawer : PropertyDrawer
    {
        // this class is a workaround
        // for not being able to put more than one propertydrawer on a field

        protected SerializedProperty conditionalProp;
        protected bool gotConditional;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CacheConditional(property);

            if (conditionalProp != null && conditionalProp.boolValue)
            {
                return 0f;
            }
            else
            {
                return EditorGUI.GetPropertyHeight(property);
            }
        }

        protected void CacheConditional(SerializedProperty property)
        {
            if (!gotConditional)
            {
                conditionalProp = GetConditionalProperty(property);
                gotConditional = true;
            }
        }

        private SerializedProperty GetConditionalProperty(SerializedProperty sender)
        {
            SerializedProperty _prop = null;
            var attr = fieldInfo.GetCustomAttribute<HideIfAttribute>();
            if (attr != null)
                _prop = sender.serializedObject.FindProperty(attr.fieldName);
            return _prop;
        }
    }

    [CustomPropertyDrawer(typeof(Array))]
    public class idkDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.Button(position, "lol");
            base.OnGUI(position, property, label);
        }
    }
}
