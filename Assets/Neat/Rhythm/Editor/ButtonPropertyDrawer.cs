using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace Neat.Attributes
{
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Debug.Log(property.serializedObject.context.GetType());

            position.y += 20;
            //base.OnGUI(position, property, label);
            if (GUI.Button(position, fieldInfo.Name))
            {
                if (fieldInfo.FieldType == typeof(Action))
                {
                    Action a = (Action)fieldInfo.GetValue(property.serializedObject.targetObject);
                    a.Invoke();
                }
            }
        }
    }
}
