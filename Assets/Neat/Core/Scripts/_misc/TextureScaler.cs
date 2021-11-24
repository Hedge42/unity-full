using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScaler : MonoBehaviour
{
    public enum ScaleAxis
    {
        XY,
        XZ,
        YZ
    }

    public float scale;
    public ScaleAxis axis;
    private void OnValidate()
    {
        Init();

    }
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        var locScale = transform.localScale;

        Vector2 v;
        var x = locScale.x / 2 * scale;
        var y = locScale.y / 2 * scale;
        var z = locScale.z / 2 * scale;
        if (axis == ScaleAxis.XY)
            v = new Vector2(x, y);
        else if (axis == ScaleAxis.XZ)
            v = new Vector2(x, z);
        else
            v = new Vector2(y, z);

        // gameObject.GetMaterial().mainTextureScale = v;
        GetComponent<Renderer>().sharedMaterial.mainTextureScale = v;
    }
}
