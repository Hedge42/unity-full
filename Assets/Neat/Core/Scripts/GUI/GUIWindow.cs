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
        private MemberInfo[] members;
        private int id;

        private bool hideRect => ReferenceEquals(obj, null);


        private static int windowCount;
        private float prefixWidth => 150;

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
            this.members = obj.FindMembers();

            //Functions.mem
            this.rect = GUIWindowDrawer.defaultRect;
        }
        public void Draw()
        {
            rect = GUI.Window(id, rect, DrawWindow, title);
        }
        private void DrawWindow(int windowID)
        {
            if (members == null)
                members = obj.FindMembers();

            // draw fields based on type
            foreach (var member in members)
                DrawMember(member);

            DrawButtons();

            // top-bar drag handle
            Rect dragArea = new Rect(0, 0, rect.width, 15);
            GUI.DragWindow(dragArea);
        }

        private void DrawMember(MemberInfo member)
        {
            // WORK IN PROGRESS
            GUILayout.BeginHorizontal();
            var _type = member.GetValueType();
            object value = member.GetValue(obj);
            GUILayout.Label(member.Name, GUILayout.Width(prefixWidth));

            if (value is string)
            {
                member.SetValue(obj, GUILayout.TextField(value.ToString()));
            }
            else if (value is bool)
            {
                member.SetValue(obj, GUILayout.Toggle((bool)value, ""));
            }
            else if (value is float)
            {
                var range = member.GetCustomAttribute<RangeAttribute>();
                if (range != null)
                {
                    float slider = GUILayout.HorizontalSlider((float)value, range.min, range.max);
                    float.TryParse(GUILayout.TextField($"{(slider).ToString("f2")}", GUILayout.MaxWidth(80)), out float result);

                    member.SetValue(obj, result);
                }
                else
                {
                    var input = GUILayout.TextField(value.ToString());
                    if (float.TryParse(input, out float result))
                    {
                        member.SetValue(obj, result);
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
                    member.SetValue(obj, Mathf.RoundToInt(input));
                    GUILayout.Label($"{f}", GUILayout.Width(50));
                }
                else
                {
                    var input = GUILayout.TextField(value.ToString());
                    if (int.TryParse(input, out int result))
                    {
                        member.SetValue(obj, result);
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
                member.SetValue(obj, newVector);
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
                member.SetValue(obj, newVector);
            }
            else
            {
                GUILayout.Label($"{member.Name} = {value}");
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