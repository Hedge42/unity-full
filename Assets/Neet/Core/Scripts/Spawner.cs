using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public GameObject prefab;

    [Range(1, 100)]
    public int cap; // how many of the prefab should be added to the pool upon loading?
    public bool isHardCapped; // if another object is needed but all are being used, should it take from the active objects [true], or create a new one [false]?


    private Queue<GameObject> inactiveQ = new Queue<GameObject>();
    private Queue<GameObject> activeQ = new Queue<GameObject>();

    public GameObject GetNext()
    {
        // set active? as param?
        // if this is called outside of this class, and SetActive() is NOT used
        // it never enters the queue cycle...

        GameObject g;
        if (inactiveQ.Count > 0)
        {
            // ideal
            g = inactiveQ.Dequeue();
            SetActive(g);
        }
        else
        {
            if (isHardCapped)
            {
                // take the oldest active object to the back of the line
                g = activeQ.Dequeue();
                activeQ.Enqueue(g);
            }
            else
            {
                // create new
                g = Instantiate(prefab, transform);
                SetActive(g);
            }
        }

        return g;
    }

    protected void SetInactive(GameObject g)
    {
        // destroy it instead if the total count is more than the cap
        if (activeQ.Count + inactiveQ.Count > cap)
            Destroy(g);

        g.SetActive(false);
        inactiveQ.Enqueue(g);
    }
    protected void SetActive(GameObject g)
    {
        g.SetActive(true);
        activeQ.Enqueue(g);
    }
    protected void SetActive(int count, bool resetActive)
    {
        if (resetActive && activeQ.Count > 0)
        {
            foreach (GameObject g in activeQ)
            {
                SetInactive(g);
            }
        }

        for (int i = 0; i < count; i++)
        {
            SetActive(GetNext());
        }

    }
    protected void SpawnInactive(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject g = Instantiate(prefab, transform);
            SetInactive(g);
        }
    }
    protected void DestroyAll()
    {
        foreach (Transform child in transform)
        {
            // what does this do to the queues?
            Destroy(child);

            Debug.Log(activeQ.Count);
            Debug.Log(inactiveQ.Count);
        }
    }
    protected void SetAllInactive()
    {
        foreach (GameObject g in activeQ)
            SetInactive(g);
    }
    protected List<GameObject> GetAll()
    {
        List<GameObject> objects = new List<GameObject>();

        foreach (GameObject g in activeQ)
            objects.Add(g);

        foreach (GameObject g in inactiveQ)
            objects.Add(g);

        return objects;
    }
}
