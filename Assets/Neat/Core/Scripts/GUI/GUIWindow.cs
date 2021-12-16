using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Tools;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    // GUIDrawer
    [ExecuteInEditMode]
    public class GUIWindow : MonoBehaviour
    {
        public Object target;

        public Rect rect;
        private System.Type type;
        private string title;
        public MemberInfo[] members;

        private int id;

        private bool drawWindow => ReferenceEquals(target, null);
        private static int windowCount;

        private Object prevTarget;

        private bool isDirty => !ReferenceEquals(target, prevTarget);// || members == null;

        public static GUIWindow Open(Object target, GameObject gameObject)
        {
            var window = gameObject.GetOrAddComponent<GUIWindow>();
            window.SetTarget(target);
            return window;
        }
        private void OnEnable()
        {
            SetTarget(target);
        }
        private void OnGUI()
        {
            if (target != null)
            {
                if (isDirty)
                    SetTarget(target);

                rect = GUI.Window(target.GetHashCode(), rect, DrawWindow, title);
            }

            prevTarget = target;
        }
        public void SetTarget(Object _target)
        {
            this.target = _target;

            if (target != null)
            {
                this.title = $"{_target.GetType()} Inspector";
                this.members = Functions.FindValidMembers(_target.GetType()).ToArray();
                Debug.Log(members.Length);
                // this.members = Functions.FindMembers<GUIAttribute>(target.GetType()).ToArray();
            }
        }
        private void DrawWindow(int windowID)
        { 
            // content

            Rect position = new Rect(rect);
            position.height = 20;

            GUILayout.Label($"{members.Length}");

            foreach (var member in members)
                DrawMemberLayout(member);

            // drag, buttons, etc
            WindowFunctions();
        }

        private void WindowFunctions()
        {
            DrawCornerButtons();

            // top-bar drag handle
            Rect dragArea = new Rect(0, 0, rect.width, 15);
            GUI.DragWindow(dragArea);
        }

        

        // these really shouldn't be here
        private void Draw(MemberInfo member, Attribute[] attributes)
        {
            foreach (var attribute in attributes)
                Draw(member, attribute);
        }
        private void Draw(MemberInfo member, Attribute attribute)
        {
            if (attribute is ButtonAttribute)
            {
                var button = attribute as ButtonAttribute;
                if (member is MethodInfo)
                {
                    var method = member as MethodInfo;
                    Drawers.ButtonLayout(method.Name, method, target, button);
                }
            }
        }
        private void DrawMemberLayout(MemberInfo member)
        {
            GUILayout.BeginHorizontal();
            object value = member.GetValue(target);
            GUILayout.Label(member.Name, GUILayout.Width(CustomGUISettings.labelWidth));


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
                FloatDrawer.DrawFloatLayout(member, target, value);
            }
            else if (value is int)
            {
                IntDrawer.DrawIntLayout(member, target, value);
            }
            else if (value is Vector2)
            {
                Vector2Drawer.DrawVector2Layout(member, target, value);
            }
            else if (value is Vector2Int)
            {
                Vector2IntDrawer.DrawVector2IntLayout(member, target, value);
            }
            else
            {
                GUILayout.Label($"{value} ({member.GetValueType()})");
            }

            GUILayout.EndHorizontal();
        }
        private Rect DrawMember(Rect position, MemberInfo member)
        {
            return default;
        }

        private void DrawCornerButtons()
        {
            var offset = 3;
            var size = 18;


            Rect topRight = new Rect(rect.width - (size + offset), offset, size, size);
            if (GUI.Button(topRight, "x"))
            {
                // Debug.Log("u thought lol");
                target = null;
            }
        }
    }
}