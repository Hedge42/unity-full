using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    public Dictionary<string, List<KeyCode>> keyDic;

    [HideInInspector]
    public List<string> keys;
    [HideInInspector]
    public List<List<KeyCode>> values;

    public void RebuildDic()
    {
        Dictionary<string, List<KeyCode>> temp = new Dictionary<string, List<KeyCode>>();
        for (int i = 0; i < keys.Count; i++)
        {
            temp.Add(keys[i], values[i]);
        }
        keyDic = temp;
    }
}