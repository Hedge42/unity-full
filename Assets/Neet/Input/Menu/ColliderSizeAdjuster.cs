using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This class exists to make a 2D object's collider automatically
/// resize to match its RectTransform
/// </summary>
public class ColliderSizeAdjuster : MonoBehaviour
{
    public bool validate;

    // check this for free positioning
    public bool ignorePivot;
    private void OnValidate()
    {
        FixSize();
    }
    private void Start()
    {
        // delayed start method because collider would sometimes use the wrong size
        // when controlled by other elements during start like a layout group
        StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForEndOfFrame();
        FixSize();
    }

    private void FixSize()
    {
        Vector2 rect = GetComponent<RectTransform>().rect.size;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        collider.size = rect;

        // adjust for anchors
        // find the function...
        // pivot.x == 0? offset.x = rect.x / 2
        // pivot.x == .5? offset.x = 0
        // pivot.x == 1? offset.x = (rect.x / 2) * -1

        if (!ignorePivot)
        {
            Vector2 pivot = GetComponent<RectTransform>().pivot;
            collider.offset = new Vector2(rect.x / 2 - pivot.x * rect.x, rect.y / 2 - pivot.y * rect.y);
        }
    }
}
