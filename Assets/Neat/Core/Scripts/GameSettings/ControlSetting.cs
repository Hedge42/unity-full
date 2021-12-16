using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.GameManager;

[System.Serializable]
public class ControlSetting
{
    public static readonly string fileNameJSON = "controlSettings.json";

    public const int DPI_MIN = 100;
    public const int DPI_MAX = 16000;
    public const float DIST_MIN = .5f;
    public const float DIST_MAX = 36f;

    public static ControlSetting Load()
    {
        var obj = FileManager.instance.LoadGameObjectJSON<ControlSetting>(fileNameJSON);
        if (obj == null)
            obj = new ControlSetting();
        return obj;
    }
    public void SaveJSON()
    {
        FileManager.instance.SaveGameObjectJSON(this, fileNameJSON);
    }

    public int dpi = 800;
    public float distance = 23;
}
