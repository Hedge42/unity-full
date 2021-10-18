using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class EventTest : MonoBehaviour
{
    public event Action onTrigger;

    private void Start()
    {
        onTrigger += Test;
        print(onTrigger.Method);
        onTrigger -= Something.Test;
        onTrigger += Something.Test;
        print(onTrigger.Method);


        print("sending event..");
        onTrigger.Invoke();
    }

    void Test()
    {
        print("idk");
    }

    void Find()
    {
        // ??????
        // GetComponentInParent<>
    }

    private class Something : MonoBehaviour
    {
        public static void Test()
        {
            print("Something");
        }
    }
}
