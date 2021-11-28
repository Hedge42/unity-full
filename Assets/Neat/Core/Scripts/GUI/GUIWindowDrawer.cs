using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Tools;

namespace Neat.Tools
{
    [ExecuteAlways]
    public class GUIWindowDrawer : MonoBehaviour
    {
        private static GUIWindowDrawer _instance;
        public static GUIWindowDrawer instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<GUIWindowDrawer>();
                return _instance;
            }
        }
        public static readonly Rect defaultRect = new Rect(0, 0, 600, 900);

        // could be struct
        public List<GUIWindow> windows;
        public List<Object> objects;

        public GUISkin skin;

        private bool initialized;

        private void OnGUI()
        {
            SetWindowObjects();
            HandleSkin();
            HandleEditorMouseClick();

            foreach (var window in windows)
                window.Draw();
        }
        public GUIWindow Open<T>(T obj) where T : Object
        {
            // return existing, if same object reference
            foreach (var window in windows)
            {
                if (ReferenceEquals(window.obj, obj))
                //if (window.obj == obj)
                {
                    return window;
                }
            }

            // create new window with reference object
            var newWindow = new GUIWindow(obj);
            windows.Add(newWindow);
            objects.Add(obj);
            return newWindow;
        }

        private void SetWindowObjects()
        {
            if (!initialized)
            {
                foreach (var window in windows)
                {
                    window.SetObject(window.obj);
                }

                initialized = true;
            }
        }
        private void HandleSkin()
        {
            if (skin != null)
                GUI.skin = skin;

            GUI.skin.textField.wordWrap = false;
            GUI.skin.label.wordWrap = false;
        }
        private void HandleEditorMouseClick()
        {

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
    }
}