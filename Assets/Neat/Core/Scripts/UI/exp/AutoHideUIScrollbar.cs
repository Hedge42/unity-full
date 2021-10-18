// Attach this script to the ScrollRect
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// https://forum.unity.com/threads/hide-scrollbar.285929/
[RequireComponent(typeof(ScrollRect))]
public class AutoHideUIScrollbar : MonoBehaviour
{

    public bool alsoDisableScrolling;

    private float disableRange = 0.99f;
    private ScrollRect scrollRect;
    private ScrollbarClass scrollbarVertical = null;
    private ScrollbarClass scrollbarHorizontal = null;

    private class ScrollbarClass
    {
        public Scrollbar bar;
        public bool active;
    }


    void Start()
    {
        scrollRect = gameObject.GetComponent<ScrollRect>();
        if (scrollRect.verticalScrollbar != null)
            scrollbarVertical = new ScrollbarClass() { bar = scrollRect.verticalScrollbar, active = true };
        if (scrollRect.horizontalScrollbar != null)
            scrollbarHorizontal = new ScrollbarClass() { bar = scrollRect.horizontalScrollbar, active = true };

        if (scrollbarVertical == null && scrollbarHorizontal == null)
            Debug.LogWarning("Must have a horizontal or vertical scrollbar attached to the Scroll Rect for AutoHideUIScrollbar to work");
    }

    void Update()
    {
        if (scrollbarVertical != null)
            SetScrollBar(scrollbarVertical, true);
        if (scrollbarHorizontal != null)
            SetScrollBar(scrollbarHorizontal, false);
    }

    void SetScrollBar(ScrollbarClass sbc, bool isVertical)
    {
        if (sbc.active && sbc.bar.size > disableRange)
            SetBar(sbc, false, isVertical);
        else if (!sbc.active && sbc.bar.size < disableRange)
            SetBar(sbc, true, isVertical);
    }

    void SetBar(ScrollbarClass scrollbar, bool isActive, bool isVertical)
    {
        scrollbar.bar.gameObject.SetActive(isActive);
        scrollbar.active = isActive;
        if (alsoDisableScrolling)
        {
            if (isVertical)
            {
                scrollRect.vertical = isActive;
            }
            else
            {
                scrollRect.horizontal = isActive;
            }
        }
    }
}
