using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Neat.Attributes
{
    [CustomPropertyDrawer(typeof(MinMaxAttribute))]
    public class BoundsPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // base.OnGUI(position, property, label);
            // https://forum.unity.com/threads/get-a-general-object-value-from-serializedproperty.327098/
            var targetObject = property.serializedObject.targetObject;
            var boundsAttr = attribute as MinMaxAttribute;
            // var targetObjectClassType = targetObject.GetType();
            // var field = targetObjectClassType.GetField(property.propertyPath);
            var field = fieldInfo;

            if (field != null)
            {
                if (field.FieldType == typeof(Vector2))
                {

                    Vector2 v = (Vector2)field.GetValue(targetObject);
                    Vector2 minmax = boundsAttr.range;

                    float _min = v.x;
                    float _max = v.y;

                    // attribute data

                    // this.fieldInfo.GetValue
                    float width = position.width;
                    float xMax = position.xMax;
                    GUILayout.BeginHorizontal();

                    position = EditorGUI.PrefixLabel(position, label);

                    position.width = 40;
                    _min = EditorGUI.FloatField(position, _min);
                    position.x += 40;

                    position.width = (xMax - 40) - position.x;
                    EditorGUI.MinMaxSlider(position, ref _min, ref _max, minmax.x, minmax.y);
                    position.x += position.width;

                    position.width = 40;
                    _max = EditorGUI.FloatField(position, _max);
                    // position.x += 40;

                    field.SetValue(targetObject, new Vector2(_min, _max));

                    GUILayout.EndHorizontal();
                }
                else if (field.FieldType == typeof(Vector2Int))
                {
                    Vector2Int v = (Vector2Int)field.GetValue(targetObject);
                    Vector2 minmax = boundsAttr.range;

                    //int ix = v.x;
                    //int iy = v.y;
                    float fx = (float)v.x;
                    float fy = (float)v.y;

                    // attribute data

                    // this.fieldInfo.GetValue
                    float width = position.width;
                    float xMax = position.xMax;
                    GUILayout.BeginHorizontal();

                    position = EditorGUI.PrefixLabel(position, label);

                    position.width = 40;
                    fx = EditorGUI.IntField(position, Mathf.RoundToInt(fx));
                    position.x += 40;

                    position.width = (xMax - 40) - position.x;
                    EditorGUI.MinMaxSlider(position, ref fx, ref fy, minmax.x, minmax.y);

                    position.x += position.width;

                    position.width = 40;
                    fy = EditorGUI.IntField(position, Mathf.RoundToInt(fy));

                    int ix = Mathf.RoundToInt(fx);
                    int iy = Mathf.RoundToInt(fy);
                    // position.x += 40;

                    field.SetValue(targetObject, new Vector2Int(ix, iy));

                    GUILayout.EndHorizontal();
                }

            }
        }
    }
}
