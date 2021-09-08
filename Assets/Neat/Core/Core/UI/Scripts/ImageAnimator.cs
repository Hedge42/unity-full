using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// https://answers.unity.com/questions/1199251/onmouseover-ui-button-c.html
public class ImageAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // add this component to a Button

    [Range(0, .2f)]
    public float duration;
    [Range(0, .3f)]
    public float hoverDeltaScale;
    public Color hoverColor;
    [Range(-.1f, .1f)]
    public float pressedDeltaScale;
    public Color pressedColor;


    private bool isMouseOver = false;
    private bool isMousePressed = false;
    private Vector3 startScale;
    private Color startColor;
    // private ProceduralImage image;
    private Image image;

    private void Awake()
    {
        startScale = transform.localScale;
        //image = GetComponent<ProceduralImage>();
        image = GetComponent<Image>();
        startColor = image.color;
    }

    IEnumerator PointerEnter()
    {
        Vector3 scaleAtAnimationStart = transform.localScale;
        Vector3 endScale = startScale + Vector3.one * hoverDeltaScale;

        // if already halfway to endScale, only spend half of the duration getting there
        // float fixedDuration = ((deltaScale - (endScale.x - scaleAtAnimationStart.x)) / deltaScale) * duration;

        float startTime = Time.unscaledTime;
        while (Time.unscaledTime < startTime + duration && isMouseOver)
        {
            float ratio = (Time.unscaledTime - startTime) / duration;
            transform.localScale = Vector3.Lerp(scaleAtAnimationStart, endScale, ratio);
            image.color = Color.Lerp(startColor, hoverColor, ratio);
            //yield return new WaitForEndOfFrame();
            yield return null;
        }

        transform.localScale = endScale;
        image.color = hoverColor;
    }
    IEnumerator PointerExit()
    {
        Vector3 scaleAtAnimationStart = transform.localScale;
        Vector3 endScale = startScale;

        // if already halfway to endScale, only spend half of the duration getting there
        //float fixedDuration = (deltaScale - (scaleAtAnimationStart.x - endScale.x)) * duration;

        float startTime = Time.unscaledTime;
        while (Time.unscaledTime < startTime + duration && !isMouseOver)
        {
            float ratio = (Time.unscaledTime - startTime) / duration;
            transform.localScale = Vector3.Lerp(scaleAtAnimationStart, endScale, ratio);
            image.color = Color.Lerp(hoverColor, startColor, ratio);
            yield return null;
        }

        transform.localScale = endScale;
        image.color = startColor;
    }
    IEnumerator PointerDown()
    {
        Vector3 scaleAtAnimationStart = transform.localScale;
        Vector3 endScale = startScale + Vector3.one * pressedDeltaScale + Vector3.one * hoverDeltaScale;

        float startTime = Time.unscaledTime;
        while (Time.unscaledTime < startTime + duration && isMouseOver && isMousePressed)
        {
            float ratio = (Time.unscaledTime - startTime) / duration;
            transform.localScale = Vector3.Lerp(scaleAtAnimationStart, endScale, ratio);
            image.color = Color.Lerp(hoverColor, pressedColor, ratio);
            yield return null;
        }

        image.color = pressedColor;
        transform.localScale = endScale;
    }
    IEnumerator PointerUp()
    {
        if (!isMouseOver)
            yield break;

        Vector3 scaleAtAnimationStart = transform.localScale;
        Vector3 endScale = startScale + Vector3.one * hoverDeltaScale;

        float startTime = Time.unscaledTime;
        while (Time.unscaledTime < startTime + duration && !isMousePressed)
        {
            float ratio = (Time.unscaledTime - startTime) / duration;
            transform.localScale = Vector3.Lerp(scaleAtAnimationStart, endScale, ratio);
            image.color = Color.Lerp(pressedColor, hoverColor, ratio);
            yield return null;
        }

        transform.localScale = endScale;
        image.color = hoverColor;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        StartCoroutine(PointerEnter());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        StartCoroutine(PointerUp());
        StartCoroutine(PointerExit());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isMousePressed = true;
        StartCoroutine(PointerDown());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMousePressed = false;
        StartCoroutine(PointerUp());
    }
}
