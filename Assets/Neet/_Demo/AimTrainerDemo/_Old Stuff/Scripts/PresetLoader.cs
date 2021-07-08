using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Neet.File;
using UnityEngine.UI;
using TMPro;

public class PresetLoader : MonoBehaviour
{
    public string directory;
    public Transform container;
    public Button prefab;
    public ReTargetSettingsUI ui;

    private void Start()
    {
        LoadPresets();
    }
    public void SelectPreset(TargetSetting ts)
    {
        ui.LoadSetting(ts);
    }
    public void LoadPresets()
    {
        string[] tsFiles = Directory.GetFiles(
            directory, "*.json", SearchOption.TopDirectoryOnly);

        var sList = new List<TargetSetting>();

        foreach (var file in tsFiles)
        {
            var s = FileManager.instance.DeserializeJson<TargetSetting>(file);
            if (s == null)
                print("Couldn't deserialize " + file);
            else
            {
                print("Deserialized " + s.ToString());

                Button b = Instantiate(prefab, container).GetComponent<Button>();
                b.onClick.AddListener(delegate { SelectPreset(s); });
                b.GetComponentInChildren<TextMeshProUGUI>().text = file;
            }
        }
    }
}
