using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.File;

[System.Serializable]
public class ControlSetting
{
    public static readonly string fileNameJSON = "controlSettings.json";
    public static readonly string fileNameBinary = "controlSettings.sav";

    public const int DPI_MIN = 100;
    public const int DPI_MAX = 16000;
    public const float DIST_MIN = .5f;
    public const float DIST_MAX = 36f;

    public static ControlSetting Load()
    {
        //var obj = FileManager.instance.LoadGameObjectJSON<ControlSetting>(fileNameJSON);
        var obj = FileManager.instance.LoadGameObjectBinary<ControlSetting>(fileNameBinary);
        if (obj == null)
            obj = new ControlSetting();
        return obj;
    }
    public void SaveJSON()
    {
        FileManager.instance.SaveGameObjectJSON(this, fileNameJSON);
    }
    public void SaveBinary()
    {
        FileManager.instance.SavePersistentGameObjectBinary(this, fileNameBinary);
    }

    public int dpi = 800;
    public float distance = 14.61039f;
}
