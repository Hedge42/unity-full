using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Fret
{
    // toggled by clicking
    public enum FretToggleMode
    {
        Normal,
        Hidden,
        Emphasized
    }
    public enum BorderMode
    {
        Dots,
        Numbers
    }
    public enum PlayableMode
    {
        Dot,
        Note,
        Interval,
        NoteAndInterval
    }

    public FretUI mono;
    public RectTransform rect;
    public FretToggleMode displayMode;
    public FretboardUI fretboardUI;
    public int fretNum;
    public int rowIndex;

    public abstract void UpdateDisplay();

    public virtual void Hide()
    {
        displayMode = FretToggleMode.Hidden;
        UpdateDisplay();
    }
    public virtual void ToggleMode()
    {
        if (displayMode == FretToggleMode.Normal)
            displayMode = FretToggleMode.Emphasized;
        else
            displayMode = FretToggleMode.Normal;
        UpdateDisplay();
    }
}
