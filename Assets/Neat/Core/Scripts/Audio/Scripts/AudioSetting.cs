using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.GameManager;

[System.Serializable]
public class AudioSetting
{
    // these 3 members should be on all savable objects
    // better method? abstract classes don't work
    public static readonly string fileNameJSON = "audioSettings.json";
    public static readonly string fileNameBinary = "audioSettings.sav";

    public static AudioSetting Load()
    {
        //var obj = FileManager.instance.LoadGameObjectBinary<AudioSetting>(fileNameBinary);
        var obj = FileManager.instance.LoadGameObjectJSON<AudioSetting>(fileNameJSON);
        if (obj == null)
            obj = new AudioSetting();
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

    // values
    public float masterVolume = .5f;
    public float sfxVolume = 1f;
    public float musicVolume = 1f;
}
