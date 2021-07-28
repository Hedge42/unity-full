using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ImageFlasher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Range(0, 1)]
    public float bottomAlpha;
    [Range(.2f, 1f)]
    public float rate;
    public bool detectMouseHover = true;

    private Image image;
    private Color startColor;
    private Coroutine currentProcess;

    private void Awake()
    {
        image = GetComponent<Image>();
        startColor = image.color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (detectMouseHover)
            StartFlashing();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (detectMouseHover)
            StopFlashing();
    }
    public void StartFlashing()
    {
        currentProcess = StartCoroutine(FadeOut());
    }
    public void StopFlashing()
    {
        if (currentProcess != null)
            StopCoroutine(currentProcess);

        image.color = startColor;
    }
    private IEnumerator FadeOut()
    {
        float startTime = Time.unscaledTime;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, bottomAlpha);

        while (Time.unscaledTime < startTime + rate)
        {
            float ratio = (Time.unscaledTime - startTime) / rate;
            image.color = Color.Lerp(startColor, endColor, ratio);
            yield return null;
        }

        currentProcess = StartCoroutine(FadeIn());
    }
    private IEnumerator FadeIn()
    {
        float startTime = Time.unscaledTime;
        Color fadedColor = image.color;

        while (Time.unscaledTime < startTime + rate)
        {
            float ratio = (Time.unscaledTime - startTime) / rate;
            image.color = Color.Lerp(fadedColor, startColor, ratio);
            yield return null;
        }

        currentProcess = StartCoroutine(FadeOut());
    }
}
