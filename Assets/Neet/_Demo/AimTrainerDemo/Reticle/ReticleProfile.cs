using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.File;

[System.Serializable]
public class ReticleProfile
{
    public const string filename = "reticle.sav";

    public const int OFFSET_MAX = 30;
    public const int WIDTH_MAX = 8;
    public const int LENGTH_MAX = 30;
    public const int RGBA_MAX = 255;
    public const int DOT_MAX = 8;

    public static ReticleProfile Load()
    {
        ReticleProfile loaded = FileManager.instance.LoadGameObjectBinary<ReticleProfile>(filename);
        if (loaded == default(ReticleProfile)) // default == null, failed load
            loaded = new ReticleProfile();

        return loaded;
    }
    public void Save()
    {
        FileManager.instance.SaveGameObjectBinary(this, filename);
    }

    [Range(0, OFFSET_MAX)]
    public int lineOffset;

    [Range(0, WIDTH_MAX)]
    public int lineWidth;

    [Range(0, LENGTH_MAX)]
    public int lineLength;

    [Range(0, DOT_MAX)]
    public int dotSize;

    [Range(0, RGBA_MAX)]
    public int r;

    [Range(0, RGBA_MAX)]
    public int g;

    [Range(0, RGBA_MAX)]
    public int b;

    [Range(0, RGBA_MAX)]
    public int a;

    public ReticleProfile()
    {
        lineOffset = 2;
        lineWidth = 2;
        lineLength = 4;
        dotSize = 0;

        r = 167;
        g = 242;
        b = 255;
        a = 255;
    }

    public Color color
    {
        get
        {
            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }
    }
}
