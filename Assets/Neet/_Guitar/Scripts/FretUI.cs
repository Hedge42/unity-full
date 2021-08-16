﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// a monobehavior layer for fret/border objects
// serves as a template + container for fret and border objects
public class FretUI : MonoBehaviour, IPointerClickHandler
{
    public Fret fret;

    public TextMeshProUGUI tmp;

    public Image borderDot1;
    public Image borderDot2;

    public Fret.FretToggleMode displayMode
    {
        get { return fret.displayMode; }
        set { fret.displayMode = value; }
    }
    public FretboardUI tab
    {
        get { return fret.fretboardUI; }
        set { fret.fretboardUI = value; }
    }
    public int fretNum
    {
        get { return fret.fretNum; }
        set { fret.fretNum = value; }
    }

    public void UpdateText()
    {
        fret.UpdateDisplay();
    }
    public void Hide()
    {
        fret.Hide();
    }
    public void ToggleMode()
    {
        fret.ToggleMode();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            fret.Hide();
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            fret.ToggleMode();
        }
    }
}
