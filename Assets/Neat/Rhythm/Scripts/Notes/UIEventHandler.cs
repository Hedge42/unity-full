using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Neat.Music
{
    public class UIEventHandler : MonoBehaviour, IPointerClickHandler,
        IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler,
        IDragHandler, IInitializePotentialDragHandler
    {
        public virtual void OnDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public virtual void OnInitializePotentialDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
    }
}
