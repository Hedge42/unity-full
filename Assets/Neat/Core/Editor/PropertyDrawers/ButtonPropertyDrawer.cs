using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;

namespace Neat.Tools
{
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonPropertyDrawer : ExtendedPropertyDrawer
    {
        private MethodInfo method;
        private bool isAwake;
        private string methodName;
        private Object obj;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!isAwake)
            {
                obj = property.serializedObject.targetObject;
                methodName = (attribute as ButtonAttribute).methodName;
                method = obj.GetType().GetMethod(methodName);
                // method.GetParameters() // TODO validate params before setting method
                isAwake = true;
            }

            EditorGUI.BeginProperty(position, label, property);

            if (method != null && GUI.Button(position, methodName))
            {
                method.Invoke(obj, null);
            }
            EditorGUI.EndProperty();
        }
    }
}
