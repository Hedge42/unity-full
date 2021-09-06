using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// should make reusing media-type controls possible
// this could just be a regular class...
public class MediaInput
{
    [System.Serializable]
    public class Setting
    {
        public KeyCode skipForward = KeyCode.RightArrow;
        public KeyCode skipBack = KeyCode.LeftArrow;
        public KeyCode pause = KeyCode.Space;
        public KeyCode stop = KeyCode.Backspace;
    }

    public Setting input = new Setting();

    public UnityEvent onPlayPause = new UnityEvent();
    public UnityEvent onSkipForward = new UnityEvent();
    public UnityEvent onSkipBack = new UnityEvent();
    public UnityEvent onStop = new UnityEvent();

    public bool enabled = true;

    public void GetInput()
    {
        if (enabled)
        {
            if (Input.GetKeyDown(input.pause))
                onPlayPause?.Invoke();
            if (Input.GetKeyDown(input.skipForward))
                onSkipForward?.Invoke();
            if (Input.GetKeyDown(input.skipBack))
                onSkipBack?.Invoke();
            if (Input.GetKeyDown(input.stop))
                onStop?.Invoke();

            var wheel = Input.mouseScrollDelta;
            if (wheel.y < 0)
                onSkipForward?.Invoke();
            else if (wheel.y > 0)
                onSkipBack?.Invoke();
        }
    }
}
