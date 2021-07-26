using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOverTime : MonoBehaviour
{
    [System.Serializable]
    public class Colorization
    {
        public Color color = Color.gray;
        [Range(.1f, 2f)] public float time = .5f;
    }

    public bool playOnStart;
    public bool repeat;
    public Colorization[] colorizations;

    private Coroutine currentCycle;

    private void Awake()
    {
        // cache Color (part of the material? renderer?
    }
    private void Start()
    {
        if (playOnStart)
        {
            StartCycling();
        }
    }
    private void OnValidate()
    {
        if (colorizations != null && colorizations.Length > 0)
        {
            // color the material to the 1st color in the series
            GetComponent<Renderer>().material.color = colorizations[0].color;
        }
    }

    public void StartCycling()
    {
        if (currentCycle != null)
            StopCoroutine(currentCycle);

        currentCycle = StartCoroutine(Cycle());
    }

    private IEnumerator Cycle()
    {
        for (int i = 0; i < colorizations.Length; i++)
        {
            float startTime = Time.time;
            while (Time.time < startTime + colorizations[i].time)
            {
                float timePassed = Time.time - startTime;
                float ratio = timePassed / colorizations[i].time;

                // target is i + 1, unless that's out of bounds
                // avoid array index exception: go back to index 0
                Color targetColor =  i + 1 >= colorizations.Length ? colorizations[0].color : colorizations[i + 1].color;
                Color startColor = colorizations[i].color;

                Color target = Color.Lerp(startColor, targetColor, ratio);
                GetComponent<Renderer>().material.color = target;
                yield return null;
            }
        }

        if (repeat)
            StartCycling();
    }
}
