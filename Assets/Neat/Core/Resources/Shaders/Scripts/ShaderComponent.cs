using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[ExecuteAlways]
public class ShaderComponent : UIBehaviour
{
    public float width
    {
        get
        {
            return img.material.GetFloat("_Width");
        }
        set
        {
            // var x = value;
            img.material.SetFloat("_Width", value);
            //mpb.SetFloat("_Width", value);

            //img.material.shader.

            //rnd.SetPropertyBlock(mpb);
        }
    }
    public float height;
    public float radius
    {
        get
        {
            return mpb.GetFloat("_Radius");
        }
        set
        {
            mpb.SetFloat("_Radius", value);
            rnd.SetPropertyBlock(mpb);
        }
    }

    public Color fillColor;

    public float borderWidth;
    public Color borderColor;

    public float fillShadowLength;
    public Color fillShadowColor;

    public float outerShadowLength;
    public Color outerShadowColor;

    public float borderShadowLength;
    public Color borderShadowColor;

    public Color overlayColor;

    private RectTransform _rt;
    public RectTransform rt
    {
        get
        {
            if (_rt == null)
                _rt = GetComponent<RectTransform>();
            return _rt;
        }
    }

    private Image _img;
    public Image img
    {
        get
        {
            if (_img == null)
                _img = GetComponent<Image>();
            return _img;
        }
    }

    private Material _mat;
    public Material mat
    {
        get
        {
            if (_mat == null)
                _mat = img.material;
            return _mat;
        }
        set
        {
            _mat = img.material;
        }
    }

    private Renderer _rnd;
    public Renderer rnd
    {
        get
        {
            //img.canvasRenderer.mater

            if (_rnd == null)
                _rnd = img.GetComponent<Renderer>();
            return _rnd;
        }
    }

    private MaterialPropertyBlock _mpb;
    public MaterialPropertyBlock mpb
    {
        get
        {
            if (_mpb == null)
            {
                _mpb = new MaterialPropertyBlock();
                rnd.GetPropertyBlock(_mpb);
            }
            return _mpb;
        }
    }

    protected override void OnRectTransformDimensionsChange()
    {
        Vector2 size = rt.rect.size;

        width = size.x;

        // mat.SetFloat("_Width", size.x);
        // mat.SetFloat("_Height", size.y);
    }
    public void ApplyInstance()
    {
    }
}
