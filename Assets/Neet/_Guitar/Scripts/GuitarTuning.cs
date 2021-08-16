using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// made a class to serialize tunings
public class GuitarTuning
{
    public string name;

    public int numStrings { get { return stringValues.Length; } }

    public int[] stringValues;

    public GuitarTuning()
    {
        this.stringValues = new int[] { 4, 9, 2, 7, 0, 5 };
        this.name = "Standard";
    }
    
    private static GuitarTuning Standard()
    {
        return new GuitarTuning();
    }
}
