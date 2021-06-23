using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyListener : MonoBehaviour
{
    public string note;

    [System.Serializable]
    public class KeyEvent
    {
        public KeyCode key;
        public UnityEvent onKeyDown;
    }
    public enum KeyType
    {
        KeyDown,
        KeyUp,
        Key
    }

    public KeyEvent[] events;


    private void Update()
    {
        foreach (KeyEvent e in events)
            if (Input.GetKeyDown(e.key))
                e.onKeyDown.Invoke();
    }
}
