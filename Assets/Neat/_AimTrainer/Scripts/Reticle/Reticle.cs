using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    public bool loadProfileOnStart;
    public ReticleProfile profile;


    public RectTransform centerDot;
    public RectTransform top;
    public RectTransform bottom;
    public RectTransform left;
    public RectTransform right;

    // public RectTransform topOutline;
    // public RectTransform bottomOutline;
    // public RectTransform leftOutline;
    // public RectTransform rightOutline;

    [HideInInspector]
    public Image[] images;

    private Canvas c;

    private void Start()
    {
        if (loadProfileOnStart)
        {
            profile = ReticleProfile.Load();
            UpdateReticle();
        }
    }

    public void UpdateReticle()
    {
        // GetComponent<Canvas>().enabled = showInEditor;

        centerDot.sizeDelta = new Vector2(profile.dotSize, profile.dotSize);
        // centerDot.anchoredPosition = Vector3.zero;

        // update position of inner lines
        var widthOffset = profile.lineWidth % 2;
        int posOffset = (profile.lineWidth - 1) / 2;

        top.anchoredPosition = Vector2.up * profile.lineOffset;
        bottom.anchoredPosition = Vector2.down * (profile.lineOffset - 2 + widthOffset);
        left.anchoredPosition = Vector2.left * (profile.lineOffset - 2 + widthOffset);
        right.anchoredPosition = Vector2.right * profile.lineOffset;
        //right.anchoredPosition = Vector2.right * profile.lineOffset;

        top.anchoredPosition += Vector2.left * posOffset;
        bottom.anchoredPosition += Vector2.left * posOffset;
        left.anchoredPosition += Vector2.down * posOffset;
        right.anchoredPosition += Vector2.down * posOffset;

        int centerOffset = (profile.dotSize) / 2;
        centerDot.anchoredPosition = Vector2.zero;
        centerDot.anchoredPosition -= Vector2.one * centerOffset;

        // topOutline.anchoredPosition = top.anchoredPosition + Vector2.down * outlineSize;

        // set sizes
        Vector2 topBottom = new Vector2(profile.lineWidth, profile.lineLength);
        Vector2 leftRight = new Vector2(profile.lineLength, profile.lineWidth);
        top.sizeDelta = topBottom;
        bottom.sizeDelta = topBottom;
        left.sizeDelta = leftRight;
        right.sizeDelta = leftRight;

        // topOutline.sizeDelta = topBottom + Vector2.one * outlineSize * 2;

        images = new Image[5];
        images[0] = top.GetComponent<Image>();
        images[1] = bottom.GetComponent<Image>();
        images[2] = left.GetComponent<Image>();
        images[3] = right.GetComponent<Image>();
        images[4] = centerDot.GetComponent<Image>();

        // update colors
        images[0].color = profile.color;
        images[1].color = profile.color;
        images[2].color = profile.color;
        images[3].color = profile.color;
        images[4].color = profile.color;

        // adjust

    }

    public void UpdatePosition(int width, int height)
    {
        int centerX = width / 2;
        int centerY = height / 2;

        string x = "even";
        string y = "even";

        if (width % 2 == 1)
        {
            centerX += 1;
            x = "odd";
        }
        if (height % 2 == 1)
        {
            centerY += 1;
            y = "odd";
        }

        print(x + ", " + y);

        // all anchors bottom left

        Vector3 centerOffset = Vector3.one * (profile.dotSize % 2);

        centerDot.position = new Vector3(centerX, centerY);
        centerDot.position -= centerOffset;

        right.position = centerDot.position + Vector3.right * profile.lineOffset;
        left.position = centerDot.position + Vector3.left * profile.lineOffset;
        top.position = centerDot.position + Vector3.up * profile.lineOffset;
        bottom.position = centerDot.position + Vector3.down * profile.lineOffset;
    }

    private void Awake()
    {
        c = GetComponent<Canvas>();
        UpdateReticle();
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
}

