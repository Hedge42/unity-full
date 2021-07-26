using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=thA3zv0IoUM&t=358s
public class PlotDebug : MonoBehaviour
{
    public AnimationCurve plot = new AnimationCurve();

    public void AddKey(float value)
    {
        plot.AddKey(Time.realtimeSinceStartup, value);
    }
}
