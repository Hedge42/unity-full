using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDebugger : MonoBehaviour
{
    public bool debug;

    void Update()
    {
        //check if the left mouse has been pressed down this frame
        if (debug && Input.GetMouseButtonDown(0))
        {


            print(EventSystem.current?.currentSelectedGameObject?.name);
        }
    }
}
