using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Tools;

//#if UNITY_EDITOR
//using UnityEditor;
//// [CustomEditor(typeof(GUIDrawer))]
//public class GUIDrawerEditor : Editor
//{
//    bool reflected;


//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        Debug.Log($"Inspector event: {Event.current}");
//    }
//}

//#endif

[ExecuteInEditMode]
[Extend]
public class GUIDrawer : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/GUI.Window.html

    public GUISkin skin;
    public Rect windowRect = new Rect(0, 0, 600, 1000);
    public Object obj; // object to create an inspector for

    private float prefixWidth = 100;
    private FieldInfo[] fields;
    private bool reflected;

    public GUIDrawer drawer;

    private MethodInfo[] methods;

    private void OnEnable()
    {
        reflected = false;
        Reflect();
    }

    void OnGUI()
    {
        InitGUI();
        if (obj != null)
        {
            // Register the window. Notice the 3rd parameter
            windowRect = GUI.Window(0, windowRect, DrawWindow, "Cool Window");
        }
    }

    private void InitGUI()
    {
        if (skin != null)
            GUI.skin = skin;

        GUI.skin.textField.wordWrap = false;
        GUI.skin.label.wordWrap = false;
#if UNITY_EDITOR
        // makes mouse work in gameview edit mode!
        // https://answers.unity.com/questions/1679704/is-there-a-way-to-implement-mouse-events-in-the-ga.html
        if (Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint)
        {
            UnityEditor.EditorUtility.SetDirty(this);
        }
        else
        {
            // Debug.Log($"onGUI: {Event.current}");
        }
#endif
    }

    private void Reflect(bool reflected = false)
    {
        if (!reflected)
        {
            // guess which fields get serialized without Editor?
            var type = obj.GetType();
            fields = type.GetFields();

            this.reflected = true;

            //methods = type.GetMethods();
            //foreach (var m in methods)
            //{
            //    if (m.GetCustomAttribute<ButtonAttribute>())
            //    {

            //    }
            //}

            // TODO: BindingFlags, attributes, properties, methods
            // var bindingFlags = BindingFlags.Public | ...;
            // var methods = type.GetMethods
            // var props = type.GetProperties
            // var attributes = type.GetCustomAttributes
        }
    }
    private void DrawWindow(int windowID)
    {
        // finding fields to draw
        if (!reflected)
            Reflect();

        // draw fields based on type
        foreach (var field in fields)
            DrawField(field);

        // top-bar drag handle
        Rect dragArea = new Rect(0, 0, windowRect.width, 15);
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


    private void FullScreen()
    {
        windowRect = Screen.safeArea;
    }

    // trying to get keyboard input in edit mode
    // see GameViewEventHandler.cs
    public static void PressKey(KeyCode key)
    {
        waiting = true;
        waitingKey = key;
    }
    private static bool waiting;
    private static KeyCode waitingKey;
    private static void HandleWait()
    {
        if (waiting)
        {

            var _e = Event.KeyboardEvent(waitingKey.ToString());
            _e.Use();
            Debug.Log("here!");
            waiting = false;
        }
    }
}
