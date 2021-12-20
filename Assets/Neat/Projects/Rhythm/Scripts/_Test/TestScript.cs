using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Neat/TestObj")]
public class TestObj : ScriptableObject
{
    private Test obj;

    public string something;
    public float value;



    private void Awake()
    {
        Debug.Log("Scriptable object Awake", this);
    }
    private void OnEnable()
    {
        Debug.Log("Scriptable object Enable", this);
    }

    private void OnValidate()
    {
        // obj.something = something;
        //obj.value = value;
    }
}

[System.Serializable]
public class Test 
{
    public string something;
    public float value;
}

public class TestScript : MonoBehaviour
{
    public TestObj obj;

    public void InstantiateEm()
    {
        obj = ScriptableObject.CreateInstance<TestObj>();
        obj.name = "Bob";
    }
}
