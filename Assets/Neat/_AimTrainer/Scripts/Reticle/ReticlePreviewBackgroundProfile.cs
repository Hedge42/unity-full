using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.GameManager;

[System.Serializable]
public class ReticlePreviewBackgroundProfile
{
    private const string filename = "reticlebackground.sav";

    public static ReticlePreviewBackgroundProfile Load()
    {
        var profile = FileManager.instance
            .LoadGameObjectBinary<ReticlePreviewBackgroundProfile>(filename);

        if (profile == default(ReticlePreviewBackgroundProfile))
            profile = new ReticlePreviewBackgroundProfile();

        return profile;
    }

    public void Save()
    {
        FileManager.instance.SavePersistentGameObjectBinary(this, filename);
    }

    public int r;
    public int g;
    public int b;

    public ReticlePreviewBackgroundProfile()
    {
        r = 50;
        g = 50;
        b = 50;
    }

    public Color color
    {
        get
        {
            return new Color(r / 255f, g / 255f, b / 255f);
        }
    }
}
