using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppCloser
{
    // FIXME This does not belong here
    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE_WIN
        Application.Quit();
#endif
    }
}


