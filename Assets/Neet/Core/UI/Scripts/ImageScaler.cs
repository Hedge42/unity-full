using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This component maintains a constant real-size for an image
/// while scaling the border and rounded corners of an image
/// by re-scaling and adjusting the sizeDelta
/// </summary>
[ExecuteInEditMode]
public class ImageScaler : MonoBehaviour
{
    public const float MIN_SCALE = 0.001f;
    public const float MAX_SCALE = 1f;

    [HideInInspector]
    public float scale = 1f;

    private Vector2 size = Vector2.one * 50;
    private bool inheritSizeFromParent = true;
    private Vector2 LR = Vector2.zero;
    private Vector2 TB = Vector2.zero;
    private bool adjustTextSize;


    private void Awake()
    {
        // https://answers.unity.com/questions/1706551/disable-editing-of-components-in-inspector.html
        GetComponent<RectTransform>().hideFlags = HideFlags.NotEditable;
    }

    public void UpdateImage()
    {
        ChangeSize();

        // needs center justification with no scaling
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 midVector = Vector2.one * .5f;
        rt.anchorMin = midVector;
        rt.anchorMax = midVector;
        rt.pivot = midVector;

        rt.anchoredPosition = Vector3.zero;
    }

    private void ChangeSize()
    {
        if (inheritSizeFromParent)
        {
            size = transform.parent.GetComponent<RectTransform>().rect.size;
            size = new Vector2(size.x, size.y);
        }

        AdjustSizeDelta();
    }

    private void AdjustSizeDelta()
    {
        transform.localScale = new Vector2(scale, scale);
        GetComponent<RectTransform>().sizeDelta = new Vector2(size.x / scale, size.y / scale);
    }
    private void AdjustText()
    {
        GetComponentInChildren<TextMeshProUGUI>().transform.localScale = Vector3.one * (1 / scale);
        GetComponentInChildren<TextMeshProUGUI>().GetComponent<RectTransform>()
            .sizeDelta = new Vector2(size.x, size.y);

    }
    private void ImageHeightAndWidth(out int x, out int y)
    {
        Texture2D source = GetComponent<Image>().sprite.texture;
        x = source.width;
        y = source.height;
        print("(" + x + ", " + y + ")");
    }
}
