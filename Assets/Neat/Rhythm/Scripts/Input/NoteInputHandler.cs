using Neat.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Neat.Music
{
    using Input = UnityEngine.Input;
    using KeyCode = UnityEngine.KeyCode;
    public class NoteInputHandler : UIEventHandler, InputState
    {
        private static InputState borrowed;

        private ChartController _controller;
        public ChartController controller
        {
            get
            {
                if (_controller == null)
                    _controller = ui.overlay.controller;
                return _controller;
            }
        }
        private KeyOverlay _overlay;
        public KeyOverlay overlay
        {
            get
            {
                if (_overlay == null)
                    _overlay = ui.overlay;
                return _overlay;
            }
        }
        private NoteUI _ui;
        public NoteUI ui
        {
            get
            {
                if (!_ui)
                    _ui = GetComponentInParent<NoteUI>();
                return _ui;
            }
        }

        // later: multi-select support
        public static NoteUI selected;

        public override void OnDrag(PointerEventData eventData)
        {
            // print(eventData.pointerCurrentRaycast.screenPosition);
        }
        public override void OnPointerDown(PointerEventData eventData)
        {

        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            print("Clicked " + ui.note.FullFullName());
            Select();
        }


        // input
        public void GetInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var selected = EventSystem.current.currentSelectedGameObject;

                if (selected != this.gameObject)
                {
                    print("Cancelling edit...");
                    Deselect();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                print("Applying...");
                Deselect();
            }

            HandleMouseWheel();
        }
        private void HandleMouseWheel()
        {
            // or left-right keys
            var wheel = Input.mouseScrollDelta.y;
            if (Mathf.Abs(wheel) > .1f)
            {
                FretScroll((int)wheel);
            }
        }
        private void PrintMousePosition()
        {
            print("Mouse @ " + Input.mousePosition);
        }
        private void FretScroll(int value)
        {
            // feeling like this logic shouldn't be here
            int newFret = ui.note.fret - value;
            newFret = Mathf.Clamp(newFret, 0, 24);

            int delta = newFret - ui.note.fret;
            ui.note.fret += delta;
            ui.note.value += delta;

            ui.UpdateText();
        }

        public void Select()
        {
            print("Selecting " + ui.note.FullFullName());

            Deselect();
            selected = ui;
            controller.states.SetInput(this);
            ui.ToggleHighlight(true);

        }
        public static void Deselect()
        {
            // deselect current
            if (selected != null)
            {
                print("Deselecting " + selected.gameObject.name);
                selected.overlay.controller.states.UpdateInput();
                selected.ToggleHighlight(false);
            }
        }

       
    }
}
