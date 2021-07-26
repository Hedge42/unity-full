using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neet.Extensions;

public class PanelExpander : MonoBehaviour
{
    public void UpdateSize(VerticalLayoutGroup v)
    {
        float height = 0;
        foreach (var child in v.transform.GetChildren())
        {
            height += child.GetComponent<RectTransform>().sizeDelta.y;
            height += v.spacing;
        }
        // var topOffset = transform.parent.GetComponent<RectTransform>().offsetMax.y;
        var rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height + v.padding.top + v.padding.bottom + 20);
    }
}
