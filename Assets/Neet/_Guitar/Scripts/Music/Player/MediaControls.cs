using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// should make reusing media-type controls possible
// this could just be a regular class...

public interface IMediaController
{
    void PauseToggle();
    void Skip(float deltaTime);
    void SkipTo(float time);
    void Scroll(float delta);
    void Reset();
}

public class MediaInput
{
    [System.Serializable]
    private class MediaInputSetting
    {
        public KeyCode skipForward = KeyCode.RightArrow;
        public KeyCode skipBack = KeyCode.LeftArrow;
        public KeyCode pause = KeyCode.Space;
        public KeyCode toBeginning = KeyCode.Home;
        public KeyCode toEnd = KeyCode.End;
    }
    private MediaInputSetting input = new MediaInputSetting();


    public UnityEvent onPause = new UnityEvent();
    public UnityEvent<float> onSkip = new UnityEvent<float>();
    public UnityEvent<float> onSkipTo = new UnityEvent<float>();
    public UnityEvent<float> onScroll = new UnityEvent<float>();

    public bool enabled = true;
    private IMediaController controller;

    public MediaInput(IMediaController controller)
    {
        this.controller = controller;
    }

    public MediaInput() { }

    public void GetControllerInput()
    {
        if (controller != null && enabled)
        {
            if (Input.GetKeyDown(input.skipForward))
                controller.Skip(5f);
            if (Input.GetKeyDown(input.skipBack))
                controller.Skip(-5f);
            if (Input.GetKeyDown(input.pause))
                controller.PauseToggle();
            if (Input.GetKeyDown(input.toBeginning))
                controller.Reset();

            var wheel = Input.mouseScrollDelta;
            if (Mathf.Abs(wheel.y) > 0f)
                onScroll?.Invoke(-wheel.y);
        }
    }
    public void GetInput()
    {
        if (enabled)
        {
            if (Input.GetKeyDown(input.skipForward))
                onSkip?.Invoke(5f);
            if (Input.GetKeyDown(input.skipBack))
                onSkip?.Invoke(-5f);
            if (Input.GetKeyDown(input.pause))
                onPause?.Invoke();
            if (Input.GetKeyDown(input.toBeginning))
                onSkipTo?.Invoke(0f);

            var wheel = Input.mouseScrollDelta;
            if (Mathf.Abs(wheel.y) > 0f)
                onScroll?.Invoke(-wheel.y);
        }
    }
}

public class MediaControls : MonoBehaviour
{
    public KeyCode skipForward = KeyCode.RightArrow;
    public KeyCode skipBack = KeyCode.LeftArrow;
    public KeyCode pause = KeyCode.Space;
    public KeyCode toBeginning = KeyCode.Home;
    public KeyCode toEnd = KeyCode.End;

    private UnityAction onPause;
    private UnityAction<float> onSkip;
    private UnityAction<float> onSkipTo;

    public void SetTarget(UnityAction _pause, UnityAction<float> _skip, UnityAction<float> _skipTo)
    {
        this.onPause = _pause;
        this.onSkip = _skip;
        this.onSkipTo = _skipTo;
    }

    private void GetInput()
    {
        if (enabled)
        {
            if (Input.GetKeyDown(skipForward))
                onSkip(5f);
            if (Input.GetKeyDown(skipBack))
                onSkip(-5f);
            if (Input.GetKeyDown(pause))
                onPause();
            if (Input.GetKeyDown(toBeginning))
                onSkipTo(0f);
        }
    }

    private void Update()
    {
        if (enabled)
        {
            if (Input.GetKeyDown(skipForward))
                onSkip.Invoke(5f);
            if (Input.GetKeyDown(skipBack))
                onSkip?.Invoke(-5f);
            if (Input.GetKeyDown(pause))
                onPause?.Invoke();
            if (Input.GetKeyDown(toBeginning))
                onSkipTo?.Invoke(0f);
        }
    }
}
