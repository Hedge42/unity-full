using System;
using UnityEngine;
using Neet.File;
using System.IO;
using System.Collections.Generic;

[Serializable]
public class TargetSetting
{
    /*This class should include ALL possible target settings
     Even if a particular setting isn't irrelevant for some spawners
    
     Needed so that the setting can be seralized*/

    public static TargetSetting current;
    public static string directory
    {
        get
        {
            var path = FileManager.instance.PersistentDataPath
                + "/TargetPresets";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }
    }
    public const string ext = ".ts";

    public string name;
    public Type spawnerType;

    // challenge
    public bool isTimeLimit;
    public int challengeLimit;
    public int targetLimit;
    public int timeLimit;

    // color
    public Color color;
    public Color trackingColor;

    // size
    public float radius;
    public float distance;
    public float distanceMin;
    public float distanceMax;

    // angles
    public float angleMin;
    public float angleMax;
    public float xAngleMin;
    public float xAngleMax;
    public float yAngleMin;
    public float yAngleMax;

    // movement
    public float accelMin;
    public float accelMax;
    public float speedMin;
    public float speedMax;
    public float secondsPerTurnMin;
    public float secondsPerTurnMax;

    // timings
    public float delay;
    public float delayMin;
    public float delayMax;
    public float timeoutTime;
    public float secondsPerTurn;
    public float lifespan;
    public float lifespanMax;
    public float lifespanMin;

    // bool
    public bool canTimeout;
    public bool showTurnIndicator;
    public bool resetToCenter;
    public bool showCenterLines;

    public string SaveJson()
    {
        var fileName = FileManager.instance.GetUniqueFilename(directory, name + ext);
        var path = FileManager.instance.SerializeJson(this, directory + "/", name);
        Debug.Log("Saved " + name + " to " + path);
        return path;
    }
    public string SaveBinary()
    {
        var fileName = FileManager.instance.GetUniqueFilename(directory, name + ext);
        var path = directory + "/" + fileName;

        FileManager.instance.SerializeBinary(this, path);
        Debug.Log("Saved " + name + " to " + path);
        return path;
    }

    public static TargetSetting[] LoadJson(Type spawnerType)
    {
        var valid = new List<TargetSetting>();
        var files = Directory.GetFiles(directory);
        foreach (var file in files)
        {
            var ts = FileManager.instance.DeserializeJson<TargetSetting>(file);
            if (ts.spawnerType == spawnerType)
                valid.Add(ts);
        }

        return valid.ToArray();
    }
}
