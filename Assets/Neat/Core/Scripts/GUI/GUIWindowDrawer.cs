using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public List<GUIWindow> windows;
        public GUISkin skin;

        private void OnGUI()
        {
            InitGUI();

            foreach (var window in windows)
                window.Draw();
        }
        public GUIWindow Open<T>(T obj) where T : Object
        {
            foreach (var window in windows)
            {
                if (window.obj == obj)
                {
                    return window;
                }
            }

            var newWindow = new GUIWindow(obj);
            windows.Add(newWindow);
            return newWindow;
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
    }
}