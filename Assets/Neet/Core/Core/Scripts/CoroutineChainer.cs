using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineChainer
{
    List<IEnumerator> routines;

    private MonoBehaviour m;

    public CoroutineChainer(MonoBehaviour m)
    {
        this.m = m;
        routines = new List<IEnumerator>();
    }

    public void AddRoutine(IEnumerator i)
    {
        routines.Add(i);
    }

    public void Start()
    {
        m.StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        int count = 0;
        foreach (var i in routines)
        {
            Debug.Log("Started coroutine " + count++.ToString());
            var c = m.StartCoroutine(i);

            while (c != null)
                yield return null;
        }

        Debug.Log("Finished!");
    }
}
