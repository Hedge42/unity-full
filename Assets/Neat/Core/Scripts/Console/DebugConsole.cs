using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

using UnityEngine.InputSystem;

namespace Neat.Tools
{
    // editor enable: find members
    // 

    [ExecuteInEditMode]
    [MultiInspector, GUIInspector, CustomGUIInspector(typeof(DebugConsoleGUIInspector))]
    public class DebugConsole : MonoBehaviour
    {
        private static DebugConsole _instance;
        public static DebugConsole instance
        {
            get
            {
                // == operator checks if gameObject was previously destroyed
                if (_instance == null)
                    _instance = Find();
                return _instance;
            }
        }

        public InputActionAsset inputs;

        public bool showConsole;
        public string input = "";



        private string logText = "type \"help\" for list of commands";

        private Vector2 scrollPosition = default;
        private Rect scrollView = new Rect(0, 20, Screen.width, 150);
        private Rect scrollRect = new Rect(0, 0, Screen.width - 18, 500);
        private GUIStyle boxStyle;


        private ConsoleAutocompleter _autocomplete;
        public ConsoleAutocompleter autocomplete
        {
            get
            {
                if (ReferenceEquals(_autocomplete, null))
                    _autocomplete = ConsoleAutocompleter.Instantiate(this);
                return _autocomplete;
            }
        }

        private void Awake()
        {
            showConsole = false;
            input = "";
        }
        private void OnGUI()
        {
            //if (boxStyle == null)
            //{
            //    boxStyle = new GUIStyle(GUI.skin.box);
            //    var newColor = new Color(0, 0, 0, .8f);
            //    boxStyle.normal.background = MakeTex(1, 1, newColor);
            //}
            //// print($"FOCUSED: {GUI.GetNameOfFocusedControl()}");

            //if (showConsole)
            //{
            //    if (Pressed(cancel))
            //    {
            //        Hide();
            //    }
            //    else if (Pressed(submit))
            //    {
            //        Process(input);

            //        if (Pressing(forceVisible))
            //            Hide();
            //    }
            //    else
            //    {
            //        Focus();
            //    }
            //}
            //else
            //{
            //    if (Pressed(submit))
            //    {
            //        Focus();
            //        input = "";
            //    }
            //}
        }

        public static void Clear()
        {
            instance.logText = "";
        }

        private void Focus()
        {
            // https://docs.unity3d.com/ScriptReference/GUI.FocusControl.html
            showConsole = true;
            GUI.SetNextControlName("input");
            input = GUI.TextField(new Rect(0, 0, Screen.width, 20), input);
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
            input = "";
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

            this.input = "";
        }

        [DebugCommand]
        private static DebugCommandBase ImACommand(float f, string x)
        {
            return null;
        }

        [DebugCommand(description = "Prints a line to the console")]
        public static void Log(string text)
        {
            instance._Log(text);
        }

        public void _Log(string text)
        {
            logText = $"{text}\n{instance.logText}";
        }

        public static DebugConsole Find()
        {
            DebugConsole found = GameObject.FindObjectOfType<DebugConsole>();

            // create an empty gameObject, add component
            if (found == null)
            {
                Debug.Log($"Instantiating DebugConsole object...");
                var go = Instantiate(new GameObject("Debug Console (generated)"));
                found = go.AddComponent<DebugConsole>();
            }

            return found;
        }
    }
}
