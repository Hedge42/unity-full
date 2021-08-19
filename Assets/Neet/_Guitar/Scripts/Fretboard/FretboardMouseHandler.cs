using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[ExecuteAlways]
public class FretboardMouseHandler : MonoBehaviour
{
    private FretboardUI _ui;
    private FretboardUI ui
    {
        get
        {
            //return GetComponent<FretboardUI>();

            if (_ui == null)
                _ui = GetComponent<FretboardUI>();
            return _ui;
        }
        set
        {
            _ui = value;
        }
    }



    public event Action onFretClick;
    public event Action onEdgeClick;

    private void Update()
    {

    }


    
}
