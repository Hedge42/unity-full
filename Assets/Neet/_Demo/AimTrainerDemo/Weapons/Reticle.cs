using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    public bool showInEditor;
    public float lineOffset;
    public Vector2 lineSize; // (width, length)
    public float dotSize;
    public Color dotColor;
    public Color lineColor;

    [HideInInspector]
    public Canvas c;
    public RectTransform centerDot;
    public RectTransform innerTop;
    public RectTransform innerBottom;
    public RectTransform innerLeft;
    public RectTransform innerRight;

    [HideInInspector]
    public Image[] images;

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        GetComponent<Canvas>().enabled = showInEditor;

        centerDot.sizeDelta = new Vector2(dotSize, dotSize);

        // update position of inner lines
        innerTop.anchoredPosition = Vector2.up * lineOffset;
        innerBottom.anchoredPosition = Vector2.down * lineOffset;
        innerLeft.anchoredPosition = Vector2.left * lineOffset;
        innerRight.anchoredPosition = Vector2.right * lineOffset;

        // set sizes
        innerTop.sizeDelta = lineSize;
        innerBottom.sizeDelta = lineSize;
        innerLeft.sizeDelta = lineSize;
        innerRight.sizeDelta = lineSize;

        // rotate left and right lines
        innerLeft.localEulerAngles = new Vector3(0, 0, 90);
        innerRight.localEulerAngles = new Vector3(0, 0, 90);

        images = new Image[5];
        images[0] = innerTop.GetComponent<Image>();
        images[1] = innerBottom.GetComponent<Image>();
        images[2] = innerLeft.GetComponent<Image>();
        images[3] = innerRight.GetComponent<Image>();
        images[4] = centerDot.GetComponent<Image>();

        // update colors
        images[0].color = lineColor;
        images[1].color = lineColor;
        images[2].color = lineColor;
        images[3].color = lineColor;
        images[4].color = dotColor;
    }

    private void Awake()
    {
        c = GetComponent<Canvas>();
        Init();
    }

    void SetActive(bool value)
    {
        if (value)
        {
            c.enabled = true;
        }
        else
        {
            c.enabled = false;
        }
    }

    private void Start()
    {
        if (!showInEditor)
            SetActive(true);

        // hide cursor when paused
        // Pause.instance.onPause += delegate { SetActive(false); };
        // Pause.instance.onResume += delegate { SetActive(true); };
    }
}

