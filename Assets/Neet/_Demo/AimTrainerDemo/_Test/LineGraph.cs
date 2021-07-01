using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LineGraph : MonoBehaviour
{
    public TextMeshProUGUI title;

    public Image dotSprite;
    public Image lineSprite;

    public float dotSize = 20f;
    public float lineSize = 3f;
    public int maxTicks = 15;
    public int numRandomValues;

    public RectTransform pointContainer;
    public RectTransform lineContainer;

    public RectTransform yTickContainer;
    public Image yTick;
    public RectTransform xTickContainer;
    public Image xTick;

    private List<RectTransform> points;
    private List<RectTransform> lines;
    private RectTransform rt;

    private string ySuffix;

    private void Awake()
    {
        points = new List<RectTransform>();
        lines = new List<RectTransform>();
    }

    public void GenerateRandom()
    {
        List<float> values = new List<float>();

        for (int i = 0; i < numRandomValues; i++)
            values.Add(Random.Range(0, 100));

        Generate(values, 0, 100);
    }

    public void Generate(List<float> values, float min, float max)
    {
        Clear();
        rt = GetComponent<RectTransform>();

        // 2 points - step is full width
        var step = rt.rect.width / (values.Count - 1);

        for (int i = 0; i < values.Count; i++)
        {
            var x = step * i;
            var y = GetY(values[i], min, max);
            CreatePoint(x, y);
            CreateLine();
        }

        int xTicks = values.Count;
        while (xTicks > maxTicks)
            xTicks /= 2;
        DrawXAxis(xTicks);

        int yTicks = 5;
        DrawYAxis(yTicks, min, max);
    }
    public void SetTitle(string title)
    {
        this.title.text = title;
    }
    public void SetYSuffix(string suffix)
    {
        ySuffix = suffix;
    }

    private void CreatePoint(float x, float y)
    {
        GameObject point = Instantiate(dotSprite.gameObject, pointContainer.transform);
        point.SetActive(true);
        var pointRect = point.GetComponent<RectTransform>();
        pointRect.anchoredPosition = new Vector2(x, y);
        pointRect.sizeDelta = new Vector2(dotSize, dotSize);
        points.Add(pointRect);
    }

    private void CreateLine()
    {
        // create a line between the last two points
        if (points.Count > 1)
        {
            var a = points[points.Count - 2];
            var b = points[points.Count - 1];

            var aPos = a.anchoredPosition;
            var bPos = b.anchoredPosition;

            var midpoint = (aPos + bPos) / 2;
            var distance = Vector2.Distance(aPos, bPos);

            GameObject line = Instantiate(lineSprite.gameObject, lineContainer.transform);
            line.SetActive(true);
            var lineRect = line.GetComponent<RectTransform>();
            lineRect.anchoredPosition = midpoint;
            lineRect.sizeDelta = new Vector2(distance, lineSize);

            var deltaPos = bPos - aPos;
            var angle = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
            lineRect.localEulerAngles = new Vector3(0, 0, angle);

            lines.Add(lineRect);
        }
    }

    private void DrawYAxis(int numTicks, float min, float max)
    {
        if (numTicks < 2)
            numTicks = 2;

        var step = rt.rect.height / (numTicks - 1);
        for (int i = 0; i < numTicks; i++)
        {
            var tick = Instantiate(yTick.gameObject, yTickContainer);
            tick.SetActive(true);
            var tickRT = tick.GetComponent<RectTransform>();

            var y = step * i;
            tickRT.anchoredPosition = new Vector2(tickRT.anchoredPosition.x, y);

            var t = y / rt.rect.height;
            var value = Mathf.Lerp(min, max, t);

            var tickText = tick.GetComponentInChildren<TextMeshProUGUI>();
            tickText.text = value.ToString("f0");
            tickText.text += ySuffix;
        }

    }
    private void DrawXAxis(int numTicks)
    {
        if (numTicks < 2)
            numTicks = 2;

        var step = rt.rect.width / (numTicks - 1);
        for (int i = 0; i < numTicks; i++)
        {
            var tick = Instantiate(xTick.gameObject, xTickContainer);
            tick.SetActive(true);
            var tickRT = tick.GetComponent<RectTransform>();

            var x = step * i;
            tickRT.anchoredPosition = new Vector2(x, tickRT.anchoredPosition.y);

            tick.GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
        }
    }

    private float GetY(float value, float min, float max)
    {
        var height = rt.rect.height;
        var ratio = (value - min) / (max - min);
        return height * ratio;
    }

    private void Clear()
    {
        foreach (var line in lineContainer.GetChildren())
            DestroyImmediate(line.gameObject);
        foreach (var point in pointContainer.GetChildren())
            DestroyImmediate(point.gameObject);
        foreach (var tick in xTickContainer.GetChildren())
            DestroyImmediate(tick.gameObject);
        foreach (var tick in yTickContainer.GetChildren())
            DestroyImmediate(tick.gameObject);


        points = new List<RectTransform>();
        lines = new List<RectTransform>();
    }
}
