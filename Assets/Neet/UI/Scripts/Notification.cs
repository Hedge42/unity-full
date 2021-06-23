using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notification : MonoBehaviour
{
    TextMeshProUGUI tmp;

    public static Notification Create(Transform parent, string text = "")
    {
        GameObject g = Instantiate(new GameObject("notification"), parent);
        g.transform.SetParent(parent);

        Notification n = g.AddComponent<Notification>();
        g.transform.SetParent(parent);

        var yPosition = 50;
        g.transform.localPosition = new Vector3(0, yPosition, 0);

        // TODO text styling
        // tmp.textStyle = ...

        var tmp = g.AddComponent<TextMeshProUGUI>();
        n.tmp = tmp;
        tmp.fontSize = 1;
        var canvas = g.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        tmp.text = text;

        return n;
    }
    private static void Setup()
    {

    }
    public void SetPosition(Vector3 position)
    {
        transform.localPosition = position;
    }
    public void SetParent(Transform parent)
    {
        transform.parent = parent;
    }
    public void FadeOut()
    {
        StartCoroutine(_FadeOut());
    }
    private IEnumerator _FadeOut()
    {
        var startColor = tmp.color;
        var endColor = Color.clear;
        var duration = .5f;

        var startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            var t = (Time.time - startTime) / duration;
            tmp.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        if (gameObject != null)
            Destroy(gameObject);
    }
}
