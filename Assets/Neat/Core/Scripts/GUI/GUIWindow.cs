using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Tools;

namespace Neat.Tools
{
    [System.Serializable]
    public class GUIWindow
    {
        public Object obj;

        [HideIf("hideRect")]
        public Rect rect;
        private System.Type type;
        private string title;
        private FieldInfo[] fields;
        private int id;

        private bool hideRect => ReferenceEquals(obj, null);


        private static int windowCount;
        private readonly float prefixWidth = 150;

        public GUIWindow(Object obj)
        {
            SetObject(obj);
        }
        public void SetObject(Object obj)
        {
            this.obj = obj;
            this.title = $"{type} Inspector";
            this.id = windowCount++;
            this.type = obj.GetType();
            this.fields = type.GetFields();
            this.rect = GUIWindowDrawer.defaultRect;
        }
        public void Draw()
        {
            rect = GUI.Window(id, rect, DrawWindow, title);
        }
        private void DrawWindow(int windowID)
        {
            

            if (type == null)
                type = obj.GetType();
            if (fields == null)
                fields = type.GetFields();

            // draw fields based on type
            foreach (var field in fields)
                DrawField(field);

            DrawButtons();

            // top-bar drag handle
            Rect dragArea = new Rect(0, 0, rect.width, 15);
            GUI.DragWindow(dragArea);
        }

        private void DrawField(FieldInfo x)
        {
            // WORK IN PROGRESS

            GUILayout.BeginHorizontal();
            System.Type _type = x.FieldType;
            object value = x.GetValue(obj);
            GUILayout.Label(x.Name, GUILayout.Width(prefixWidth));

            if (value is string)
            {
                x.SetValue(obj, GUILayout.TextField(value.ToString()));
                //if (GUI.GetNameOfFocusedControl() != "str_input0")
                //GUI.SetNextControlName($"str_input{count++}");
                //GUI.FocusControl("str_input0");
            }
            else if (value is bool)
            {
                x.SetValue(obj, GUILayout.Toggle((bool)value, ""));
            }
            else if (value is float)
            {
                var range = x.GetCustomAttribute<RangeAttribute>();
                if (range != null)
                {
                    float slider = GUILayout.HorizontalSlider((float)value, range.min, range.max);
                    float.TryParse(GUILayout.TextField($"{(slider).ToString("f2")}", GUILayout.MaxWidth(80)), out float result);

                    x.SetValue(obj, result);
                    // GUILayout.Label($"{((float)value).ToString("f2")}", GUILayout.Width(50));
                    // var input = GUILayout.TextField($"{((float)value).ToString("f2")}", GUILayout.Width(50));

                }
                else
                {
                    var input = GUILayout.TextField(value.ToString());
                    if (float.TryParse(input, out float result))
                    {
                        x.SetValue(obj, result);
                    }
                }
            }
            else if (value is int)
            {
                var range = x.GetCustomAttribute<RangeAttribute>();
                if (range != null)
                {
                    float f = (int)value;
                    float input = GUILayout.HorizontalSlider(f, range.min, range.max);
                    x.SetValue(obj, Mathf.RoundToInt(input));
                    GUILayout.Label($"{f}", GUILayout.Width(50));
                }
                else
                {
                    var input = GUILayout.TextField(value.ToString());
                    if (int.TryParse(input, out int result))
                    {
                        x.SetValue(obj, result);
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
                x.SetValue(obj, newVector);
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
                x.SetValue(obj, newVector);
            }
            else
            {
                GUILayout.Label($"{value}");
            }

            GUILayout.EndHorizontal();
        }

        private void DrawButtons()
        {
            var offset = 3;
            var size = 15;
            Rect topRight = new Rect(rect.width - (size + offset), offset, size, size);
            if (GUI.Button(topRight, "x"))
            {
                Debug.Log("nice");
            }
        }
    }
}