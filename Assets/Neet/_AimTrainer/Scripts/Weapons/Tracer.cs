using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracer : MonoBehaviour
{
    // on each update
    // spawn a semi-transparent orb in front of the face
    // gradually fade out the orb

    [Range(.01f, .4f)]
    public float speed;
    [Range(1, 10)]
    public int frames;
    private int currentFrame;

    private GameObject container;

    private void Start()
    {
        // no transform parent
        container = new GameObject("Tracer container");
    }

    private void Update()
    {
        currentFrame = (currentFrame + 1) % 100;
        if (currentFrame % frames == 0)
            StartCoroutine(SpawnTracer());
    }

    private IEnumerator SpawnTracer()
    {
        GameObject g = Instantiate(this.gameObject, container.transform);
        g.GetComponent<Tracer>().enabled = false;
        g.transform.position = transform.position;
        Renderer r = g.GetComponent<Renderer>();

        Color startColor = r.material.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        // http://thomasmountainborn.com/2016/05/25/materialpropertyblocks/
        MaterialPropertyBlock materialBlock = new MaterialPropertyBlock();
        r.GetPropertyBlock(materialBlock);

        float startTime = Time.time;
        while (Time.time < startTime + speed)
        {
            float ratio = (Time.time - startTime) / speed;
            Color c = Color.Lerp(startColor, endColor, ratio);
            materialBlock.SetColor("_Color", c);
            r.SetPropertyBlock(materialBlock);
            yield return null;
        }

        Destroy(g);
    }
}
