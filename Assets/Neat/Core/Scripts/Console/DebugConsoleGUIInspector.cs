using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    [CustomGUIInspector(typeof(DebugConsole))]
    public class DebugConsoleGUIInspector : GUIInspector
    {
        private DebugConsole console;
        public DebugConsoleGUIInspector(Object target) : base(target)
        {
            console = (DebugConsole)target;
        }
        protected override MemberInfo[] FindMembers()
        {
            var members = Functions.FindAttributeMembers<DebugCommandAttribute>(flags)
                .Where(m => m is MethodInfo && (m as MethodInfo).IsStatic)
                .ToArray();

            ViewCommands(members);

            return members;
        }

        private void ViewCommands(MemberInfo[] members)
        {
            Debug.Log($"Found {members.Length} tagged commands: ");
            foreach (var member in members)
            {
                if (member is MethodInfo)
                {
                    var method = member as MethodInfo;
                    var _params = method.GetParameters().Select(p => p.ToString());
                    var args = $"({String.Join(',', _params)})";

                    Debug.Log($"cmd: {method.Name} {args}");
                }
            }
        }

        public KeyCode show = KeyCode.Slash;
        public KeyCode submit = KeyCode.Return;
        public KeyCode cancel = KeyCode.Escape;
        public KeyCode forceVisible = KeyCode.RightShift | KeyCode.LeftShift;

        private string logText = "type \"help\" for list of commands";
        private Vector2 scrollPosition = default;
        private Rect scrollView = new Rect(0, 20, Screen.width, 150);
        private Rect scrollRect = new Rect(0, 0, Screen.width - 18, 500);
        private GUIStyle boxStyle;

        int selected;

        public override void OnGUI()
        {
            var controlName = "neato.console.input";
            GUI.SetNextControlName(controlName);
            console.input = GUILayout.TextField(console.input);

            console.HandleAutoComplete();

            if (GUI.changed)
                Debug.Log("Changed!");

            foreach (string id in console.autocomplete)
                GUILayout.Label(id);
        }

        private void Focus()
        {
            // https://docs.unity3d.com/ScriptReference/GUI.FocusControl.html
            console.showConsole = true;
            GUI.SetNextControlName("input");
            console.input = GUI.TextField(new Rect(0, 0, Screen.width, 20), console.input);
            GUI.FocusControl("input");

            DrawLog();
        }
        private void DrawLog()
        {
            // var color = GUI.backgroundColor;
            console.input = GUI.TextField(new Rect(0, 0, Screen.width, 20), console.input);
            scrollPosition = GUI.BeginScrollView(scrollView, scrollPosition, scrollRect, false, true);
            GUI.Box(scrollRect, "", boxStyle);
            GUI.Label(scrollRect, logText);
            GUI.EndScrollView();
        }
        private Texture2D MakeTex(int width, int height, Color col)
        {
            // https://forum.unity.com/threads/change-gui-box-color.174609/
            // because setting GUI.color makes too much sense

            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private void Hide()
        {
            console.showConsole = false;
            console.input = "";
            GUI.FocusControl(" ");
        }
        private bool Pressed(KeyCode k)
        {
            bool ret = Event.current != null &&
                (Event.current.type == EventType.KeyDown
                && Event.current.keyCode == k);

            // https://forum.unity.com/threads/help-with-gui-focuscontrol.181836/
            // this seemed to solve something...
            if (ret)
                Event.current.Use();

            return ret;
        }
        private bool Pressing(KeyCode k)
        {
            bool ret = Event.current != null && Event.current.keyCode == k;
            if (ret)
                Event.current.Use();

            return ret;
        }
        private void Process(string input)
        {
            bool valid = false;
            foreach (var cmd in DebugCommandList.commands)
            {
                if (cmd.Attempt(input))
                {
                    valid = true;
                }
            }

            if (!valid)
            {
                DebugConsole.Log($"Unrecognized command \"{input}\"");
            }

            console.input = "";
        }
    }
}
