using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// created as a simpler replacement to Neet.UI.Menu.cs
public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;

    public bool hideOnResume;
    public bool showOnPause;

    private Canvas c;



    // events
    private static UnityEvent _onPause;
    public static UnityEvent onPause
    {
        get
        {
            if (_onPause == null)
                _onPause = new UnityEvent();
            return _onPause;
        }
    }
    private static UnityEvent _onResume;
    public static UnityEvent onResume
    {
        get
        {
            if (_onResume == null)
                _onResume = new UnityEvent();
            return _onResume;
        }
    }

    public static void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            onPause?.Invoke();
        }
    }
    public static void Resume()
    {
        if (isPaused)
        {
            isPaused = false;
            onResume?.Invoke();
        }
    }

    private void Awake()
    {
        c = GetComponent<Canvas>();

        if (hideOnResume)
            onResume.AddListener(Show);
        if (showOnPause)
            onPause.AddListener(Hide);
    }
    private IEnumerator Start()
    {
        yield return null;
        if (c.enabled && showOnPause)
            Pause();
    }

    public void Show()
    {
        c.enabled = false;
    }
    public void Hide()
    {
        c.enabled = true;
    }
}
