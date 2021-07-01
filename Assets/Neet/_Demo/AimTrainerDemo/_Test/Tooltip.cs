using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject toolip;

    public float delay;
    public float fade;

    private CanvasGroup c;

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
        c = toolip.GetComponent<CanvasGroup>();

    }
    private void OnMouseOver()
    {
        // IsMouseOver = true;
    }
    private void OnMouseExit()
    {
        // FadeOut();
        // IsMouseOver = false;
    }

    public void Toggle(bool value)
    {
        IsMouseOver = value;
    }

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

        routine = null;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsMouseOver = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsMouseOver = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }
}
