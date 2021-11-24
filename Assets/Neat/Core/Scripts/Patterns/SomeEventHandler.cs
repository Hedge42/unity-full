using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Neat.Experimental
{
    public class SomeEventHandler : MonoBehaviour, IPointerClickHandler,
        IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler,
        IDragHandler, IInitializePotentialDragHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
    }
}
