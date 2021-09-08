using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class ShaderDebugger : MonoBehaviour
{
    [Button]
    public void PrintValue()
    {
        print(GetComponent<Image>().material.GetFloat("alpha"));
    }
}


