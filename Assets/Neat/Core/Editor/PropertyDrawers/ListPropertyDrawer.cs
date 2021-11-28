using Neat.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Neat.Tools
{
    // [CustomPropertyDrawer(typeof(ListAttribute))]
    public class ListPropertyDrawer : PropertyDrawer
    {
        //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        //{
        //    return base.GetPropertyHeight(property, label);
        //}
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            var btnPosition = new Rect(position);
            btnPosition.width = 20f;
            GUI.Button(btnPosition, "+");

            var xmax = position.xMax;
            position.x += btnPosition.width;
            position.xMax = xmax;
            EditorGUI.PropertyField(position, property);
            //base.OnGUI(position, property, label);

            // on editor initialization
            // use reflection to find attribute
            // create new serialized object based on the field

            //Editor e = Editor.CreateEditor(property.serializedObject.targetObject);
            // do inspector this way??
            //e.OnInspectorGUI();
        }



    }

    
}