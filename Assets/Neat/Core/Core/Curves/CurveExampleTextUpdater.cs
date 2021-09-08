using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurveExampleTextUpdater : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TranslateOverTime translation;

    private void OnValidate()
    {
        text.text = gameObject.name = transform.parent.name = translation.translations[0].curve.name;
    }
}
