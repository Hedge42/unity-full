using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteAlways]
public class PixelSizeAdjuster : MonoBehaviour
{
    [HideInInspector]
    public bool controlWidth;
    [HideInInspector]
    public int width;

    [HideInInspector]
    public bool controlHeight;
    [HideInInspector]
    public int height;

    [SerializeField]
    private Canvas _canvas;
    public Canvas canvas
    {
        get
        {
            // already assigned
            if (_canvas != null)
                return _canvas;

            // find eldest canvas
            Transform parent = transform.parent;
            _canvas = GetComponent<Canvas>();
            while (parent != null)
            {
                // update canvas
                var _olderCanvas = parent.GetComponent<Canvas>();
                if (_olderCanvas != null)
                    _canvas = _olderCanvas;

                parent = parent.parent;
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

    private void OnEnable()
    {
        UpdateSize();
    }

    private void OnDestroy()
    {
        handler.onChange -= UpdateSize;
    }

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
