using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This class exists to make a 2D object's collider automatically
/// resize to match its RectTransform
/// </summary>
[ExecuteAlways]
public class ColliderSizeAdjuster : UIBehaviour
{
    public bool validate;

    // check this for free positioning
    public bool ignorePivot;

    protected override void OnRectTransformDimensionsChange()
    {
        FixSize();
    }
    private void FixSize()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 size = rt.rect.size;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        collider.size = size;

        // adjust for anchors
        // find the function...
        // pivot.x == 0? offset.x = rect.x / 2
        // pivot.x == .5? offset.x = 0
        // pivot.x == 1? offset.x = (rect.x / 2) * -1

        if (!ignorePivot)
        {
            Vector2 pivot = GetComponent<RectTransform>().pivot;
            collider.offset = new Vector2(size.x / 2 - pivot.x * size.x, size.y / 2 - pivot.y * size.y);
        }
    }
}
