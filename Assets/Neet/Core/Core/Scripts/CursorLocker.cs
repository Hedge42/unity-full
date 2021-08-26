using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.UI;

public class CursorLocker : MonoBehaviour
{
    // this script controls mouse function
    // so there should only be one
    public static CursorLocker instance;

    public CursorLockMode mode;
    public bool active;

    public event Action onCursorLocked;
    public event Action onCursorUnlocked;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // DontDestroyOnLoad(this);

        // Menu.onPause.AddListener(delegate { SetLockMode(CursorLockMode.None); });
        // Menu.onResume.AddListener(delegate { SetLockMode(CursorLockMode.Locked); });
    }
    void Start()
    {
        SetLockMode(mode);
    }

    public void SetLockMode(CursorLockMode m)
    {
        mode = m;

        if (active)
        {
            Cursor.lockState = mode;

            // send events
            if (mode == CursorLockMode.Locked)
                onCursorLocked?.Invoke();
            else
                onCursorUnlocked?.Invoke();
        }
    }
}
