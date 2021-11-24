using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using EventSystems = UnityEngine.EventSystems;


namespace Neat.Music
{

    class RaycastUI : MonoBehaviour
    {
        // raycast stuff
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
        private bool RaycastFret(out Fret f)
        {
            var results = _RaycastUI();

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
        private List<RaycastResult> _RaycastUI()
        {
            var pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = UnityEngine.Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);
            return results;
        }
        // ----
    }
}