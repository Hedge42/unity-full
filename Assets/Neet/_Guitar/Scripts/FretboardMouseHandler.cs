using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    // https://forum.unity.com/threads/how-to-raycast-onto-a-unity-canvas-ui-image.855259/

    private PointerEventData pointerEventData;
    private GraphicRaycaster _raycaster;
    public GraphicRaycaster raycaster
    {
        get
        {
            if (_raycaster == null)
                _raycaster = GetComponent<GraphicRaycaster>();
            return _raycaster;
        }
    }

    private EventSystem _eventSystem;
    private EventSystem eventSystem
    {
        get
        {
            if (_eventSystem == null)
                _eventSystem = GameObject.FindObjectOfType<EventSystem>();
            return _eventSystem;
        }
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (RaycastFret(out Fret f))
        //        f.ToggleMode();
        //}
        //else if (Input.GetMouseButtonDown(1))
        //{
        //    if (RaycastFret(out Fret f))
        //        f.Hide();
        //}

    }

    private bool RaycastFret(out Fret f)
    {
        var results = RaycastUI();

        f = null;
        foreach (var result in results)
        {
            var fretUI = result.gameObject.GetComponent<FretUI>();
            if (fretUI != null)
            {
                f = fretUI.fret;
                return true;
            }
        }
        return false;
    }
    private List<RaycastResult> RaycastUI()
    {
        var pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        return results;
    }
}
