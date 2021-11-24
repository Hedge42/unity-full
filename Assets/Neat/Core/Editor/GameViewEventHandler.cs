using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Neat.Tools
{
    public class GameViewEventHandler
    {
        // made in an attempt to get gameView to accept keyboard input in edit mode

        // [InitializeOnLoadMethod]
        static void EditorInit()
        {
            //FieldInfo info = typeof(EditorApplication).GetField("globalEventHandler", BindingFlags.Static | BindingFlags.NonPublic);
            //EditorApplication.CallbackFunction value = (EditorApplication.CallbackFunction)info.GetValue(null);
            //value += EditorKeyPress;
            //info.SetValue(null, value);

            //SceneView.beforeSceneGui += OnSceneGUI;
            //SceneView.duringSceneGui += OnSceneGUI;
        }
        static void EditorKeyPress()
        {
            var key = Event.current.keyCode;
            if (key != KeyCode.None)
            {
                Debug.Log($"Global key event: {key}");

                // TODO: reroute event to gameView?

                // ???
                var view = EditorWindow.GetWindow(System.Type.GetType("UnityEditor.GameView,UnityEditor"));
                Event.current.Use();
                EditorUtility.SetDirty(view);
                view.Repaint();
                //view.SendEvent(Event.current);
                //view.SendEvent(Event.KeyboardEvent(key.ToString()));
                //Event.current.Use(); // block event ???
                //GUIDrawer.PressKey(key);
            }
        }
        static void SceneViewKeyPress(SceneView sc)
        {
            if (Event.current.keyCode != KeyCode.None)
            {
                Debug.Log($"Scene view key press: {Event.current.keyCode}");
                var key = Event.current.keyCode;
                // GUIDrawer.PressKey(key);
            }
        }
        private static void OnSceneGUI(SceneView sceneView)
        {
            // https://answers.unity.com/questions/921989/get-keycode-events-in-editor-without-object-select.html

            // Do your general-purpose scene gui stuff here...
            // Applies to all scene views regardless of selection!

            // You'll need a control id to avoid messing with other tools!
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            EventType et = Event.current.GetTypeForControl(controlID);
            Debug.Log($"SceneGUI event: {et}");
        }
    }
}
