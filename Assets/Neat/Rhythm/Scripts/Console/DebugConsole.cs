using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Tools
{
    // editor enable: find members
    // 

    public class DebugConsole : MonoBehaviour
    {
        public static DebugConsole instance;

        public bool showConsole;

        public KeyCode show = KeyCode.Slash;
        public KeyCode submit = KeyCode.Return;
        public KeyCode cancel = KeyCode.Escape;
        public KeyCode forceVisible = KeyCode.RightShift | KeyCode.LeftShift;

        private string cmdText = "";
        private string logText = "type \"help\" for list of commands";
        private Vector2 scrollPosition = default;
        private Rect scrollView = new Rect(0, 20, Screen.width, 150);
        private Rect scrollRect = new Rect(0, 0, Screen.width - 18, 500);
        private GUIStyle boxStyle;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            DontDestroyOnLoad(this);

            showConsole = false;
            cmdText = "";

        }
        private void OnGUI()
        {
            if (boxStyle == null)
            {
                boxStyle = new GUIStyle(GUI.skin.box);
                var newColor = new Color(0, 0, 0, .8f);
                boxStyle.normal.background = MakeTex(1, 1, newColor);
            }
            // print($"FOCUSED: {GUI.GetNameOfFocusedControl()}");

            if (showConsole)
            {
                if (Pressed(cancel))
                {
                    Hide();
                }
                else if (Pressed(submit))
                {
                    Process(cmdText);

                    if (Pressing(forceVisible))
                        Hide();
                }
                else
                {
                    Focus();
                }
            }
            else
            {
                if (Pressed(submit))
                {
                    Focus();
                    cmdText = "";
                }
            }
        }

        public static void Log(string text)
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<DebugConsole>();

            instance.logText = $"{text}\n{instance.logText}";
        }
        public static void Clear()
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<DebugConsole>();

            instance.logText = "";
        }

        private void Focus()
        {
            // https://docs.unity3d.com/ScriptReference/GUI.FocusControl.html
            showConsole = true;
            GUI.SetNextControlName("input");
            cmdText = GUI.TextField(new Rect(0, 0, Screen.width, 20), cmdText);
            GUI.FocusControl("input");

            DrawLog();
        }
        private void DrawLog()
        {
            // var color = GUI.backgroundColor;

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
            showConsole = false;
            cmdText = "";
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

            cmdText = "";
        }
    }
}
