using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorSelector : MonoBehaviour
{
    public TMP_InputField r;
    public TMP_InputField g;
    public TMP_InputField b;
    public Image colorPreview;

    private void Awake()
    {
        r.onEndEdit.AddListener(delegate { UpdateColor(); });
        g.onEndEdit.AddListener(delegate { UpdateColor(); });
        b.onEndEdit.AddListener(delegate { UpdateColor(); });
    }

    private void UpdateColor()
    {
        if (int.TryParse(r.text, out int ri))
        {

        }
        if (int.TryParse(g.text, out int gi))
        {

        }
        if (int.TryParse(b.text, out int bi))
        {

        }
    }
}
