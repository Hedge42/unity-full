using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Neat.Tools.UI
{
    // should attach to DontDestroyOnLoad GameManager
    public class PauseListener : MonoBehaviour
    {
        // fields
        private static bool _isPaused;
        private static UnityEvent _onPause;
        private static UnityEvent _onResume;

        // properties
        public static bool isPaused
        {
            get
            {
                return _isPaused;
            }
            set
            {
                if (value)
                    Pause();
                else
                    Resume();
            }
        }
        public static bool isListeningForKey { get; set; }
        public static UnityEvent onPause
        {
            get
            {
                if (_onPause == null)
                    _onPause = new UnityEvent();
                return _onPause;
            }
        }
        public static UnityEvent onResume
        {
            get
            {
                if (_onResume == null)
                    _onResume = new UnityEvent();
                return _onResume;
            }
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                Pause();
        }

        private void Update()
        {

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            //if (isListeningForKey && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                MenuToggle();
                Toggle();
            }

        }
        public static void Pause()
        {
            _isPaused = true;
            onPause.Invoke();
        }
        public static void Resume()
        {
            _isPaused = false;
            onResume.Invoke();
        }
        private void Toggle()
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
        private void MenuToggle()
        {
            if (Menu.isPaused)
                Menu.Resume();
            else
                Menu.Pause();
        }
    }
}
