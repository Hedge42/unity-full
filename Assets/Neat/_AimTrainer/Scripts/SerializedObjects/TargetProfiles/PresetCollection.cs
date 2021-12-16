using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.GameManager;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class PresetCollection
{
    // public static readonly string fileNameBinary = "trainerPresets.sav";
    public static readonly string fileNameJSON = "trainerPresets.json";

    public List<PresetProfile> items;

    public static PresetCollection loaded = null;
    private static int selectedIndex = -1;
    private int lastLoadedIndex = 0;

    public List<string> names;

    public static int SelectedIndex
    {
        // changing this value changes the lastLoadedIndex
        get
        {
            return selectedIndex;
        }
        set
        {
            selectedIndex = value;
            if (selectedIndex >= 0 && loaded?.items.Count > 0)
                loaded.lastLoadedIndex = selectedIndex;
        }
    }


    public static PresetProfile currentItem
    {
        get
        {
            try
            {
                return loaded.items[SelectedIndex];
            }
            catch
            {
                Debug.Log("Index out of range: " + SelectedIndex.ToString() + "/"
                    + loaded.items.Count.ToString());
                return null;
            }
        }
    }


    public List<string> GetNames()
    {
        List<string> list = new List<string>();
        foreach (var item in items)
            list.Add(item.name);

        return list;
    }

    public PresetCollection()
    {
        items = new List<PresetProfile>();
        items.Add(new PresetProfile());
    }

    public static PresetCollection Load()
    {
        // need to ensure the collection is valid when it loads

        PresetCollection loaded = FileManager.instance.LoadGameObjectJSON<PresetCollection>(fileNameJSON);
        if (loaded == default(PresetCollection)) // null?
            loaded = new PresetCollection();


        loaded.names = loaded.GetNames();
        PresetCollection.loaded = loaded;
        SelectedIndex = loaded.lastLoadedIndex; // ??
        PresetProfile.current = currentItem; // ??
        return loaded;
    }
    public void Save()
    {
        FileManager.instance.SaveGameObjectJSON(this, fileNameJSON);
    }

    // adding and removing
    public void RemovePreset(PresetProfile p)
    {
        try
        {
            int index = items.IndexOf(p);
            items.Remove(p);
            AdjustSelectedIndex(index);
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
    }
    public void RemovePreset(int index)
    {
        try
        {
            items.RemoveAt(index);
            AdjustSelectedIndex(index);
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
    }
    private void AdjustSelectedIndex(int last)
    {
        int newIndex = last - 1;
        if (newIndex < 0)
            newIndex = 0;

        SelectedIndex = newIndex;
        PresetProfile.current = currentItem;
    }

    // these might be better to use in some places
    public static void Select(PresetProfile profile)
    {
        // should return -1 if not in the list
        int index = loaded.items.IndexOf(profile);

        // property set does stuff
        SelectedIndex = index;

        // not sure if this is a good idea
        PresetProfile.current = currentItem;
    }
    public static void Select(int profileIndex)
    {
        if (profileIndex >= 0 && profileIndex < loaded.items.Count)
        {
            // really though
            // the property does stuff, then we auto set the current profile
            // neither of those things are well-handled. 
            SelectedIndex = profileIndex;
            PresetProfile.current = currentItem;
        }
        else
        {
            Debug.LogError("That index is bad");
        }
    }


    public bool AddPreset(PresetProfile p)
    {
        bool valid = true;
        foreach (var i in items)
        {
            if (p.name.Equals(i.name))
                valid = false;
        }

        if (valid)
        {
            items.Add(p);
            names.Add(p.name);
        }
        else
            Debug.LogError("Cannot save preset with same name!");

        return valid;
    }

    public PresetCollection Clone()
    {
        // https://levelup.gitconnected.com/5-ways-to-clone-an-object-in-c-d1374ec28efa
        using (var ms = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, this);
            ms.Seek(0, SeekOrigin.Begin);
            return (PresetCollection)formatter.Deserialize(ms);
        }
    }
}
