using Neet.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    private const float delay = .1f;
    private const float fade = .1f;
    private const float maxWidth = 400;

    private CanvasGroup c;
    private EventHandler e;
    private RectTransform rt;
    private Coroutine routine;

    private bool isMouseOver;
    public bool IsMouseOver
    {
        get { return isMouseOver; }
        set
        {
            if (value && !isMouseOver)
                FadeIn();
            else if (!value && isMouseOver)
                FadeOut();

            isMouseOver = value;
        }
    }

    private void Awake()
    {
        c = GetComponent<CanvasGroup>();

        // add an event handler component to the parent
        if (GetComponentInParent<EventHandler>() == null)
            transform.parent.gameObject.AddComponent<EventHandler>();
        e = GetComponentInParent<EventHandler>();

        e.onPointerEnter += delegate { Toggle(true); };
        e.onPointerExit += delegate { Toggle(false); };

        e.onPointerClick += delegate
        {
            Neet.UI.ContextMenu.instance.Show("Hello");
        };
    }

    public void FixRect()
    {
        rt = GetComponent<RectTransform>();
        var tmp = GetComponentInChildren<TextMeshProUGUI>();
        tmp.autoSizeTextContainer = false;
        Vector2 lineSize = tmp.GetPreferredValues();

        rt.sizeDelta = new Vector2(lineSize.x, rt.sizeDelta.y);

        // figuring out how to scale y
        { 
        // float y = GetComponent<RectTransform>().sizeDelta.y - 6.5f;
        //float rectWidth = lineSize.x;
        //float rectHeight = lineSize.y;
        //while (rectWidth > maxWidth)
        //{
        //    float subtract = rectWidth - maxWidth;
        //    if (subtract > maxWidth)
        //    {
        //        subtract = maxWidth;
        //        rectWidth -= subtract;
        //        rectHeight += lineSize.y;
        //    }

        //}
        }
    }
    public void Toggle(bool value)
    {
        IsMouseOver = value;
    }

    // fade routines
    private Coroutine FadeIn()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(_FadeIn());
        return routine;
    }
    private IEnumerator _FadeIn()
    {
        // wait for delay seconds to start fade in
        var startTime = Time.time;
        while (Time.time < startTime + delay)
            yield return null;

        // start fade in
        startTime = Time.time;
        var startAlpha = c.alpha;
        while (Time.time < startTime + fade)
        {
            var t = (Time.time - startTime) / fade;
            var alpha = Mathf.Lerp(startAlpha, 1, t);
            c.alpha = alpha;
            yield return null;
        }

        routine = null;
    }
    private Coroutine FadeOut()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(_FadeOut());
        return routine;
    }
    private IEnumerator _FadeOut()
    {
        // wait for delay seconds to start fade in
        var startTime = Time.time;
        while (Time.time < startTime + delay)
            yield return null;

        // start fade in
        startTime = Time.time;
        var startAlpha = c.alpha;
        while (Time.time < startTime + fade)
        {
            var t = (Time.time - startTime) / fade;
            var alpha = Mathf.Lerp(startAlpha, 0, t);
            c.alpha = alpha;
            yield return null;
        }

        c.alpha = 0;
        routine = null;
    }
}
