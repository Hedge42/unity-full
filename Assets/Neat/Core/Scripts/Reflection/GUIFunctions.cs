using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Neat.Tools
{
    // allow gui methods called from GUI and GUILayout with using statements
    // using GUI = GUIExtensions;
    // using GUILayout = GUILayoutExtensions;
    public class GUILayoutExtensions : GUILayout
    {
        public static float FloatField(float value, float min, float max, params GUILayoutOption[] options)
        {
            return GUIFunctions.FloatFieldLayout(value, min, max, options);
        }
        public static float IntField(int value, int min, int max, params GUILayoutOption[] options)
        {
            return GUIFunctions.IntFieldLayout(value, min, max, options);
        }

        public static float FloatSlider(float value, float min, float max, params GUILayoutOption[] options)
        {
            return GUIFunctions.FloatSliderLayout(value, min, max, options);
        }

        public static float IntSlider(int value, int min, int max, params GUILayoutOption[] options)
        {
            return GUIFunctions.IntSliderLayout(value, min, max, options);
        }
    }
    public class GUIExtensions : GUI
    {
        public static int IntField(Rect position, int value, int min, int max)
        {
            return GUIFunctions.IntField(position, value, min, max);
        }
        public static float FloatField(Rect position, float value, float min, float max)
        {
            return GUIFunctions.FloatField(position, value, min, max);
        }
    }

    public static partial class GUIFunctions
    {
        public static void DrawMemberLayout(MemberInfo member, object target, float prefixWidth = 100)
        {
            // WORK IN PROGRESS
            GUILayout.BeginHorizontal();
            var _type = member.GetValueType();
            object value = member.GetValue(target);
            GUILayout.Label(member.Name, GUILayout.Width(prefixWidth));

            if (value is string)
            {
                member.SetValue(target, GUILayout.TextField(value.ToString()));
            }
            else if (value is bool)
            {
                member.SetValue(target, GUILayout.Toggle((bool)value, ""));
            }
            else if (value is float)
            {
                var range = member.GetCustomAttribute<RangeAttribute>();
                if (range != null)
                {
                    float slider = GUILayout.HorizontalSlider((float)value, range.min, range.max);
                    float.TryParse(GUILayout.TextField($"{(slider).ToString("f2")}", GUILayout.MaxWidth(80)), out float result);

                    member.SetValue(target, result);
                }
                else
                {
                    var input = GUILayout.TextField(value.ToString());
                    if (float.TryParse(input, out float result))
                    {
                        member.SetValue(target, result);
                    }
                }
            }
            else if (value is int)
            {
                var range = member.GetCustomAttribute<RangeAttribute>();
                if (range != null)
                {
                    float f = (int)value;
                    float input = GUILayout.HorizontalSlider(f, range.min, range.max);
                    member.SetValue(target, Mathf.RoundToInt(input));
                    GUILayout.Label($"{f}", GUILayout.Width(50));
                }
                else
                {
                    var input = GUILayout.TextField(value.ToString());
                    if (int.TryParse(input, out int result))
                    {
                        member.SetValue(target, result);
                    }
                }
            }
            else if (value is Vector2)
            {
                Vector2 v = (Vector2)value;

                GUILayout.BeginHorizontal();
                var _x = GUILayout.TextField(v.x.ToString());
                var _y = GUILayout.TextField(v.y.ToString());
                float.TryParse(_x, out float xf);
                float.TryParse(_y, out float yf);
                GUILayout.EndHorizontal();

                var newVector = new Vector2(xf, yf);
                member.SetValue(target, newVector);
            }
            else if (value is Vector2Int)
            {
                Vector2Int v = (Vector2Int)value;

                GUILayout.BeginHorizontal();
                var _x = GUILayout.TextField(v.x.ToString());
                var _y = GUILayout.TextField(v.y.ToString());
                int.TryParse(_x, out int xi);
                int.TryParse(_y, out int yi);
                GUILayout.EndHorizontal();

                var newVector = new Vector2Int(xi, yi);
                member.SetValue(target, newVector);
            }
            else
            {
                GUILayout.Label($"{member.Name} = {value}");
            }

            GUILayout.EndHorizontal();
        }
        public static void DrawMemberLayout2(MemberInfo member, Object target)
        {
            var canRead = member.CanRead();
            var canWrite = member.CanWrite();
            var isDisabled = canRead && !canWrite;
            var lastEnabled = GUI.enabled;

            if (isDisabled)
                GUI.enabled = false;

            // draw prefix and property
            GUILayout.BeginHorizontal();
            object value = member.GetValue(target);
            var content = new GUIContent(member.Name, $"tooltip for {member.Name}");
            GUILayout.Label(content, GUILayout.Width(CustomGUISettings.prefixWidth));
            //DrawMemberValue(member, target, value);
            value = GetMemberValue(member, target, value, content);
            GUILayout.EndHorizontal();
            GUI.enabled = lastEnabled;

            if (canWrite)
                member.SetValue(target, value);
        }


        public static void DrawMemberLayout3(MemberInfo member, Object target)
        {
            throw new System.NotImplementedException();
            if (member is MethodInfo)
            {
                var method = member as MethodInfo;
                
            }
            else if (member is PropertyInfo)
            {
                var property = member as PropertyInfo;
            }
            else if (member is EventInfo)
            {
                var _event = member as EventInfo;
            }
            else if (member is FieldInfo)
            {
                var field = member as FieldInfo;
            }
            else
            {
                DrawDefaultLayout(member, target);
            }
        }
        public static void DrawDefaultLayout(MemberInfo member, Object target)
        {

        }
        public static void DrawMethodLayout(MethodInfo method, Object target)
        {

        }
        public static void DrawPropertyLayout(PropertyInfo property, Object target)
        {

        }
        public static void DrawFieldLayout(FieldInfo property, Object target)
        {

        }
        public static void DrawEventLayout(EventInfo property, Object target)
        {

        }

        public static bool CanRead(this MemberInfo member) // , Object target)
        {
            // decides whether to go on to type-drawers
            var readableProp = member is PropertyInfo && (member as PropertyInfo).CanRead == true;
            return member is FieldInfo || readableProp;
        }
        public static bool CanWrite(this MemberInfo member)
        {
            var writableProp = member is PropertyInfo && (member as PropertyInfo).CanWrite == true;
            var writableField = member is FieldInfo && (member as FieldInfo).IsPublic; // exposed attribute?
            return writableField || writableProp;
        }

        // sets values
        private static void DrawMemberValue(MemberInfo member, Object target, object value)
        {
            if (value is string)
            {
                member.SetValue(target, GUILayout.TextField(value.ToString()));
            }
            else if (value is bool)
            {
                member.SetValue(target, GUILayout.Toggle((bool)value, ""));
            }
            else if (value is float)
            {
                FloatDrawer.GetFloatLayout(member, target, value);
            }
            else if (value is int)
            {
                IntDrawer.GetIntLayout(member, target, value);
            }
            else if (value is Vector2)
            {
                Vector2Drawer.GetVector2Layout(member, target, value);
            }
            else if (value is Vector2Int)
            {
                Vector2IntDrawer.GetVector2IntLayout(member, target, value);
            }
            else
            {
                GUILayout.Label($"{value} ({member.GetValueType()})");
            }
        }

        // does not set values
        private static object GetMemberValue(MemberInfo member, Object target, object value, GUIContent content, params GUILayoutOption[] options)
        {
            if (value is string)
            {
                return GUILayout.TextField(value.ToString(), options);
            }
            else if (value is bool)
            {
                return GUILayout.Toggle((bool)value, content);
            }
            else if (value is float)
            {
                return FloatDrawer.GetFloatLayout(member, target, value);
            }
            else if (value is int)
            {
                return IntDrawer.GetIntLayout(member, target, value);
            }
            else if (value is Vector2)
            {
                return Vector2Drawer.GetVector2Layout(member, target, value);
            }
            else if (value is Vector2Int)
            {
                return Vector2IntDrawer.GetVector2IntLayout(member, target, value);
            }
            else
            {
                GUILayout.Label($"{value} ({member.GetValueType()})");
                return value;
            }
        }

        public static void DrawMember(Rect fullRect, MemberInfo member, object target, float prefixWidth = 100)
        {
            float suffixWidth = 70;

            // WORK IN PROGRESS
            var _type = member.GetValueType();
            object value = member.GetValue(target);

            // [prefix][pre][body][post]

            // slow?
            Rect prefixRect = new Rect(fullRect);
            prefixRect.width = prefixWidth;

            Rect bodyRect = new Rect(fullRect);
            bodyRect.xMin = prefixRect.xMax;

            Rect postRect = new Rect(fullRect);
            postRect.xMin = fullRect.xMax - suffixWidth;

            Rect bodyWithPost = new Rect(bodyRect);
            bodyWithPost.xMax -= suffixWidth;

            // [pre] [body]
            Rect preRect = new Rect(bodyRect);
            preRect.width = suffixWidth;

            // 2 text fields...
            Rect bodyLeft = new Rect(bodyRect);
            Rect bodyRight = new Rect(bodyRect);
            bodyLeft.width = bodyRight.width = bodyRect.width / 2;
            bodyRight.x = bodyLeft.xMax;

            // if draw prefix?
            GUI.Label(prefixRect, member.Name);

            if (value is string)
            {
                member.SetValue(target, GUI.TextField(bodyRect, value.ToString()));
            }
            else if (value is bool)
            {
                member.SetValue(target, GUI.Toggle(bodyRect, (bool)value, ""));
            }
            else if (value is float)
            {
                var range = member.GetCustomAttribute<RangeAttribute>();
                if (range != null)
                {
                    float slider = GUI.HorizontalSlider(bodyWithPost, (float)value, range.min, range.max);
                    float.TryParse(GUI.TextField(postRect, $"{(slider)}"), out float result);

                    member.SetValue(target, result);
                }
                else
                {
                    var input = GUI.TextField(bodyRect, value.ToString());
                    if (float.TryParse(input, out float result))
                    {
                        member.SetValue(target, result);
                    }
                }
            }
            else if (value is int)
            {
                var range = member.GetCustomAttribute<RangeAttribute>();
                if (range != null)
                {
                    float f = (int)value;
                    float input = GUI.HorizontalSlider(bodyWithPost, f, range.min, range.max);
                    member.SetValue(target, Mathf.RoundToInt(input));
                    GUI.Label(postRect, $"{f}");
                }
                else
                {
                    var input = GUI.TextField(bodyRect, value.ToString());
                    if (int.TryParse(input, out int result))
                    {
                        member.SetValue(target, result);
                    }
                }
            }
            else if (value is Vector2)
            {
                Vector2 v = (Vector2)value;

                var _x = GUI.TextField(bodyLeft, v.x.ToString());
                var _y = GUI.TextField(bodyRight, v.y.ToString());
                float.TryParse(_x, out float xf);
                float.TryParse(_y, out float yf);

                var newVector = new Vector2(xf, yf);
                member.SetValue(target, newVector);
            }
            else if (value is Vector2Int)
            {
                Vector2Int v = (Vector2Int)value;

                var _x = GUI.TextField(bodyLeft, v.x.ToString());
                var _y = GUI.TextField(bodyRight, v.y.ToString());
                int.TryParse(_x, out int xi);
                int.TryParse(_y, out int yi);

                var newVector = new Vector2Int(xi, yi);
                member.SetValue(target, newVector);
            }
            else
            {
                GUI.Label(bodyRect, $"{value}");
            }
        }

        public static void _DrawPropertyLayout(PropertyInfo prop, Object target)
        {
            var canDraw = prop.CanRead;
            var disabled = !prop.CanWrite;
        }

        public static int IntFieldLayout(int value, int min, int max, params GUILayoutOption[] options)
        {
            string input = GUILayout.TextField(value.ToString(), options);//, GUILayout.Width(CustomGUISettings.labelWid));
            return ProcessIntField(value, min, max, input);
        }
        public static int IntSliderLayout(int value, int min, int max, params GUILayoutOption[] options)
        {
            value = Mathf.RoundToInt(GUILayout.HorizontalSlider(value, min, max));
            value = IntFieldLayout(value, min, max, GUILayout.Width(CustomGUISettings.labelWidth));
            return value;
        }

        public static int IntField(Rect position, int value, int min, int max)
        {
            string input = GUI.TextField(position, value.ToString());
            return ProcessIntField(value, min, max, input);
        }
        public static int IntSlider(Rect position, int value, int min, int max)
        {
            Rect left, right;
            position.Split(out left, out right);

            float input = GUI.HorizontalSlider(left, value, min, max);
            value = Mathf.RoundToInt(input);

            value = IntField(right, value, min, max);

            return value;
        }
        private static int ProcessIntField(int value, int min, int max, string input)
        {
            if (int.TryParse(input, out int result))
                return Mathf.Clamp(result, min, max);
            else
                return value;
        }

        public static float FloatField(Rect position, float value, float min, float max)
        {
            string input = GUI.TextField(position, value.ToString());

            if (float.TryParse(input, out float result))
                return Mathf.Clamp(result, min, max);
            else
                return value;
        }
        public static float FloatFieldLayout(float value, float min, float max, params GUILayoutOption[] options)
        {
            string input = GUILayout.TextField(value.ToString(), options);

            if (float.TryParse(input, out float result))
                return Mathf.Clamp(result, min, max);
            else
                return value;
        }
        public static float FloatSlider(Rect position, float value, float min, float max)
        {
            position.Split(out Rect left, out Rect right);

            value = GUI.HorizontalSlider(left, value, min, max);
            value = FloatField(right, value, min, max);

            return value;
        }
        public static float FloatSliderLayout(float value, float min, float max, params GUILayoutOption[] options)
        {
            value = GUILayout.HorizontalSlider(value, min, max);
            value = FloatFieldLayout(value, min, max, GUILayout.Width(CustomGUISettings.labelWidth));

            return value;
        }

        public static void Split(this Rect rect, out Rect left, out Rect right, float rightWidth = CustomGUISettings.labelWidth)
        {
            left = new Rect(rect);
            right = new Rect(rect);

            left.xMax = right.xMin = rightWidth;
        }

        public static GUIStyle FontStyle(this GUIStyle style, FontStyle s)
        {
            GUIStyle g = new GUIStyle(style);
            g.fontStyle = s;
            return g;
        }
        public static GUIStyle Bold(this GUIStyle style)
        {
            return style.FontStyle(UnityEngine.FontStyle.Bold);
        }
        public static GUIStyle FontSize(this GUIStyle style, int fontSize)
        {
            GUIStyle g = new GUIStyle(style);
            g.fontSize = fontSize;
            return g;
        }
        public static GUIStyle TextColor(this GUIStyle style, Color c)
        {
            GUIStyle g = new GUIStyle(style);
            g.normal.textColor = c;
            return g;
        }
        public static GUIStyle Rich(this GUIStyle style)
        {
            GUIStyle s = new GUIStyle(style);
            s.richText = true;
            return s;
        }
        public static GUIContent Tooltip(this string s, string tooltip)
        {
            return new GUIContent(s, tooltip);
        }
    }
}
