using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.GameManager;

[System.Serializable]
public class ColorProfile
{
    // possible range of values
    public const int RGB_MIN = 0;
    public const int RGB_MAX = 255;

    // data
    public int backgroundR = 0;
    public int backgroundG = 0;
    public int backgroundB = 0;
    public int targetR = 255;
    public int targetG = 80;
    public int targetB = 80;
    public int targetA = 255;
    public int dummyR = 80;
    public int dummyG = 80;
    public int dummyB = 255;
    public int dummyA = 255;
    public int centerR = 255;
    public int centerG = 255;
    public int centerB = 255;
    public int centerA = 255;
    public int trackingR = 255;
    public int trackingG = 255;
    public int trackingB = 255;

    public Color backgroundColor
    {
        get
        {
            return new Color(backgroundR / 255f, backgroundG / 255f, backgroundB / 255f);
        }
    }
    public Color targetColor
    {
        get
        {
            return new Color(targetR / 255f, targetG / 255f, targetB / 255f, targetA / 255f);
        }
    }
    public Color dummyColor
    {
        get
        {
            return new Color(dummyR / 255f, dummyG / 255f, dummyB / 255f, dummyA / 255f);
        }
    }
    public Color centerColor
    {
        get
        {
            return new Color(centerR / 255f, centerG / 255f, centerB / 255f, centerA / 255f);
        }
    }
    public Color trackingColor
    {
        get
        {
            return new Color(trackingR / 255f, trackingG / 255f, trackingB / 255f);
        }
    }
}