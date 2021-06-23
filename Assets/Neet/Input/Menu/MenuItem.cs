using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.Input;
using System;
using UnityEngine.Events;

namespace Neet.Input
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(ColliderSizeAdjuster))]
    public class MenuItem : MonoBehaviour
    {
        public MenuItemOverrides snapOverrides;

        public UnityEvent OnSelect;
        public UnityEvent OnCursorEnter;
        public UnityEvent OnCursorExit;

        [HideInInspector]
        public List<Collider2D> collisions;

        // possible feature?
        // public GamepadControl shortcut; 

        private void Awake()
        {
            collisions = new List<Collider2D>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collisions.Add(collision);

            collision.GetComponentInParent<Cursor>()?.Highlight(this);

            OnCursorEnter?.Invoke();
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            collisions.Remove(collision);

            collision.GetComponentInParent<Cursor>()?.DeHighlight(this);

            OnCursorExit?.Invoke();
        }

        public void PrintSomething()
        {
            print(gameObject.name + " was selected");
        }

        public void Select()
        {
            OnSelect?.Invoke();
        }
    }
}