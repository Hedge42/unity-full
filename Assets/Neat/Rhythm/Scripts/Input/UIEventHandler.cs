using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Neat.Music
{
    public abstract class UIEventHandler : MonoBehaviour, IPointerClickHandler,
        IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler,
        IDragHandler, IInitializePotentialDragHandler, IEndDragHandler
    {
        // this probably shouldn't be the way it is
        public virtual void OnDrag(PointerEventData eventData) { }
        public virtual void OnEndDrag(PointerEventData eventData) { }
        public virtual void OnInitializePotentialDrag(PointerEventData eventData) { }
        public virtual void OnPointerClick(PointerEventData eventData) { }
        public virtual void OnPointerDown(PointerEventData eventData) { }
        public virtual void OnPointerEnter(PointerEventData eventData) { }
        public virtual void OnPointerExit(PointerEventData eventData) { }
    }
}
