using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Reticle))]
public class ReticleTracer : MonoBehaviour
{
    [Range(.01f, .4f)]
    public float speed;
    [Range(1, 10)]
    public int frames;
    private int currentFrame;

    public Motor motor;

    private Transform container;
    private Reticle ret;

    private Color colorA;
    private Color colorB;

    private void Start()
    {
        // spawn and make sibling
        container = new GameObject("Tracer container").transform;
        container.parent = transform.parent;

        ret = GetComponent<Reticle>();
        colorA = ret.profile.color;
        colorB = new Color(colorA.r, colorA.g, colorA.b, 0);
    }

    private void Update()
    {
        // spawn tracer every x frames
        currentFrame = (currentFrame + 1) % 100;
        if (currentFrame % frames == 0)
            StartCoroutine(SpawnTracer());
    }

    private IEnumerator SpawnTracer()
    {
        GameObject tracer = Instantiate(this.gameObject);
        Image[] ims = tracer.GetComponent<Reticle>().images;
        tracer.GetComponent<ReticleTracer>().enabled = false;
        tracer.GetComponent<Reticle>().enabled = false;
        tracer.GetComponent<Canvas>().sortingOrder = 0;
        tracer.transform.position = transform.position;

        float startTime = Time.time;
        while (Time.time < startTime + speed)
        {
            float ratio = (Time.time - startTime) / speed;
            foreach (Image i in ims)
                i.color = Color.Lerp(colorA, colorB, ratio);

            // adjust position based on head (parent)
            //tracer.transform.position += anchor.GetComponent<Rigidbody>().velocity * Time.deltaTime;
            tracer.transform.position += motor.deltaPosition;

            tracer.transform.LookAt(transform.parent);
            yield return null;
        }

        Destroy(tracer);
    }
}
           