using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteAlways]
public class PixelSizeAdjuster : MonoBehaviour
{
    public bool controlWidth;
    public int width;

    public bool controlHeight;
    public int height;

    private Canvas _canvas;
    public Canvas canvas
    {
        get
        {
            if (_canvas != null)
                return _canvas;

            // traverse parents
            Transform parent = transform.parent;

            for (; ; )
            {
                // not found
                if (parent == null)
                {
                    Debug.LogError("No canvas found");
                    break;
                }


                // try to get canvas in parent
                var parentCanvas = parent.GetComponent<Canvas>();

                // fail - increment parent
                if (parentCanvas == null)
                {
                    parent = parent.parent;
                    continue;
                }

                // success - set and return
                else
                {
                    _canvas = parentCanvas;
                    break;
                }
            }

            return _canvas;
        }
    }

    private RectChangedHandler _handler;
    public RectChangedHandler handler
    {
        get
        {
            var _canvas = canvas;

            if (_canvas != null)
            {
                _handler = _canvas.GetComponent<RectChangedHandler>();

                if (_handler == null)
                    _handler = _canvas.gameObject.AddComponent<RectChangedHandler>();
            }

            return _handler;
        }
    }

    private RectTransform _rect;
    public RectTransform rect
    {
        get
        {
            if (_rect == null)
                _rect = GetComponent<RectTransform>();
            return _rect;
        }
    }

    private bool isSubscribed;

    private void Initialize()
    {
        if (!isSubscribed && handler != null)
        {
            handler.onChange += UpdateSize;
            isSubscribed = true;
        }
    }

    public void UpdateSize()
    {
        Initialize();

        var size = rect.sizeDelta;

        if (controlWidth)
            size.x = (float)width / canvas.transform.localScale.x;

        if (controlHeight)
            size.y = (float)height / canvas.transform.localScale.y;

        rect.sizeDelta = size;
    }
}
