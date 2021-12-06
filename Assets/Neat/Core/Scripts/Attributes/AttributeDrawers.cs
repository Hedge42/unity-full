using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    public static class CustomGUISettings
    {
        public static bool drawPrefix = true;
        public const float prefixWidth = 100;
        public const float labelWidth = 50; // ???
    }

    [CustomGUIDrawer(typeof(int))]
    public static class IntDrawer
    {
        public static void DrawProperty(Rect rect, MemberInfo member, Object target, object value, params GUILayoutOption[] options)
        {
            var range = member.GetCustomAttribute<RangeAttribute>();
            int result;
            if (range != null)
                result = GUIFunctions.IntSlider(rect, (int)value, (int)range.min, (int)range.max);
            else
                result = GUIFunctions.IntField(rect, (int)value, (int)range.min, (int)range.max);

            member.SetValue(target, result);
        }
        public static void DrawIntLayout(MemberInfo member, Object target, object value, params GUILayoutOption[] options)
        {
            // int _value = Convert.ToInt32(value);
            var range = member.GetCustomAttribute<RangeAttribute>();
            int result;
            if (range != null)
                result = GUIFunctions.IntSliderLayout((int)value, (int)range.min, (int)range.max, options);
            else
                result = GUIFunctions.IntFieldLayout((int)value, int.MinValue, int.MaxValue, options);

            member.SetValue(target, result);
        }

        public static int GetIntLayout(MemberInfo member, Object target, object value, params GUILayoutOption[] options)
        {
            // int _value = Convert.ToInt32(value);
            var range = member.GetCustomAttribute<RangeAttribute>();
            int result;
            if (range != null)
                result = GUIFunctions.IntSliderLayout((int)value, (int)range.min, (int)range.max, options);
            else
                result = GUIFunctions.IntFieldLayout((int)value, int.MinValue, int.MaxValue, options);

            return result;
            // member.SetValue(target, result);
        }

    }

    [CustomGUIDrawer(typeof(float))]
    public static class FloatDrawer
    {
        public static void DrawProperty(Rect rect, MemberInfo member, Object target, object value)
        {
            var x = member.GetCustomAttribute<RangeAttribute>();
            float result;
            if (x != null)
                result = GUIFunctions.FloatSlider(rect, (float)value, x.min, x.max);
            else
                result = GUIFunctions.FloatField(rect, (float)value, x.min, x.max);

            member.SetValue(target, result);
        }
        public static void DrawFloatLayout(MemberInfo member, Object target, object value)
        {
            var x = member.GetCustomAttribute<RangeAttribute>();
            float result;
            if (x != null)
                result = GUIFunctions.FloatSliderLayout((float)value, x.min, x.max);
            else
                result = GUIFunctions.FloatFieldLayout((float)value, x.min, x.max);

            member.SetValue(target, result);
        }

        public static float GetFloatLayout(MemberInfo member, Object target, object value)
        {
            var x = member.GetCustomAttribute<RangeAttribute>();
            float result;
            if (x != null)
                result = GUIFunctions.FloatSliderLayout((float)value, x.min, x.max);
            else
                result = GUIFunctions.FloatFieldLayout((float)value, x.min, x.max);

            // member.SetValue(target, result);
            return result;
        }
    }

    [CustomGUIDrawer(typeof(Vector2))]
    public static class Vector2Drawer
    {
        public static void DrawVector2Layout(MemberInfo member, Object target, object value)
        {
            Vector2 _value = (Vector2)value;
            float x = GUIFunctions.FloatFieldLayout(_value.x, int.MinValue, int.MaxValue);
            float y = GUIFunctions.FloatFieldLayout(_value.y, int.MinValue, int.MaxValue);
            Vector2 result = new Vector2(x, y);

            member.SetValue(target, result);
        }
        public static Vector2 GetVector2Layout(MemberInfo member, Object target, object value)
        {
            Vector2 _value = (Vector2)value;
            float x = GUIFunctions.FloatFieldLayout(_value.x, int.MinValue, int.MaxValue);
            float y = GUIFunctions.FloatFieldLayout(_value.y, int.MinValue, int.MaxValue);
            Vector2 result = new Vector2(x, y);
            return result;
            //member.SetValue(target, result);
        }
    }
    [CustomGUIDrawer(typeof(Vector2Int))]
    public static class Vector2IntDrawer
    {
        public static void DrawVector2IntLayout(MemberInfo member, Object target, object value)
        {
            Vector2Int _value = (Vector2Int)value;
            int x = GUIFunctions.IntFieldLayout(_value.x, int.MinValue, int.MaxValue);
            int y = GUIFunctions.IntFieldLayout(_value.y, int.MinValue, int.MaxValue);
            Vector2Int result = new Vector2Int(x, y);

            member.SetValue(target, result);
        }

        public static Vector2Int GetVector2IntLayout(MemberInfo member, Object target, object value)
        {
            Vector2Int _value = (Vector2Int)value;
            int x = GUIFunctions.IntFieldLayout(_value.x, int.MinValue, int.MaxValue);
            int y = GUIFunctions.IntFieldLayout(_value.y, int.MinValue, int.MaxValue);
            Vector2Int result = new Vector2Int(x, y);

            return result;
            // member.SetValue(target, result);
        }
    }

    public static class Drawers
    {
        public static void ButtonLayout(string text, MethodInfo member, Object target, ButtonAttribute attribute, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(text, options))
            {
                if (member.IsEzMethod())
                {
                    var type = target.GetType();
                    member.Invoke((object)target, null);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }
        public static void Button(Rect btn, MemberInfo member, Object target, Attribute attribute, params GUILayoutOption[] options)
        {

        }
    }

}