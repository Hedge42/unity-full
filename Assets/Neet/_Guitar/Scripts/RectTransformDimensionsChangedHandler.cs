using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[ExecuteAlways]
public class RectTransformDimensionsChangedHandler : UIBehaviour
{
    public event Action onChange;

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        onChange?.Invoke();
    }
}
